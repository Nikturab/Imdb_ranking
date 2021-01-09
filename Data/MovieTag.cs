using System;
namespace MovieRanking.Data
{
    public class MovieTag
    {
        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public string TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
