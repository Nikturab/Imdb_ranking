using System;
namespace MovieRanking.Data
{
    public class MovieMovie
    {
        public int BaseMovieId { get; set; }
        public Movie BaseMovie { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int Similarity;
    }
}
