using AutoMapper;
using Microsoft.Extensions.Logging;

namespace ProductionExamples.UseCases;

internal sealed class ArrayFormulasRepository
{
    private const int _defaultPageSize = 32_000;
    private readonly IMapper _mapper;
    private readonly ITemplateDataProvider _templateDataProvider;
    private readonly ILogger<ArrayFormulasRepository> _logger;
    private readonly Api.IExcelServiceClient _client;

    public ArrayFormulasRepository(
        ITemplateDataProvider templateDataProvider,
        ILogger<ArrayFormulasRepository> logger,
        Api.IExcelServiceClient client,
        IMapper mapper)
    {
        _mapper = mapper;
        _templateDataProvider = templateDataProvider;
        _logger = logger;
        _client = client;
    }

    public async Task<IFormulaResult> CalculateValuesAsync(IFormula formula, CancellationToken ct)
    {
        if (formula is not ArrayFormula arrayFormula)
            return new ErrorFormulaResult(Errors.InvalidFormula);

        try
        {
            return await CalculateValuesAsync(arrayFormula, ct);
        }
        catch (Exception ex)
        {
            return new ErrorFormulaResult(ex.Message);
        }
    }

    private async Task<IFormulaResult> CalculateValuesAsync(ArrayFormula formula, CancellationToken ct)
    {
        var loadPageAsync = formula.Args.Accept(new LoadArrayFormulaVisitor(_client, _mapper, _templateDataProvider, formula.IsTransposed(), ct));
        var firstPage = await loadPageAsync(1).ConfigureAwait(false);
        int pagesCount = firstPage.Pages;
        var pages = new List<Api.ArrayFormulaResult>(pagesCount) { firstPage };
        for (int currentPage = firstPage.Page + 1; currentPage <= pagesCount; currentPage++)
            pages.Add(await loadPageAsync(currentPage).ConfigureAwait(false));

        if (pages.Any(x => x.EnrichedErrors?.Length > 0))
            return new ErrorFormulaResult(pages[0].EnrichedErrors);

        return ArrayFormulaResult.Create(
            formula: formula,
            rowsCount: firstPage.TotalCount,
            columnsCount: firstPage.ColumnsCount,
            values: pages.SelectMany(x => x.Values ?? []));
    }

    private sealed class LoadArrayFormulaVisitor : IArrayFormulaArgsVisitor<Func<int, Task<Api.ArrayFormulaResult>>>
    {
        private readonly Api.IExcelServiceClient _client;
        private readonly IMapper _mapper;
        private readonly ITemplateDataProvider _templateDataProvider;
        private readonly bool _isTransposed;
        private readonly CancellationToken _ct;

        public LoadArrayFormulaVisitor(
            Api.IExcelServiceClient client,
            IMapper mapper,
            ITemplateDataProvider templateDataProvider,
            bool isTransposed,
            CancellationToken ct)
            => (_client, _mapper, _templateDataProvider, _isTransposed, _ct) = (client, mapper, templateDataProvider, isTransposed, ct);

        public Func<int, Task<Api.ArrayFormulaResult>> Visit(CashTransactionsArrayFormulaArgs args)
        {
            var apiRequest = _mapper.Map<Api.CashTransactionsArrayFormula>(args);
            apiRequest.PageSize = _defaultPageSize;
            apiRequest.TemplateData = GetTemplateData();

            return page =>
            {
                apiRequest.Page = page;
                return _client.CalculateCashTransactionsAsync(apiRequest, _ct);
            };
        }

        public Func<int, Task<Api.ArrayFormulaResult>> Visit(ExchangeRatesArrayFormulaArgs args)
        {
            var apiRequest = _mapper.Map<Api.ExchangeRatesArrayFormula>(args);
            apiRequest.TemplateData = GetTemplateData();

            return page => _client.CalculateExchangeRatesAsync(apiRequest, _ct);
        }

        private Api.TemplateData? GetTemplateData()
        {
            var templateData = _templateDataProvider.GetTemplateInActiveContext();
            return templateData?.ToApiTemplateData(GetStubHeaderFormat());
        }

        private string GetStubHeaderFormat() => _isTransposed switch
        {
            false => Constants.ARRAY_FORMULA_STUB_HEADER_FORMAT,
            true => Constants.ARRAY_FORMULA_STUB_HEADER_FORMAT_TRANSPOSED
        };
    }
}
