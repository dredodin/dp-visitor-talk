using LanguageExt;
using static LanguageExt.Prelude;

namespace ProductionExamples;

public interface IFormulaResult
{
    T Accept<T>(IFormulaResultVisitor<T> visitor, IFormula formula);
}

public sealed record ErrorFormulaResult(IReadOnlyCollection<string> Errors) : IFormulaResult
{
    private string? _errorMessage;

    public ErrorFormulaResult(string error) : this(new[] { error }) { }

    public static Option<ErrorFormulaResult> TryCreate(IEnumerable<string> errors)
    {
        if (!errors.Any())
            return None;

        return new ErrorFormulaResult(errors.ToHashSet());
    }

    public string GetErrorText() => _errorMessage ??= string.Join(", ", Errors);

    public T Accept<T>(IFormulaResultVisitor<T> visitor, IFormula formula) => visitor.Visit(this, formula);
}

public sealed record NotSyncFormulaResult : IFormulaResult
{
    public static NotSyncFormulaResult Instance { get; } = new();

    public T Accept<T>(IFormulaResultVisitor<T> visitor, IFormula formula) => visitor.Visit(this, formula);
}

public sealed record ProcessingResult : IFormulaResult
{
    public T Accept<T>(IFormulaResultVisitor<T> visitor, IFormula formula) => visitor.Visit(this, formula);
}

public sealed record SubmitFormulaResult(IFormulaResult Actual) : IFormulaResult
{
    public T Accept<T>(IFormulaResultVisitor<T> visitor, IFormula formula) => visitor.Visit(this, formula);
}

public sealed record TransformFormulaResult(IFormulaResult Actual, TransformFormulaResult.Types Type) : IFormulaResult
{
    public enum Types
    {
        None = 0,
        Hardcode = 1,
        ConvertGetToPut = 2,
        ConvertPutToGet = 4,
        ChangeValueToDelete = 8,
        ChangeValueToClear = 16,
        ChangeValueToNull = 32,
        AnyChangeValue = ChangeValueToDelete | ChangeValueToClear | ChangeValueToNull
    }

    public T Accept<T>(IFormulaResultVisitor<T> visitor, IFormula formula) => visitor.Visit(this, formula);
}

public sealed record ValueFormulaResult(object Value) : IFormulaResult
{
    public T Accept<T>(IFormulaResultVisitor<T> visitor, IFormula formula) => visitor.Visit(this, formula);
}

public sealed record ArrayFormulaResult : IFormulaResult
{
    public bool IsTransposed { get; private set; }
    public object[,] ValuesWithRespectToTranspose { get; private set; } = new object[0,0];

    public static IFormulaResult Create(
       ArrayFormula formula,
       int rowsCount,
       int columnsCount,
       IEnumerable<IList<object>> values)
    {
        var isTransposed = formula.IsTransposed();
        var valuesArray = values.ToArray();

        var matrix = Array2.CreateExcelMatrix(rowsCount, columnsCount, isTransposed, (col, row) => valuesArray[row][col]);
        return new ArrayFormulaResult
        {
            IsTransposed = isTransposed,
            ValuesWithRespectToTranspose = matrix,
        };
    }

    public int GetRowsCount() => ValuesWithRespectToTranspose.GetLength(0);

    public int GetColumnsCount() => ValuesWithRespectToTranspose.GetLength(1);

    public T Accept<T>(IFormulaResultVisitor<T> visitor, IFormula formula) => visitor.Visit(this, formula);
}