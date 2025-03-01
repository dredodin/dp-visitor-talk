namespace ProductionExamples;

public static class HashCodeExtensions
{
    public static HashCode AppendHashEnumerable<T>(this HashCode that, IEnumerable<T> items)
    {
        return items?.Aggregate(that, (hashCode, item) => hashCode.AppendHash(item)) ?? that.AppendHash(default(T));
    }

    public static HashCode AppendHash<T>(this HashCode that, T item)
    {
        that.Add(item);
        return that;
    }
}
