namespace ProductionExamples;

public static class Errors
{
    public const string InternalServerError = "Internal Server Error.";
    public const string ExcelAddinOutdated = "Excel Add-In is outdated, please upgrade to the latest version.";
    public const string AccessDenied = "Access Denied.";
    public const string InvalidFormula = "Invalid Formula.";
    public const string BenchmarkNameNotValid = "Benchmark Name is not valid.";
    public const string DataItemNotValid = "Data Item is not valid.";
    public const string OwnerNotValid = "Owner is not valid.";
    public const string SubRequestsNotValid = "Sub requests are not valid.";
    public const string BenchmarkTimeHorizonNotValid = "Time Horizon is not valid.";
    public const string BenchmarkDataModeNotValid = "Data Mode is not valid.";
    public const string CurrencyNotValid = "Currency is not valid.";
    public const string BenchmarkVintageYearTypeNotValid = "Vintage Year Type is not valid.";
    public const string EndDateNotValid = "End Date is not valid.";
    public const string ScenarioNotValid = "Scenario is not valid.";
    public const string TransactionTypeNotValid = "Transaction Type is not valid.";
    public const string TransactionDateNotValid = "Transaction Date is not valid.";
    public const string AmountNotValid = "Amount is not valid.";
    public const string ReturnFieldNotValid = "Return field is not valid.";
    public const string StartDateNotValid = "Start Date is not valid.";
    public const string OwnersNotValid = "Owners are not valid.";
    public const string DataItemNotExist = "Data Item does not exist.";
    public const string TransactionIdOrOwnerAndTransactionTypeRequired = "Transaction Id or Fund/Owner and Transaction Type values are required.";
    public const string PathExpressionNotValid = "Invalid path expression/s.";
    public const string DataLineageDocumentIdNotValid = "Document ID must be a number greater than 0.";
    public const string DataLineageDocumentIdRequired = "Document ID is required.";
    public const string DataLineagePageNumberNotValid = "Page Number must be a number greater than 0.";
    public const string DataLineagePageNumberRequired = "Page Number is required.";
    public const string DataLineageLeftCellReferenceRequired = "Cell reference for Left is required.";
    public const string DataLineageTopCellReferenceRequired = "Cell reference for Top is required.";
    public const string DataLineageWidthCellReferenceRequired = "Cell reference for Width is required.";
    public const string DataLineageHeightCellReferenceRequired = "Cell reference for Height is required.";
    public const string SourceCurrencyNotValid = "Source Rate is not valid.";
    public const string TargetCurrencyNotValid = "Destination Rate is not valid.";

    public static string MLCInvestmentRequired(int index) => $"Please select a valid Investment {index}";
}

public static class Constants
{
    public const string NotSynced = "Not synced";
    public const string DELETE_VALUE = "<Delete>";
    public const string CLEAR_VALUE = "<Clear>";
    public const string NULL_STRING_VALUE = "<Null>";

    public const string ARRAY_FORMULA_STUB_HEADER_FORMAT = "{0} - Begin list (1,{1})";
    public const string ARRAY_FORMULA_STUB_HEADER_FORMAT_TRANSPOSED = "{0} - Begin list ({1},1)";
}