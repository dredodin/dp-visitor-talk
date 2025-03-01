namespace ProductionExamples;

public enum XlError : int
{
    xlErrorNull = 0,
    xlErrorDiv0 = 7,
    xlErrorValue = 15,
    xlErrorRef = 23,
    xlErrorName = 29,
    xlErrorNum = 36,
    xlErrorNA = 42,
    xlErrGettingData = 43
}

public static class XlErrorExtensions
{
    private const string xlErrorNull = "#NULL!";
    private const string xlErrorDiv0 = "#DIV/0!";
    private const string xlErrorValue = "VALUE!";
    private const string xlErrorRef = "#REF!";
    private const string xlErrorName = "NAME?";
    private const string xlErrorNum = "#NUM!";
    private const string xlErrorNA = "#N/A";
    private const string xlErrGettingData = "#GETTING_DATA";
    public static XlError? ParseXlError(this string that) => that switch
    {
        xlErrorNull => XlError.xlErrorNull,
        xlErrorDiv0 => XlError.xlErrorDiv0,
        xlErrorValue => XlError.xlErrorValue,
        xlErrorRef => XlError.xlErrorRef,
        xlErrorName => XlError.xlErrorName,
        xlErrorNum => XlError.xlErrorNum,
        xlErrorNA => XlError.xlErrorNA,
        xlErrGettingData => XlError.xlErrGettingData,
        _ => null
    };
    
    public static bool ParameterIsNotSpecified(this string value)
        => string.IsNullOrWhiteSpace(value) || value.ParseXlError() is XlError.xlErrorNA;
}
