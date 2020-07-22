using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace course_app.Helpers
{
    public class PagedList<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<T> Items { get; set; }

        public PagedList(List<T> items, int count, int currentPage, int pageSize)
        {
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            PageSize = pageSize;
            TotalCount = count;
            Items = items;
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int page, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.
                Skip(pageSize * page)
                .Take(pageSize)
                .ToListAsync();

            return new PagedList<T>(items, count, page, pageSize);             
        }
    }
}
