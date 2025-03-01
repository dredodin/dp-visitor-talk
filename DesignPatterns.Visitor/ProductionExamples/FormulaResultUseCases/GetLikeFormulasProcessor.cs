using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace ProductionExamples
{
    internal sealed class GetLikeFormulasProcessor
    {
        private static readonly Regex _iGetBenchmarkRegex = new(@"iGetBenchmark\(");
        private static readonly Regex _iGetCashRegex = new(@"iGetCash[T]?\(");
        private static readonly Regex _iGetPerformanceRegex = new(@"iGetPerf|iGetIRR|iGetAgg\(");

        private readonly FormulasStorage _iGetLikeFormulasStorage;
        private readonly IFormulaBatchRepository _formulaBatchRepository;
        private readonly ILogger<GetLikeFormulasProcessor> _logger;

        public GetLikeFormulasProcessor(
            IFormulaBatchRepository formulaBatchRepository,
            ILogger<GetLikeFormulasProcessor> logger)
        {
            _iGetLikeFormulasStorage = new([_iGetBenchmarkRegex, _iGetCashRegex, _iGetPerformanceRegex]);
            _formulaBatchRepository = formulaBatchRepository;
            _logger = logger;
        }

        public async void AsyncRefresh(bool isTransform)
        {
            try
            {
                if (isTransform)
                    await TransformAsync().ConfigureAwait(false);

                await CalculateAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async Task CalculateAsync()
        {
            var toRefresh = GetRequestsToRefresh()
                .Select(x => x.AcceptResultVisitor(FormulaResultVisitor.Process))
                .ToArray();

            if (toRefresh.Length is 0)
                return;

            _iGetLikeFormulasStorage.UpdateFormulas(toRefresh);

            var results = await _formulaBatchRepository
                .CalculateAsync(toRefresh.Select(x => x.Formula), CancellationToken.None)
                .ConfigureAwait(false);

            var formulasWithResults = toRefresh
                .Zip(results, (original, calculated) => original.AcceptResultVisitor(new FormulaResultVisitor(calculated)))
                .ToArray();

            _iGetLikeFormulasStorage.UpdateFormulas(formulasWithResults);

            _logger.LogInformation("Updated {FormulasCount} iGet(Like)formulas", formulasWithResults.Length);
        }

        private IEnumerable<FormulaWithResult> GetRequestsToRefresh()
        {
            throw new NotImplementedException();
        }

        private Task TransformAsync()
        {
            throw new NotImplementedException();
        }
    }
}
