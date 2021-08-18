namespace DeemZ.Global.Extensions
{
    using System.Collections.Generic;
    using System.Linq;

    public static class LinqExtentions
    {
        public static IEnumerable<T> Paging<T>(this IEnumerable<T> query, int page = 1, int quantity = 10)
            => query.Skip((page - 1) * quantity)
                .Take(quantity);
    }
}