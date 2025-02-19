namespace DesignPatterns.Visitor;

public sealed class DateValidator
{
    public async Task<bool> ValidateDateAsync(DateTime date, CancellationToken ct)
    {
        await Task.Delay(100);
        return true;
    }
}