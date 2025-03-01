namespace ProductionExamples;

public interface IFormulaBatchRepository
{
    Task<IEnumerable<IFormulaResult>> CalculateAsync(IEnumerable<IFormula> formulas, CancellationToken cancellationToken);

    Task<IEnumerable<IFormulaResult>> ValidateAsync(IEnumerable<IFormula> formulas, string clientDateFormat, CancellationToken cancellationToken);

    Task<IEnumerable<IFormulaResult>> SubmitAsync(IEnumerable<IFormula> formulas, string clientDateFormat, CancellationToken cancellationToken);
}
