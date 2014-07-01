using System;

namespace QueryPattern.Models
{
    public class MoviesViewModel
    {
        public Movie[] Movies { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalMovieCount { get; set; }

        public int TotalPages
        {
            get
            {
                return (int) Math.Ceiling(TotalMovieCount/(double) PageSize);
            }
        }
    }
}