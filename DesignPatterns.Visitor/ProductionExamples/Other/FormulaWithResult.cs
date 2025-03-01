using LanguageExt;

namespace ProductionExamples;

public sealed record FormulaWithResult(IFormula Formula, IFormulaResult Result)
{
    public Option<FormulaWithResult> AcceptFormulaVisitor<T>(IFormulaVisitor<Option<T>> visitor) where T : IFormula
    => Formula.Accept(visitor).Map(x => new FormulaWithResult(x, Result));

    public FormulaWithResult AcceptResultVisitor<T>(IFormulaResultVisitor<T> visitor) where T : IFormulaResult
        => new(Formula, Result.Accept(visitor, Formula));

    public bool HasError() => Formula.Error.IsSome || Result.Accept(IsVisitor<ErrorFormulaResult>.Instance, Formula);

    public bool IsNotSync() => Result.Accept(IsVisitor<NotSyncFormulaResult>.Instance, Formula);

    public bool IsProcessing() => Result.Accept(IsVisitor<ProcessingResult>.Instance, Formula);

    public bool IsTransform() => Result.Accept(IsVisitor<TransformFormulaResult>.Instance, Formula);

    public bool IsSubmit() => Result.Accept(IsVisitor<SubmitFormulaResult>.Instance, Formula);

    public bool IsAllowTransform() => (Formula.Error.IsSome && !Result.Accept(IsVisitor<NotSyncFormulaResult>.Instance, Formula))
        || Result is TransformFormulaResult { Actual: ValueFormulaResult or ArrayFormulaResult or ErrorFormulaResult };
}