using System.Collections.Generic;
using System.Threading;

namespace QueryPattern.Models
{
    public static class FakeDatabase
    {
        private static int _seq = 0;

        public static readonly List<Movie> Movies = new List<Movie>
        {
            new Movie
            {
                Id = NextId(),
                Title = "Star Wars",
                Year = 1977
            },
            new Movie
            {
                Id = NextId(),
                Title = "Lord of the Rings",
                Year = 2001
            },
            new Movie
            {
                Id = NextId()
                ,
                Title = "The Fountain",
                Year = 2006
            },
            new Movie
            {
                Id = NextId(),
                Title = "Every Which Way But Loose",
                Year = 1978
            },

        };

        static int NextId()
        {
            return Interlocked.Increment(ref _seq);
        }


    }
}