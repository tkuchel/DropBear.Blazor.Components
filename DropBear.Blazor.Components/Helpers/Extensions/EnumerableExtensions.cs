namespace DropBear.Blazor.Components.Helpers.Extensions;

public static class EnumerableExtensions
{
    public static IOrderedEnumerable<TSource> OrderByDynamic<TSource>(
        this IEnumerable<TSource> source,
        Func<Func<TSource, object>> keySelectorProvider,
        bool ascending = true)
    {
        var keySelector = keySelectorProvider();
        return ascending ? source.OrderBy(keySelector) : source.OrderByDescending(keySelector);
    }
}
