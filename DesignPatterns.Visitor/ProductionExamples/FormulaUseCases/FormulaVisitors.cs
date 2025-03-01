using LanguageExt;
using static LanguageExt.Prelude;
using static ProductionExamples.TransformFormulaResult;

namespace ProductionExamples;

public interface IFormulaVisitor<out T>
{
    T Visit(GetBenchmarkFormula formula);
    T Visit(GetCashFormula formula);
    T Visit(PutCashFormula formula);
    T Visit(PutFormula formula);
    T Visit(GetPerformanceFormula formula);
    T Visit(ArrayFormula formula);
}

public sealed class DestructiveFormulaVisitor : IFormulaVisitor<Option<IFormula>>
{
    public static DestructiveFormulaVisitor Instance { get; } = new();
    public Option<IFormula> Visit(GetBenchmarkFormula formula) => None;
    public Option<IFormula> Visit(GetCashFormula formula) => None;
    public Option<IFormula> Visit(PutCashFormula formula) => formula.Amount is Constants.DELETE_VALUE ? formula : Option<IFormula>.None;
    public Option<IFormula> Visit(PutFormula formula) => formula.DataItemValue is Constants.DELETE_VALUE or Constants.CLEAR_VALUE ? formula : Option<IFormula>.None;
    public Option<IFormula> Visit(GetPerformanceFormula formula) => None;
    public Option<IFormula> Visit(ArrayFormula formula) => None;
}

public sealed class FormulaWeightVisitor : IFormulaVisitor<int>
{
    public static int EntityWeight { get; } = 64;
    public static FormulaWeightVisitor Instance { get; } = new();

    public int Visit(GetBenchmarkFormula formula) => 199;

    public int Visit(GetCashFormula formula) => 199;

    public int Visit(PutCashFormula formula) => 211;

    // TODO: update value
    public int Visit(PutFormula formula) => 150;

    public int Visit(GetPerformanceFormula formula) => 10 + formula.PathExpressions.Weight;

    public int Visit(ArrayFormula formula) => 1;
}

public sealed record IsSupportedTransformTypeVisitor(Types Type) : IFormulaVisitor<bool>
{
    public static IsSupportedTransformTypeVisitor GetToPut { get; } = new(Types.ConvertGetToPut);
    public static IsSupportedTransformTypeVisitor PutToGet { get; } = new(Types.ConvertPutToGet);
    public static IsSupportedTransformTypeVisitor ChangeValueForPut { get; } = new(Types.AnyChangeValue);

    public bool Visit(GetBenchmarkFormula formula) => Type is Types.Hardcode;

    public bool Visit(GetCashFormula formula) => Type is Types.Hardcode or Types.ConvertGetToPut;

    public bool Visit(PutCashFormula formula) => Type is Types.ConvertPutToGet;

    public bool Visit(PutFormula formula) => Type is Types.ConvertPutToGet || Types.AnyChangeValue.HasFlag(Type);

    public bool Visit(GetPerformanceFormula formula) => Type is Types.Hardcode;

    public bool Visit(ArrayFormula formula) => Type is Types.Hardcode;
}

public sealed record NotSyncFormulaValueVisitor : IFormulaVisitor<object>
{
    public object Visit(GetBenchmarkFormula formula) => XlError.xlErrGettingData;

    public object Visit(GetCashFormula formula) => XlError.xlErrGettingData;

    public object Visit(PutCashFormula formula) => GetDisplayValue(formula);

    public object Visit(PutFormula formula) => GetDisplayValue(formula);

    public object Visit(GetPerformanceFormula formula) => XlError.xlErrGettingData;

    public object Visit(ArrayFormula formula) => XlError.xlErrGettingData;

    private static string GetDisplayValue(PutCashFormula formula)
    {
        var value = formula.Amount;

        if (IsDelete(value))
            return Constants.DELETE_VALUE;

        if (IsClear(value))
            return Constants.CLEAR_VALUE;

        return value;
    }

    private static string GetDisplayValue(PutFormula putFormula)
    {
        var value = putFormula.DataItemValue;

        if (IsDelete(value))
            return Constants.DELETE_VALUE;

        if (IsClear(value))
            return Constants.CLEAR_VALUE;

        if (IsNull(value))
            return Constants.NULL_STRING_VALUE;

        return value ?? string.Empty;
    }

    private static bool IsDelete(string value) => string.Equals(value, Constants.DELETE_VALUE, StringComparison.OrdinalIgnoreCase);

    private static bool IsClear(string value) => string.Equals(value, Constants.CLEAR_VALUE, StringComparison.OrdinalIgnoreCase);

    private static bool IsNull(string value) => string.Equals(value, Constants.NULL_STRING_VALUE, StringComparison.OrdinalIgnoreCase);
}
