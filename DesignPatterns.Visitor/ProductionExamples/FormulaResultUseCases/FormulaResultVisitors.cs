using System.Globalization;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;
using static LanguageExt.Prelude;

namespace ProductionExamples;

public interface IFormulaResultVisitor<out T>
{
    T Visit(NotSyncFormulaResult formulaResult, IFormula formula);
    T Visit(ValueFormulaResult formulaResult, IFormula formula);
    T Visit(TransformFormulaResult formulaResult, IFormula formula);
    T Visit(SubmitFormulaResult formulaResult, IFormula formula);
    T Visit(ProcessingResult formulaResult, IFormula formula);
    T Visit(ArrayFormulaResult arrayFormulaResult, IFormula formula);
    T Visit(ErrorFormulaResult formulaResult, IFormula formula);
}

public sealed record CancelTransformVisitor : IFormulaResultVisitor<IFormulaResult>
{
    public static CancelTransformVisitor Instance { get; } = new();

    public IFormulaResult Visit(NotSyncFormulaResult formulaResult, IFormula formula) => formulaResult;

    public IFormulaResult Visit(ValueFormulaResult formulaResult, IFormula formula) => formulaResult;

    public IFormulaResult Visit(TransformFormulaResult formulaResult, IFormula formula) => formulaResult.Actual;

    public IFormulaResult Visit(SubmitFormulaResult formulaResult, IFormula formula) => NotSyncFormulaResult.Instance;

    public IFormulaResult Visit(ProcessingResult formulaResult, IFormula formula) => formulaResult;

    public IFormulaResult Visit(ArrayFormulaResult formulaResult, IFormula formula) => formulaResult;

    public IFormulaResult Visit(ErrorFormulaResult formulaResult, IFormula formula) => formulaResult;
}

public sealed class CompareFormulaResultVisitor : IFormulaResultVisitor<bool>
{
    private readonly PrepareFormulaValueVisitor _prepareValue;
    private readonly object _other;

    public CompareFormulaResultVisitor(Func<string> getDateFormat, object other)
    {
        _prepareValue = new(getDateFormat);
        _other = other;
    }

    public bool Visit(NotSyncFormulaResult formulaResult, IFormula formula) => _prepareValue.Visit(formulaResult, formula).Equals(_other);

    public bool Visit(ValueFormulaResult formulaResult, IFormula formula)
    {
        var formulaResultValue = formulaResult.Accept(_prepareValue, formula);
        var otherValue = new ValueFormulaResult(_other).Accept(_prepareValue, formula);

        return Equals(formulaResultValue, otherValue);
    }

    public bool Visit(TransformFormulaResult formulaResult, IFormula formula) => formulaResult.Actual.Accept(this, formula);

    public bool Visit(SubmitFormulaResult formulaResult, IFormula formula) => formulaResult.Actual.Accept(this, formula);

    public bool Visit(ProcessingResult formulaResult, IFormula formula) => _prepareValue.Visit(formulaResult, formula).Equals(_other);

    public bool Visit(ArrayFormulaResult formulaResult, IFormula formula) => _prepareValue.Visit(formulaResult, formula).Equals(_other);

    public bool Visit(ErrorFormulaResult formulaResult, IFormula formula) => _prepareValue.Visit(formulaResult, formula).Equals(_other);
}

public sealed class FormulaResultPriorityVisitor : IFormulaResultVisitor<int>
{
    public static FormulaResultPriorityVisitor Instance { get; } = new();

    public int Visit(ValueFormulaResult formulaResult, IFormula formula) => 0;

    public int Visit(ArrayFormulaResult formulaResult, IFormula formula) => 0;

    public int Visit(ErrorFormulaResult formulaResult, IFormula formula) => 1;

    public int Visit(NotSyncFormulaResult formulaResult, IFormula formula) => 2;

    public int Visit(TransformFormulaResult formulaResult, IFormula formula) => 2;

    public int Visit(SubmitFormulaResult formulaResult, IFormula formula) => 2;

    public int Visit(ProcessingResult formulaResult, IFormula formula) => 2;
}

public sealed record FormulaResultVisitor(IFormulaResult Actual) : IFormulaResultVisitor<IFormulaResult>
{
    public static FormulaResultVisitor Unsync { get; } = new(NotSyncFormulaResult.Instance);
    public static FormulaResultVisitor Process { get; } = new(new ProcessingResult());

    public static FormulaResultVisitor Error(params string[] errors) => new(new ErrorFormulaResult(errors));

    public IFormulaResult Visit(NotSyncFormulaResult formulaResult, IFormula formula) => Actual;

    public IFormulaResult Visit(ValueFormulaResult formulaResult, IFormula formula) => Actual;

    public IFormulaResult Visit(TransformFormulaResult formulaResult, IFormula formula) => new TransformFormulaResult(Actual, formulaResult.Type);

    public IFormulaResult Visit(SubmitFormulaResult formulaResult, IFormula formula) => new SubmitFormulaResult(Actual);

    public IFormulaResult Visit(ProcessingResult formulaResult, IFormula formula) => Actual;

    public IFormulaResult Visit(ArrayFormulaResult formulaResult, IFormula formula) => Actual;

    public IFormulaResult Visit(ErrorFormulaResult formulaResult, IFormula formula) => Actual;
}

public sealed class GetArrayFormulaResultVisitor : IFormulaResultVisitor<Option<ArrayFormulaResult>>
{
    public static GetArrayFormulaResultVisitor Instance { get; } = new();
    public Option<ArrayFormulaResult> Visit(NotSyncFormulaResult formulaResult, IFormula formula) => None;
    public Option<ArrayFormulaResult> Visit(ValueFormulaResult formulaResult, IFormula formula) => None;

    public Option<ArrayFormulaResult> Visit(TransformFormulaResult formulaResult, IFormula formula) => formulaResult.Actual.Accept(Instance, formula);
    public Option<ArrayFormulaResult> Visit(SubmitFormulaResult formulaResult, IFormula formula) => None;
    public Option<ArrayFormulaResult> Visit(ProcessingResult formulaResult, IFormula formula) => None;

    public Option<ArrayFormulaResult> Visit(ArrayFormulaResult formulaResult, IFormula formula) => formulaResult;

    public Option<ArrayFormulaResult> Visit(ErrorFormulaResult formulaResult, IFormula formula) => None;
}

public sealed class IsVisitor<T> : IFormulaResultVisitor<bool> where T : IFormulaResult
{
    public static IsVisitor<T> Instance { get; } = new();

    public bool Visit(NotSyncFormulaResult formulaResult, IFormula formula) => formulaResult is T;

    public bool Visit(ValueFormulaResult formulaResult, IFormula formula) => formulaResult is T;

    public bool Visit(TransformFormulaResult formulaResult, IFormula formula) => formulaResult is T || formulaResult.Actual is T;

    public bool Visit(SubmitFormulaResult formulaResult, IFormula formula) => formulaResult is T || formulaResult.Actual is T;

    public bool Visit(ProcessingResult formulaResult, IFormula formula) => formulaResult is T;

    public bool Visit(ArrayFormulaResult formulaResult, IFormula formula) => formulaResult is T;

    public bool Visit(ErrorFormulaResult formulaResult, IFormula formula) => formulaResult is T;
}

public sealed class PrepareFormulaErrorVisitor : IFormulaResultVisitor<Option<string>>
{
    public Option<string> Visit(NotSyncFormulaResult formulaResult, IFormula formula) => formula.Error.Map(x => x.GetErrorText());

    public Option<string> Visit(ValueFormulaResult formulaResult, IFormula formula) => formula.Error.Map(x => x.GetErrorText());

    public Option<string> Visit(TransformFormulaResult formulaResult, IFormula formula) => formula.Error.Map(x => x.GetErrorText());

    public Option<string> Visit(SubmitFormulaResult formulaResult, IFormula formula) => formulaResult.Actual.Accept(this, formula);

    public Option<string> Visit(ProcessingResult formulaResult, IFormula formula) => formula.Error.Map(x => x.GetErrorText());

    public Option<string> Visit(ArrayFormulaResult formulaResult, IFormula formula) => formula.Error.Map(x => x.GetErrorText());

    public Option<string> Visit(ErrorFormulaResult formulaResult, IFormula formula)
    {
        var resultText = formula.Error.Match(
            None: () => formulaResult.Errors,
            Some: x => x.Errors.Concat(formulaResult.Errors));

        return string.Join(", ", resultText);
    }
}

public sealed class PrepareFormulaValueVisitor : IFormulaResultVisitor<object>
{
    private readonly Lazy<string> _dateFormat;
    private readonly PrepareFormulaErrorVisitor _errorVisitor;
    private readonly NotSyncFormulaValueVisitor _notSyncFormulaValueVisitor;

    public PrepareFormulaValueVisitor(string dateFormat) : this(() => dateFormat) { }

    public PrepareFormulaValueVisitor(Func<string> getDateFormat)
    {
        _dateFormat = new Lazy<string>(getDateFormat);
        _errorVisitor = new PrepareFormulaErrorVisitor();
        _notSyncFormulaValueVisitor = new();
    }

    public object Visit(NotSyncFormulaResult formulaResult, IFormula formula)
        => _errorVisitor.Visit(formulaResult, formula).Match(
            () => AdjustExcelOutputValue(formula.Accept(_notSyncFormulaValueVisitor)),
            x => x);

    public object Visit(ValueFormulaResult formulaResult, IFormula formula)
        => _errorVisitor.Visit(formulaResult, formula).Match(() => AdjustExcelOutputValue(formulaResult.Value), x => x);

    public object Visit(TransformFormulaResult formulaResult, IFormula formula)
        => _errorVisitor.Visit(formulaResult, formula).Match(() => formulaResult.Actual.Accept(this, formula), x => x);

    public object Visit(SubmitFormulaResult formulaResult, IFormula formula)
        => _errorVisitor.Visit(formulaResult, formula).Match(
            () => AdjustExcelOutputValue(formula.Accept(_notSyncFormulaValueVisitor)),
            x => x);

    public object Visit(ProcessingResult formulaResult, IFormula formula)
        => _errorVisitor.Visit(formulaResult, formula).Match(
            () => AdjustExcelOutputValue(formula.Accept(_notSyncFormulaValueVisitor)),
            x => x);

    public object Visit(ArrayFormulaResult formulaResult, IFormula formula) => formula switch
    {
        ArrayFormula arrayFormula => GetArrayCaption(formulaResult, arrayFormula),
        _ => _errorVisitor.Visit(formulaResult, formula).Match(
            () => formula.Accept(_notSyncFormulaValueVisitor),
            x => x)
    };

    public object Visit(ErrorFormulaResult formulaResult, IFormula formula)
        => _errorVisitor.Visit(formulaResult, formula).ValueUnsafe();

    private static string GetArrayCaption(ArrayFormulaResult formulaResult, ArrayFormula formula)
    {
        var formulaName = formula.Args.Accept(ArrayFormulaToNameVisitor.Instance);
        var rowsCount = formulaResult.GetRowsCount();
        var columnsCount = formulaResult.GetColumnsCount();

        return $"{formulaName}({rowsCount}, {columnsCount})";
    }

    private object AdjustExcelOutputValue(object value, double? denominator = null) => value switch
    {
        null => string.Empty,
        int x => DivideIfNotNull(Convert.ToDouble(x), denominator),
        long x => DivideIfNotNull(Convert.ToDouble(x), denominator),
        double x => DivideIfNotNull(x, denominator),
        float x => DivideIfNotNull(Convert.ToDouble(x, CultureInfo.InvariantCulture), denominator),
        decimal x => DivideIfNotNull(Convert.ToDouble(x, CultureInfo.InvariantCulture), denominator),
        string x => AdjustExcelOutputStringValue(x, denominator),
        _ => value
    };

    private static object AdjustExcelOutputStringValue(string stringValue, double? denominator = null)
    {
        if (double.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var doubleValue))
            return DivideIfNotNull(doubleValue, denominator);

        if (decimal.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var decimalValue))
            return DivideIfNotNull(Convert.ToDouble(decimalValue, CultureInfo.InvariantCulture), denominator);

        return stringValue;
    }

    private static double DivideIfNotNull(double numerator, double? denominator = null)
        => denominator is null ? numerator : numerator / denominator.Value;
}

public sealed record SubmitFormulaResultVisitor : IFormulaResultVisitor<SubmitFormulaResult>
{
    public static SubmitFormulaResultVisitor Instance { get; } = new();

    public SubmitFormulaResult Visit(NotSyncFormulaResult formulaResult, IFormula formula) => new(formulaResult);

    public SubmitFormulaResult Visit(ValueFormulaResult formulaResult, IFormula formula) => new(formulaResult);

    public SubmitFormulaResult Visit(TransformFormulaResult formulaResult, IFormula formula) => new(formulaResult);

    public SubmitFormulaResult Visit(SubmitFormulaResult formulaResult, IFormula formula) => formulaResult;

    public SubmitFormulaResult Visit(ProcessingResult formulaResult, IFormula formula) => new(formulaResult);

    public SubmitFormulaResult Visit(ArrayFormulaResult formulaResult, IFormula formula) => new(formulaResult);

    public SubmitFormulaResult Visit(ErrorFormulaResult formulaResult, IFormula formula) => new(formulaResult);
}

public sealed record SubmittedFormulaResultVisitor(IFormulaResult Actual) : IFormulaResultVisitor<IFormulaResult>
{
    public IFormulaResult Visit(NotSyncFormulaResult formulaResult, IFormula formula) => Actual;

    public IFormulaResult Visit(ValueFormulaResult formulaResult, IFormula formula) => Actual;

    public IFormulaResult Visit(TransformFormulaResult formulaResult, IFormula formula) => Actual;

    public IFormulaResult Visit(SubmitFormulaResult formulaResult, IFormula formula) => Actual;

    public IFormulaResult Visit(ProcessingResult formulaResult, IFormula formula) => Actual;

    public IFormulaResult Visit(ArrayFormulaResult formulaResult, IFormula formula) => Actual;

    public IFormulaResult Visit(ErrorFormulaResult formulaResult, IFormula formula) => Actual;
}

public sealed record TransformResultVisitor(TransformFormulaResult.Types Type) : IFormulaResultVisitor<TransformFormulaResult>
{
    public static TransformResultVisitor Hardcode { get; } = new(TransformFormulaResult.Types.Hardcode);
    public static TransformResultVisitor ConvertGetToPut { get; } = new(TransformFormulaResult.Types.ConvertGetToPut);
    public static TransformResultVisitor ConvertPutToGet { get; } = new(TransformFormulaResult.Types.ConvertPutToGet);

    public TransformFormulaResult Visit(NotSyncFormulaResult formulaResult, IFormula formula) => new(formulaResult, Type);

    public TransformFormulaResult Visit(ValueFormulaResult formulaResult, IFormula formula) => new(formulaResult, Type);

    public TransformFormulaResult Visit(TransformFormulaResult formulaResult, IFormula formula) => new(formulaResult.Actual, Type);

    public TransformFormulaResult Visit(SubmitFormulaResult formulaResult, IFormula formula) => new(formulaResult, Type);

    public TransformFormulaResult Visit(ProcessingResult formulaResult, IFormula formula) => new(formulaResult, Type);

    public TransformFormulaResult Visit(ArrayFormulaResult formulaResult, IFormula formula) => new(formulaResult, Type);

    public TransformFormulaResult Visit(ErrorFormulaResult formulaResult, IFormula formula) => new(formulaResult, Type);
}
