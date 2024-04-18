namespace ISA.Core.Infrastructure.Persistence.PostgreSQL.QueryExtensionMethods;

internal static class GetPagedQuery
{
    public static IQueryable<T> GetPaged<T>(this IQueryable<T> query, int page)
    {
        if (page < 1)
            page = 1;

        var pageSize = 10; // Default page size

        var itemsToSkip = (page - 1) * pageSize;
        return query.Skip(itemsToSkip).Take(pageSize);
    }
}
