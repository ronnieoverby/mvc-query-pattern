using System.Web.Configuration;

namespace QueryPattern.Models
{
    public class MoviesQuery
    {
        // filters
        public string Title { get; set; }
        public int? Year { get; set; }

        // sorting
        public string Sort { get; set; }
        public bool SortDescending { get; set; }

        // paging
        public int Page { get; set; }
        public int PageSize { get; set; }

        public MoviesQuery()
        {
            Sort = "Id";
            Page = 1;
            PageSize = 2;
            
        }

    }
}