using System;
using System.Linq;
using System.Web.Mvc;
using QueryPattern.Models;

namespace QueryPattern.Controllers
{
    public class MoviesController : Controller
    {
        private const string QuerySessionKey = "MoviesController.Query";

        public MoviesQuery Query
        {
            get
            {
                var moviesQuery = Session[QuerySessionKey] as MoviesQuery;
                return moviesQuery ?? (Query = new MoviesQuery());
            }
            set { Session[QuerySessionKey] = value; }
        }

        public ActionResult Index()
        {
            var movies = ExecuteMoviesQuery(Query);
            return View(movies);
        }

        public ActionResult Sort(string property)
        {
            if (Query.Sort == property)
            {
                // toggle direction
                Query.SortDescending = !Query.SortDescending;
            }
            else
            {
                // change sort
                Query.Sort = property;
                Query.SortDescending = false;
            }


            var movies = ExecuteMoviesQuery(Query);
            return View("Index", movies);
        }

        private static Movie[] ExecuteMoviesQuery(MoviesQuery moviesQuery)
        {
            var query = FakeDatabase.Movies.AsQueryable();

            // filters
            if (!string.IsNullOrWhiteSpace(moviesQuery.Title))
                query = query.Where(x => x.Title.Equals(moviesQuery.Title, StringComparison.OrdinalIgnoreCase));

            if (moviesQuery.Year != null)
                query = query.Where(x => x.Year == moviesQuery.Year);

            // sorting
            moviesQuery.Sort = moviesQuery.Sort ?? "";
            switch (moviesQuery.Sort.ToLowerInvariant())
            {
                case "title":
                    query = moviesQuery.SortDescending
                        ? query.OrderByDescending(x => x.Title)
                        : query.OrderBy(x => x.Title);
                    break;

                case "year":
                    query = moviesQuery.SortDescending
                        ? query.OrderByDescending(x => x.Year)
                        : query.OrderBy(x => x.Year);
                    break;

                default:
                    query = moviesQuery.SortDescending
                        ? query.OrderByDescending(x => x.Id)
                        : query.OrderBy(x => x.Id);
                    break;
            }

            // paging
            if (moviesQuery.Skip.HasValue)
                query = query.Skip(moviesQuery.Skip.Value);

            if (moviesQuery.Take.HasValue)
                query = query.Take(moviesQuery.Take.Value);

            return query.ToArray();
        }
    }

    public class MoviesQuery
    {
        // filters
        public string Title { get; set; }
        public int? Year { get; set; }

        // sorting
        public string Sort { get; set; }
        public bool SortDescending { get; set; }

        // paging
        public int? Skip { get; set; }
        public int? Take { get; set; }

        public MoviesQuery()
        {
            Sort = "Id";
        }

    }
}