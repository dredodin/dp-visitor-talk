namespace ProductionExamples;

public interface IArrayFormulaArgsVisitor<out T>
{
    T Visit(CashTransactionsArrayFormulaArgs args);

    T Visit(ExchangeRatesArrayFormulaArgs args);
}

public sealed class ArrayFormulaToNameVisitor : IArrayFormulaArgsVisitor<string>
{
    public static ArrayFormulaToNameVisitor Instance { get; } = new();

    public string Visit(CashTransactionsArrayFormulaArgs args) => "InvestmentCashFlows";

    public string Visit(ExchangeRatesArrayFormulaArgs args) => "ExchangeRates";
}
