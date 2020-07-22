using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace course_app.Dtos
{
    public class PagedListDto<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public List<T> Items { get; set; }
    }
}
