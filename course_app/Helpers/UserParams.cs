using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace course_app.Helpers
{
    public class UserParams
    {
        private const int MaxPageSize = 50;
        private const int DefaultPage = 0;

        private int pageSize = 10;

        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }

        private int pageNumber;

        public int PageNumber
        {
            get { return pageNumber; }
            set { pageNumber = (value < DefaultPage) ? DefaultPage : value; }
        }

        public int? UserId { get; set; }
        public string? Gender { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 99;
        public string OrderBy { get; set; };

    }
}
