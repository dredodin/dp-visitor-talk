

namespace ProductionExamples.Api;

public interface IExcelServiceClient : System.IDisposable
{
    Task<ArrayFormulaResult> CalculateCashTransactionsAsync(CashTransactionsArrayFormula apiRequest, CancellationToken ct);
    Task<ArrayFormulaResult> CalculateExchangeRatesAsync(ExchangeRatesArrayFormula apiRequest, CancellationToken ct);
}

public class CashTransactionsArrayFormula()
{
    public int PageSize { get; internal set; }
    public TemplateData? TemplateData { get; internal set; }
    public int Page { get; internal set; }
}

public class ExchangeRatesArrayFormula()
{
    public TemplateData? TemplateData { get; internal set; }
}

public class ArrayFormulaResult()
{
    public int Pages { get; internal set; }
    public int Page { get; internal set; }
    public string[] EnrichedErrors { get; internal set; } = [];
    public List<IList<object>>? Values { get; internal set; }
    public int TotalCount { get; internal set; }
    public int ColumnsCount { get; internal set; }
}

public class TemplateData();