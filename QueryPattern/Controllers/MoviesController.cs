using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.Mvc;
using QueryPattern.Infrastructure;
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
            Query.Page = 1;
            return IndexView();
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

            return IndexView();
        }


        public ActionResult Page(int page)
        {
            Query.Page = page;
            return IndexView();
        }

        private ActionResult IndexView()
        {
            var model = ExecuteMoviesQuery(Query);
            return View("Index", model);
        }

        private static MoviesViewModel ExecuteMoviesQuery(MoviesQuery moviesQuery)
        {
            var query = FakeDatabase.Movies.AsQueryable();

            // filters
            if (!string.IsNullOrWhiteSpace(moviesQuery.Title))
                query = query.Where(x => x.Title.Equals(moviesQuery.Title, StringComparison.OrdinalIgnoreCase));

            if (moviesQuery.Year != null)
                query = query.Where(x => x.Year == moviesQuery.Year);

            // sorting
            var sort = moviesQuery.Sort ?? "";
            switch (sort.ToLowerInvariant())
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

            var totalMovieCount = query.Count();

            // paging
            query = query.Page(moviesQuery.Page, moviesQuery.PageSize);
            var movies = query.ToArray();

            return new MoviesViewModel
            {
                Movies = movies,
                Page = moviesQuery.Page,
                PageSize=moviesQuery.PageSize,
                TotalMovieCount = totalMovieCount
            };

        }

    }
}