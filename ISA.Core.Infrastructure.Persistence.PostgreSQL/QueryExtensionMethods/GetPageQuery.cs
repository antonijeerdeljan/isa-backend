using Microsoft.EntityFrameworkCore;

namespace ISA.Core.Infrastructure.Persistence.PostgreSQL.QueryExtensionMethods;

internal static class GetPageQuery
{
    public static IQueryable<T> GetPage<T>(this IQueryable<T> query, int page, int pageSize)
    {
        if (page < 1)
            page = 1;

        if (pageSize <= 0)
            pageSize = 10; // Default page size

        var itemsToSkip = (page - 1) * pageSize;
        return query.Skip(itemsToSkip).Take(pageSize);
    }
}
