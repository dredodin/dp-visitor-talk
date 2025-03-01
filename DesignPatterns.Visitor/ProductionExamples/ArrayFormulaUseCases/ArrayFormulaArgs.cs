using System.ComponentModel;
using LanguageExt;
using static ProductionExamples.GetPerformanceFormula;

namespace ProductionExamples;

public interface IArrayFormulaArgs : IEquatable<IArrayFormulaArgs>
{
    IEnumerable<string> EnumerateErrors();

    T Accept<T>(IArrayFormulaArgsVisitor<T> visitor);
}

public sealed record CashTransactionsArrayFormulaArgs(
     IReadOnlyCollection<CashTransactionsArrayFormulaArgs.CashFlowColumn> Columns,
     bool ShowHeaders,
     IReadOnlyDictionary<CashTransactionsArrayFormulaArgs.CashFlowColumn, string> Headers,
     IReadOnlyCollection<CashTransactionsArrayFormulaArgs.Order> OrderBy,
     IPathExpressions PathExpressions,
     Either<string, DateTime>? StartDate,
     string StartDateOffset,
     Either<string, DateTime>? EndDate,
     string EndDateOffset,
     string Currency,
     Either<string, DateTime>? FxTypeDate,
     AmountType FxTypeAmountType,
     IReadOnlyCollection<string> Scenarios,
     Either<string, DateTime>? AsOfDate,
     string AsOfDateType,
     IReadOnlyCollection<string> TransactionTypes,
     IReadOnlyCollection<string> Custom1,
     IReadOnlyCollection<string> Custom2,
     DateTime? Custom3,
     DateTime? Custom4,
     IReadOnlyCollection<string> Custom5,
     IReadOnlyCollection<string> Custom6) : IArrayFormulaArgs
{
    public IEnumerable<string> EnumerateErrors()
    {
        yield break;
    }

    public bool Equals(IArrayFormulaArgs? other)
    {
        var result = other is CashTransactionsArrayFormulaArgs args &&
            Columns.SequenceEqual(args.Columns) &&
            EqualityComparer<bool>.Default.Equals(ShowHeaders, args.ShowHeaders) &&
            Headers.SequenceEqual(args.Headers) &&
            OrderBy.SequenceEqual(args.OrderBy) &&
            EqualityComparer<IPathExpressions>.Default.Equals(PathExpressions, args.PathExpressions) &&
            EqualityComparer<Either<string, DateTime>?>.Default.Equals(StartDate, args.StartDate) &&
            EqualityComparer<string>.Default.Equals(StartDateOffset, args.StartDateOffset) &&
            EqualityComparer<Either<string, DateTime>?>.Default.Equals(EndDate, args.EndDate) &&
            EqualityComparer<string>.Default.Equals(EndDateOffset, args.EndDateOffset) &&
            EqualityComparer<string>.Default.Equals(Currency, args.Currency) &&
            EqualityComparer<Either<string, DateTime>?>.Default.Equals(FxTypeDate, args.FxTypeDate) &&
            EqualityComparer<AmountType>.Default.Equals(FxTypeAmountType, args.FxTypeAmountType) &&
            Scenarios.SequenceEqual(args.Scenarios) &&
            EqualityComparer<Either<string, DateTime>?>.Default.Equals(AsOfDate, args.AsOfDate) &&
            EqualityComparer<string>.Default.Equals(AsOfDateType, args.AsOfDateType) &&
            TransactionTypes.SequenceEqual(args.TransactionTypes) &&
            Custom1.SequenceEqual(args.Custom1) &&
            Custom2.SequenceEqual(args.Custom2) &&
            EqualityComparer<DateTime?>.Default.Equals(Custom3, args.Custom3) &&
            EqualityComparer<DateTime?>.Default.Equals(Custom4, args.Custom4) &&
            Custom5.SequenceEqual(args.Custom5) &&
            Custom6.SequenceEqual(args.Custom6);

        return result;
    }

    public override int GetHashCode()
    {
        var hash = new HashCode()
            .AppendHashEnumerable(Columns)
            .AppendHash(ShowHeaders)
            .AppendHashEnumerable(Headers)
            .AppendHashEnumerable(OrderBy)
            .AppendHash(PathExpressions)
            .AppendHash(StartDate)
            .AppendHash(StartDateOffset)
            .AppendHash(EndDate)
            .AppendHash(EndDateOffset)
            .AppendHash(Currency)
            .AppendHash(FxTypeDate)
            .AppendHash(FxTypeAmountType)
            .AppendHashEnumerable(Scenarios)
            .AppendHash(AsOfDate)
            .AppendHash(AsOfDateType)
            .AppendHashEnumerable(TransactionTypes)
            .AppendHashEnumerable(Custom1)
            .AppendHashEnumerable(Custom2)
            .AppendHash(Custom3)
            .AppendHash(Custom4)
            .AppendHashEnumerable(Custom5)
            .AppendHashEnumerable(Custom6);

        return hash.ToHashCode();
    }

    public T Accept<T>(IArrayFormulaArgsVisitor<T> visitor) => visitor.Visit(this);

    public enum CashFlowColumn
    {
        TransactionDate,
        Amount,
        TransactionType,
        Currency,
        Owner,
        Client,
        Investment,
        SecurityName,
        LocalAmount,
        LocalCurrency,
        Description,
        Scenario,
        AsOfDate,
        Shares,
        CostPerShare,
        ValuePerShare,
        Custom1 = 100,
        Custom2,
        Custom3,
        Custom4,
        Custom5,
        Custom6,
        Id,
        TransactionGroupId,
        Category,
        LastModifiedBy,
        LastModifiedDate,
        Investment1,
        Investment2,
        Investment3
    }

    public sealed record Order(CashFlowColumn Column, ListSortDirection Direction);
}

public sealed record ExchangeRatesArrayFormulaArgs(
    string SourceCurrency,
    string TargetCurrency,
    Either<string, DateTime>? StartDate,
    Either<string, DateTime>? EndDate,
    string PriceType
) : IArrayFormulaArgs
{
    public IEnumerable<string> EnumerateErrors()
    {
        if (StartDate is null)
            yield return Errors.StartDateNotValid;

        if (EndDate is null)
            yield return Errors.EndDateNotValid;

        if (SourceCurrency is null)
            yield return Errors.SourceCurrencyNotValid;

        if (TargetCurrency is null)
            yield return Errors.TargetCurrencyNotValid;
    }

    public bool Equals(IArrayFormulaArgs? other)
    {
        var result = other is ExchangeRatesArrayFormulaArgs args &&
            EqualityComparer<string>.Default.Equals(SourceCurrency, args.SourceCurrency) &&
            EqualityComparer<string>.Default.Equals(TargetCurrency, args.TargetCurrency) &&
            EqualityComparer<Either<string, DateTime>?>.Default.Equals(StartDate, args.StartDate) &&
            EqualityComparer<Either<string, DateTime>?>.Default.Equals(EndDate, args.EndDate) &&
            EqualityComparer<string>.Default.Equals(PriceType, args.PriceType);

        return result;
    }

    public override int GetHashCode()
    {
        var hash = new HashCode()
            .AppendHash(SourceCurrency)
            .AppendHash(TargetCurrency)
            .AppendHash(StartDate)
            .AppendHash(EndDate)
            .AppendHash(PriceType);

        return hash.ToHashCode();
    }

    public T Accept<T>(IArrayFormulaArgsVisitor<T> visitor) => visitor.Visit(this);
}