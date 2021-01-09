using System;
namespace MovieRanking.Data
{
    public class MoviePerson
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
