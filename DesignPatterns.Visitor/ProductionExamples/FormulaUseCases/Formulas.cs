using LanguageExt;
using static LanguageExt.Prelude;

namespace ProductionExamples;

public interface IFormula : IEquatable<IFormula>
{
    bool IsT { get; }
    CellLocation FormulaCell { get; }
    Option<ErrorFormulaResult> Error { get; }
    T Accept<T>(IFormulaVisitor<T> visitor);
}

public sealed class GetBenchmarkFormula : IFormula
{
    public GetBenchmarkFormula(
        CellLocation formulaCell,
        string ownerName,
        string investmentName,
        string benchmarkName,
        string benchmarkVintageYear,
        string benchmarkDataItemName,
        string vintageYearType,
        string dataMode,
        string endDate,
        string timeHorizon,
        string currency)
    {
        FormulaCell = formulaCell ?? throw new ArgumentNullException(nameof(formulaCell));
        OwnerName = ownerName;
        InvestmentName = investmentName;
        BenchmarkName = benchmarkName;
        BenchmarkVintageYear = benchmarkVintageYear;
        BenchmarkDataItemName = benchmarkDataItemName;
        VintageYearType = vintageYearType;
        DataMode = dataMode;
        EndDate = endDate;
        TimeHorizon = timeHorizon;
        Currency = currency;

        Error = ErrorFormulaResult.TryCreate(EnumerateErrors());
    }

    public bool IsT => false;
    public CellLocation FormulaCell { get; }
    public Option<ErrorFormulaResult> Error { get; }

    public string OwnerName { get; }
    public string InvestmentName { get; }
    public string BenchmarkName { get; }
    public string BenchmarkVintageYear { get; }
    public string BenchmarkDataItemName { get; }
    public string VintageYearType { get; }
    public string DataMode { get; }
    public string EndDate { get; }
    public string TimeHorizon { get; }
    public string Currency { get; }

    private IEnumerable<string> EnumerateErrors()
    {
        if (BenchmarkName.ParameterIsNotSpecified()) yield return Errors.BenchmarkNameNotValid;
        if (BenchmarkDataItemName.ParameterIsNotSpecified()) yield return Errors.DataItemNotValid;
        if (VintageYearType.ParameterIsNotSpecified()) yield return Errors.BenchmarkVintageYearTypeNotValid;
        if (DataMode.ParameterIsNotSpecified()) yield return Errors.BenchmarkDataModeNotValid;
        if (EndDate.ParameterIsNotSpecified()) yield return Errors.EndDateNotValid;
        if (TimeHorizon.ParameterIsNotSpecified()) yield return Errors.BenchmarkTimeHorizonNotValid;
        if (Currency.ParameterIsNotSpecified()) yield return Errors.CurrencyNotValid;
    }

    public override bool Equals(object? obj) => Equals(obj as GetBenchmarkFormula);

    public bool Equals(IFormula? other)
    {
        var result = other is GetBenchmarkFormula formula &&
            EqualityComparer<bool>.Default.Equals(IsT, formula.IsT) &&
            EqualityComparer<string>.Default.Equals(OwnerName, formula.OwnerName) &&
            EqualityComparer<string>.Default.Equals(InvestmentName, formula.InvestmentName) &&
            EqualityComparer<string>.Default.Equals(BenchmarkName, formula.BenchmarkName) &&
            EqualityComparer<string>.Default.Equals(BenchmarkVintageYear, formula.BenchmarkVintageYear) &&
            EqualityComparer<string>.Default.Equals(BenchmarkDataItemName, formula.BenchmarkDataItemName) &&
            EqualityComparer<string>.Default.Equals(VintageYearType, formula.VintageYearType) &&
            EqualityComparer<string>.Default.Equals(DataMode, formula.DataMode) &&
            EqualityComparer<string>.Default.Equals(EndDate, formula.EndDate) &&
            EqualityComparer<string>.Default.Equals(TimeHorizon, formula.TimeHorizon) &&
            EqualityComparer<string>.Default.Equals(Currency, formula.Currency);

        return result;
    }

    public override int GetHashCode()
    {
        var hash = new HashCode()
            .AppendHash(IsT)
            .AppendHash(OwnerName)
            .AppendHash(InvestmentName)
            .AppendHash(BenchmarkName)
            .AppendHash(BenchmarkVintageYear)
            .AppendHash(BenchmarkDataItemName)
            .AppendHash(VintageYearType)
            .AppendHash(DataMode)
            .AppendHash(EndDate)
            .AppendHash(TimeHorizon)
            .AppendHash(Currency);

        return hash.ToHashCode();
    }

    public T Accept<T>(IFormulaVisitor<T> visitor) => visitor.Visit(this);
}

public sealed record GetCashFormula : IFormula
{
    public GetCashFormula(
        bool isT,
        CellLocation formulaCell,
        string transactionId,
        IPathExpressions pathExpressions,
        string transactionType,
        Either<string, DateTime>? transactionDate,
        string scenario,
        Either<string, DateTime>? asOfDate,
        string propertyName,
        string scale)
    {
        IsT = isT;
        FormulaCell = formulaCell ?? throw new ArgumentNullException(nameof(formulaCell));
        TransactionId = transactionId;
        PathExpressions = pathExpressions ?? throw new ArgumentNullException(nameof(pathExpressions));
        TransactionType = transactionType;
        TransactionDate = transactionDate;
        Scenario = scenario;
        AsOfDate = asOfDate;
        PropertyName = propertyName;
        Scale = scale;
    }

    public bool IsT { get; init; }
    public CellLocation FormulaCell { get; }
    // Is not set anymore, but is required for interface implementation
    public Option<ErrorFormulaResult> Error { get; }

    public string TransactionId { get; init; }
    public IPathExpressions PathExpressions { get; init; }
    public string TransactionType { get; init; }
    public Either<string, DateTime>? TransactionDate { get; init; }
    public string Scenario { get; init; }
    public Either<string, DateTime>? AsOfDate { get; init; }
    public string PropertyName { get; init; }
    public string Scale { get; init; }

    public bool Equals(IFormula? other)
    {
        var result = other is GetCashFormula formula &&
            EqualityComparer<bool>.Default.Equals(IsT, formula.IsT) &&
            EqualityComparer<string>.Default.Equals(TransactionId, formula.TransactionId) &&
            EqualityComparer<IPathExpressions>.Default.Equals(PathExpressions, formula.PathExpressions) &&
            EqualityComparer<string>.Default.Equals(TransactionType, formula.TransactionType) &&
            EqualityComparer<Either<string, DateTime>?>.Default.Equals(TransactionDate, formula.TransactionDate) &&
            EqualityComparer<string>.Default.Equals(Scenario, formula.Scenario) &&
            EqualityComparer<Either<string, DateTime>?>.Default.Equals(AsOfDate, formula.AsOfDate) &&
            EqualityComparer<string>.Default.Equals(PropertyName, formula.PropertyName) &&
            EqualityComparer<string>.Default.Equals(Scale, formula.Scale);

        return result;
    }

    public override int GetHashCode()
    {
        var hash = new HashCode()
            .AppendHash(IsT)
            .AppendHash(TransactionId)
            .AppendHash(PathExpressions)
            .AppendHash(TransactionType)
            .AppendHash(TransactionDate)
            .AppendHash(Scenario)
            .AppendHash(AsOfDate)
            .AppendHash(PropertyName)
            .AppendHash(Scale);

        return hash.ToHashCode();
    }

    public T Accept<T>(IFormulaVisitor<T> visitor) => visitor.Visit(this);
}

public sealed class GetPerformanceFormula : IFormula
{
    public enum AmountType
    {
        Base = 0,
        Local = 1
    }

    public GetPerformanceFormula(
        CellLocation formulaCell,
        IPathExpressions pathExpressions,
        string dataItem,
        Either<string, DateTime>? startDate,
        string startDateOffset,
        Either<string, DateTime>? endDate,
        string endDateOffset,
        string currency,
        (Either<string, DateTime>? Date, AmountType AmountType) fxType,
        string scenario,
        Either<string, DateTime>? asOfDate)
    {
        FormulaCell = formulaCell ?? throw new ArgumentNullException(nameof(formulaCell));
        PathExpressions = pathExpressions ?? throw new ArgumentNullException(nameof(pathExpressions));
        DataItem = dataItem;
        StartDate = startDate;
        StartDateOffset = startDateOffset;
        EndDate = endDate;
        EndDateOffset = endDateOffset;
        Currency = currency;
        FxTypeDate = fxType.Date;
        FxTypeAmountType = fxType.AmountType;
        Scenario = scenario;
        AsOfDate = asOfDate;

        Error = ErrorFormulaResult.TryCreate(EnumerateErrors());
    }

    public bool IsT => false;
    public CellLocation FormulaCell { get; }
    public Option<ErrorFormulaResult> Error { get; }

    public IPathExpressions PathExpressions { get; }
    public string DataItem { get; }
    public Either<string, DateTime>? StartDate { get; }
    public string StartDateOffset { get; }
    public Either<string, DateTime>? EndDate { get; }
    public string EndDateOffset { get; }
    public string Currency { get; }
    public Either<string, DateTime>? FxTypeDate { get; }
    public AmountType FxTypeAmountType { get; }
    public string Scenario { get; }
    public Either<string, DateTime>? AsOfDate { get; }

    private IEnumerable<string> EnumerateErrors()
    {
        if (DataItem.ParameterIsNotSpecified())
            yield return Errors.DataItemNotExist;

        if (Currency.ParameterIsNotSpecified())
            yield return Errors.CurrencyNotValid;
    }

    public override bool Equals(object? obj) => Equals(obj as GetPerformanceFormula);

    public bool Equals(IFormula? other)
    {
        var result = other is GetPerformanceFormula formula &&
            EqualityComparer<bool>.Default.Equals(IsT, formula.IsT) &&
            EqualityComparer<IPathExpressions>.Default.Equals(PathExpressions, formula.PathExpressions) &&
            EqualityComparer<string>.Default.Equals(DataItem, formula.DataItem) &&
            EqualityComparer<Either<string, DateTime>?>.Default.Equals(StartDate, formula.StartDate) &&
            EqualityComparer<Either<string, DateTime>?>.Default.Equals(EndDate, formula.EndDate) &&
            EqualityComparer<string>.Default.Equals(StartDateOffset, formula.StartDateOffset) &&
            EqualityComparer<string>.Default.Equals(EndDateOffset, formula.EndDateOffset) &&
            EqualityComparer<string>.Default.Equals(Currency, formula.Currency) &&
            EqualityComparer<Either<string, DateTime>?>.Default.Equals(FxTypeDate, formula.FxTypeDate) &&
            EqualityComparer<AmountType>.Default.Equals(FxTypeAmountType, formula.FxTypeAmountType) &&
            EqualityComparer<string>.Default.Equals(Scenario, formula.Scenario) &&
            EqualityComparer<Either<string, DateTime>?>.Default.Equals(AsOfDate, formula.AsOfDate);

        return result;
    }

    public override int GetHashCode()
    {
        var hash = new HashCode()
            .AppendHash(IsT)
            .AppendHash(PathExpressions)
            .AppendHash(DataItem)
            .AppendHash(StartDate)
            .AppendHash(EndDate)
            .AppendHash(StartDateOffset)
            .AppendHash(EndDateOffset)
            .AppendHash(Currency)
            .AppendHash(FxTypeDate)
            .AppendHash(FxTypeAmountType)
            .AppendHash(Scenario)
            .AppendHash(AsOfDate);

        return hash.ToHashCode();
    }

    public T Accept<T>(IFormulaVisitor<T> visitor) => visitor.Visit(this);
}

public sealed record PutCashFormula : IFormula
{
    public PutCashFormula(
        bool isT,
        CellLocation formulaCell,
        string transactionId,
        IPathExpressions pathExpressions,
        string transactionType,
        DateTime? transactionDate,
        string amount,
        string currency,
        string scenario,
        DateTime? asOfDate,
        string localAmount,
        string localCurrency,
        string description,
        string custom1,
        string custom2,
        DateTime? custom3,
        DateTime? custom4,
        string custom5,
        string custom6,
        string shares,
        string valuePerShare,
        string costPerShare,
        int? documentId,
        int? pageNumber,
        double? left,
        double? top,
        double? width,
        double? height)
    {
        IsT = isT;
        FormulaCell = formulaCell ?? throw new ArgumentNullException(nameof(formulaCell));
        TransactionId = transactionId;
        PathExpressions = pathExpressions ?? throw new ArgumentNullException(nameof(pathExpressions));
        TransactionType = transactionType;
        TransactionDate = transactionDate;
        Amount = amount;
        Currency = currency;
        Scenario = scenario;
        AsOfDate = asOfDate;
        LocalAmount = localAmount;
        LocalCurrency = localCurrency;
        Description = description;
        Custom1 = custom1;
        Custom2 = custom2;
        Custom3 = custom3;
        Custom4 = custom4;
        Custom5 = custom5;
        Custom6 = custom6;
        Shares = shares;
        ValuePerShare = valuePerShare;
        CostPerShare = costPerShare;
        DocumentId = documentId;
        PageNumber = pageNumber;
        Left = left;
        Top = top;
        Width = width;
        Height = height;
    }

    public bool IsT { get; init; }
    public CellLocation FormulaCell { get; init; }
    // Is not set anymore, but is required for interface implementation
    public Option<ErrorFormulaResult> Error { get; }

    public string TransactionId { get; init; }
    public IPathExpressions PathExpressions { get; init; }
    public string TransactionType { get; init; }
    public DateTime? TransactionDate { get; init; }
    public string Amount { get; init; }
    public string Currency { get; init; }
    public string Scenario { get; init; }
    public DateTime? AsOfDate { get; init; }
    public string LocalAmount { get; init; }
    public string LocalCurrency { get; init; }
    public string Description { get; init; }
    public string Custom1 { get; init; }
    public string Custom2 { get; init; }
    public DateTime? Custom3 { get; init; }
    public DateTime? Custom4 { get; init; }
    public string Custom5 { get; init; }
    public string Custom6 { get; init; }
    public string Shares { get; init; }
    public string ValuePerShare { get; init; }
    public string CostPerShare { get; init; }
    public int? DocumentId { get; init; }
    public int? PageNumber { get; init; }
    public double? Left { get; init; }
    public double? Top { get; init; }
    public double? Width { get; init; }
    public double? Height { get; init; }

    public bool IsDataLineageSpecified => DocumentId is not null ||
        PageNumber is not null ||
        Left is not null ||
        Top is not null ||
        Width is not null ||
        Height is not null;

    public bool Equals(IFormula? other)
    {
        var result = other is PutCashFormula formula &&
            EqualityComparer<bool>.Default.Equals(IsT, formula.IsT) &&
            EqualityComparer<string>.Default.Equals(TransactionId, formula.TransactionId) &&
            EqualityComparer<IPathExpressions>.Default.Equals(PathExpressions, formula.PathExpressions) &&
            EqualityComparer<string>.Default.Equals(TransactionType, formula.TransactionType) &&
            EqualityComparer<DateTime?>.Default.Equals(TransactionDate, formula.TransactionDate) &&
            EqualityComparer<string>.Default.Equals(Amount, formula.Amount) &&
            EqualityComparer<string>.Default.Equals(Currency, formula.Currency) &&
            EqualityComparer<string>.Default.Equals(Scenario, formula.Scenario) &&
            EqualityComparer<DateTime?>.Default.Equals(AsOfDate, formula.AsOfDate) &&
            EqualityComparer<string>.Default.Equals(LocalAmount, formula.LocalAmount) &&
            EqualityComparer<string>.Default.Equals(LocalCurrency, formula.LocalCurrency) &&
            EqualityComparer<string>.Default.Equals(Description, formula.Description) &&
            EqualityComparer<string>.Default.Equals(Custom1, formula.Custom1) &&
            EqualityComparer<string>.Default.Equals(Custom2, formula.Custom2) &&
            EqualityComparer<DateTime?>.Default.Equals(Custom3, formula.Custom3) &&
            EqualityComparer<DateTime?>.Default.Equals(Custom4, formula.Custom4) &&
            EqualityComparer<string>.Default.Equals(Custom5, formula.Custom5) &&
            EqualityComparer<string>.Default.Equals(Custom6, formula.Custom6) &&
            EqualityComparer<string>.Default.Equals(Shares, formula.Shares) &&
            EqualityComparer<string>.Default.Equals(ValuePerShare, formula.ValuePerShare) &&
            EqualityComparer<string>.Default.Equals(CostPerShare, formula.CostPerShare) &&
            EqualityComparer<int?>.Default.Equals(DocumentId, formula.DocumentId) &&
            EqualityComparer<int?>.Default.Equals(PageNumber, formula.PageNumber) &&
            EqualityComparer<double?>.Default.Equals(Left, formula.Left) &&
            EqualityComparer<double?>.Default.Equals(Top, formula.Top) &&
            EqualityComparer<double?>.Default.Equals(Width, formula.Width) &&
            EqualityComparer<double?>.Default.Equals(Height, formula.Height);

        return result;
    }

    public override int GetHashCode()
    {
        var hash = new HashCode()
            .AppendHash(IsT)
            .AppendHash(TransactionId)
            .AppendHash(PathExpressions)
            .AppendHash(TransactionType)
            .AppendHash(TransactionDate)
            .AppendHash(Amount)
            .AppendHash(Currency)
            .AppendHash(Scenario)
            .AppendHash(AsOfDate)
            .AppendHash(LocalAmount)
            .AppendHash(LocalCurrency)
            .AppendHash(Description)
            .AppendHash(Custom1)
            .AppendHash(Custom2)
            .AppendHash(Custom3)
            .AppendHash(Custom4)
            .AppendHash(Custom5)
            .AppendHash(Custom6)
            .AppendHash(Shares)
            .AppendHash(ValuePerShare)
            .AppendHash(CostPerShare)
            .AppendHash(DocumentId)
            .AppendHash(PageNumber)
            .AppendHash(Left)
            .AppendHash(Top)
            .AppendHash(Width)
            .AppendHash(Height);

        return hash.ToHashCode();
    }

    public T Accept<T>(IFormulaVisitor<T> visitor) => visitor.Visit(this);
}

public sealed record PutFormula : IFormula
{
    public bool IsT { get; init; }
    public CellLocation FormulaCell { get; init; }
    public string DataItemValue { get; init; }
    public string Identifier { get; init; }
    public string DataItemId { get; init; }
    public Either<string, DateTime>? PeriodEnd { get; init; }
    public string PeriodLength { get; init; }
    public string ScenarioName { get; init; }
    public string ScaleFactor { get; init; }
    public string OwnerName { get; init; }
    public string SecurityName { get; init; }
    public string CurrencyCode { get; init; }
    public Either<string, DateTime>? AsOf { get; init; }
    public string IsRequired { get; init; }
    public int? DocumentId { get; init; }
    public int? PageNumber { get; init; }
    public double? Left { get; init; }
    public double? Top { get; init; }
    public double? Width { get; init; }
    public double? Height { get; init; }

    public Option<ErrorFormulaResult> Error { get; }

    public PutFormula(
        bool isT,
        CellLocation formulaCell,
        string dataItemValue,
        string identifier,
        string dataItemId,
        Either<string, DateTime>? periodEnd,
        string periodLength,
        string scenarioName,
        string scaleFactor,
        string ownerName,
        string securityName,
        string currencyCode,
        Either<string, DateTime>? asOf,
        string isRequired,
        int? documentId,
        int? pageNumber,
        double? left,
        double? top,
        double? width,
        double? height)
    {
        IsT = isT;
        FormulaCell = formulaCell ?? throw new ArgumentNullException(nameof(formulaCell));
        DataItemValue = dataItemValue;
        Identifier = identifier;
        DataItemId = dataItemId;
        PeriodEnd = periodEnd;
        PeriodLength = periodLength;
        ScenarioName = scenarioName;
        ScaleFactor = scaleFactor;
        OwnerName = ownerName;
        SecurityName = securityName;
        CurrencyCode = currencyCode;
        AsOf = asOf;
        IsRequired = isRequired;
        DocumentId = documentId;
        PageNumber = pageNumber;
        Left = left;
        Top = top;
        Width = width;
        Height = height;

        Error = ErrorFormulaResult.TryCreate(EnumerateErrors());
    }

    public string? Comment { get; private set; }

    public bool IsDataLineageSpecified => DocumentId is not null ||
        PageNumber is not null ||
        Left is not null ||
        Top is not null ||
        Width is not null ||
        Height is not null;

    public void SetComment(string comment) => Comment = comment;

    private IEnumerable<string> EnumerateErrors()
    {
        if (IsT)
            yield break;

        // Either asset or owner?
        if (Identifier.ParameterIsNotSpecified())
            yield return Errors.OwnerNotValid;

        if (ScenarioName.ParameterIsNotSpecified())
            yield return Errors.ScenarioNotValid;

        if (DataItemId.ParameterIsNotSpecified())
            yield return Errors.DataItemNotValid;
    }

    public bool Equals(IFormula? other)
    {
        var result = other is PutFormula formula &&
            EqualityComparer<bool>.Default.Equals(IsT, formula.IsT) &&
            EqualityComparer<string>.Default.Equals(DataItemValue, formula.DataItemValue) &&
            EqualityComparer<string>.Default.Equals(Identifier, formula.Identifier) &&
            EqualityComparer<string>.Default.Equals(DataItemId, formula.DataItemId) &&
            EqualityComparer<string>.Default.Equals(ScenarioName, formula.ScenarioName) &&
            EqualityComparer<string>.Default.Equals(PeriodLength, formula.PeriodLength) &&
            EqualityComparer<Either<string, DateTime>?>.Default.Equals(PeriodEnd, formula.PeriodEnd) &&
            EqualityComparer<string>.Default.Equals(OwnerName, formula.OwnerName) &&
            EqualityComparer<string>.Default.Equals(SecurityName, formula.SecurityName) &&
            EqualityComparer<string>.Default.Equals(CurrencyCode, formula.CurrencyCode) &&
            EqualityComparer<string>.Default.Equals(ScaleFactor, formula.ScaleFactor) &&
            EqualityComparer<Either<string, DateTime>?>.Default.Equals(AsOf, formula.AsOf) &&
            EqualityComparer<string>.Default.Equals(IsRequired, formula.IsRequired) &&
            EqualityComparer<int?>.Default.Equals(DocumentId, formula.DocumentId) &&
            EqualityComparer<int?>.Default.Equals(PageNumber, formula.PageNumber) &&
            EqualityComparer<double?>.Default.Equals(Left, formula.Left) &&
            EqualityComparer<double?>.Default.Equals(Top, formula.Top) &&
            EqualityComparer<double?>.Default.Equals(Width, formula.Width) &&
            EqualityComparer<double?>.Default.Equals(Height, formula.Height);

        return result;
    }

    public override int GetHashCode()
    {
        var hash = new HashCode()
            .AppendHash(IsT)
            .AppendHash(DataItemValue)
            .AppendHash(Identifier)
            .AppendHash(DataItemId)
            .AppendHash(ScenarioName)
            .AppendHash(PeriodLength)
            .AppendHash(PeriodEnd)
            .AppendHash(OwnerName)
            .AppendHash(SecurityName)
            .AppendHash(CurrencyCode)
            .AppendHash(ScaleFactor)
            .AppendHash(AsOf)
            .AppendHash(IsRequired)
            .AppendHash(DocumentId)
            .AppendHash(PageNumber)
            .AppendHash(Left)
            .AppendHash(Top)
            .AppendHash(Width)
            .AppendHash(Height);

        return hash.ToHashCode();
    }

    public T Accept<T>(IFormulaVisitor<T> visitor) => visitor.Visit(this);
}

public sealed record ArrayFormula : IFormula
{
    public enum IGetArrayOutputDirection
    {
        TopToBottom = 0,
        LeftToRight = 1
    }

    public sealed class ArrayOutputLocation
    {
        private readonly RangeLocation? _outputRange;

        private ArrayOutputLocation(CellLocation startOutput)
        {
            StartOutput = startOutput ?? throw new ArgumentNullException(nameof(startOutput));
        }

        private ArrayOutputLocation(RangeLocation outputRange)
        {
            _outputRange = outputRange ?? throw new ArgumentNullException(nameof(outputRange));
            StartOutput = outputRange.TopLeft;
        }

        public CellLocation StartOutput { get; }

        public string SheetName => StartOutput.SheetName;

        public bool TryGetOutputRange(out RangeLocation? rangeLocation)
            => (rangeLocation = _outputRange) != null;

        public override bool Equals(object? obj) => obj switch
        {
            ArrayOutputLocation location => StartOutput.Equals(location.StartOutput),
            CellLocation location => StartOutput.Equals(location),
            _ => false
        };

        public override int GetHashCode()
        {
            int hashCode = -1426384533;
            hashCode = hashCode * -1521134295 + StartOutput.GetHashCode();
            return hashCode;
        }

        public static implicit operator ArrayOutputLocation(CellLocation cellLocation) => new(cellLocation);
        public static implicit operator ArrayOutputLocation(RangeLocation rangeLocation) => new(rangeLocation);

        public static Option<ArrayOutputLocation> GetOptional(CellLocation cellLocation)
            => cellLocation is null ? None : new ArrayOutputLocation(cellLocation);
        public static Option<ArrayOutputLocation> GetOptional(RangeLocation rangeLocation)
            => rangeLocation is null ? None : new ArrayOutputLocation(rangeLocation);
    }

    public ArrayFormula(
        bool isT,
        CellLocation formulaCell,
        Option<ArrayOutputLocation> outputLocation,
        IGetArrayOutputDirection outputDirection,
        IArrayFormulaArgs args)
    {
        IsT = isT;
        FormulaCell = formulaCell ?? throw new ArgumentNullException(nameof(formulaCell));
        OutputLocation = outputLocation;
        OutputDirection = outputDirection;
        Args = args ?? throw new ArgumentNullException(nameof(args));

        Error = ErrorFormulaResult.TryCreate(Args.EnumerateErrors());
    }

    public bool IsT { get; }

    public CellLocation FormulaCell { get; }

    public Option<ErrorFormulaResult> Error { get; }

    public Option<ArrayOutputLocation> OutputLocation { get; }

    public IGetArrayOutputDirection OutputDirection { get; }

    public IArrayFormulaArgs Args { get; }

    public Option<CellLocation> GetStartOutput() => OutputLocation.Map(x => x.StartOutput);

    public bool IsTransposed() => OutputDirection is IGetArrayOutputDirection.LeftToRight;

    public bool Equals(IFormula? other)
    {
        var result = other is ArrayFormula formula &&
            EqualityComparer<bool>.Default.Equals(IsT, formula.IsT) &&
            EqualityComparer<Option<ArrayOutputLocation>>.Default.Equals(OutputLocation, formula.OutputLocation) &&
            EqualityComparer<IGetArrayOutputDirection>.Default.Equals(OutputDirection, formula.OutputDirection) &&
            Args.Equals(formula.Args);

        return result;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(IsT, OutputLocation, OutputDirection, Args);
    }

    public T Accept<T>(IFormulaVisitor<T> visitor) => visitor.Visit(this);

    #region ChangeTracker

    private static IFormulaResult _state = NotSyncFormulaResult.Instance;

    public static IFormulaResult GetState() => _state;

    public static IDisposable ChangeTrack() => new ChangeTracker();

    private sealed class ChangeTracker : IDisposable
    {
        private static readonly ErrorFormulaResult _error = new("References to one or more parameters are within the Array Output Range. Please update the references or Array Start Location");

        public ChangeTracker() => Interlocked.Exchange(ref _state, _error);

        public void Dispose() => Interlocked.Exchange(ref _state, NotSyncFormulaResult.Instance);
    }

    #endregion
}