using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MovieRanking.Data
{
    public class Movie
    {
        public int Id { get; set; }
        public int IMDB_ID { get; set; }
        public string Title { get; set; }
        public string Lang { get; set; }

        public Person Person { get; set; }
        public int PersonId { get; set; }

        [NotMapped]
        public Person Director { get { return Person; } set { Person = value; } }

        public byte Rate { get; set; }

        public List<MoviePerson> Actors { get; set; }
        [NotMapped]
        public List<Person> ActorsProfiles { get { return Actors==null ? new List<Person>() : Actors.Select( mp => mp.Person).Distinct().ToList(); } }
        public List<int> ActorsIds { get { return Actors==null ? new List<int>() : Actors.Select( mp => mp.PersonId).Distinct().ToList(); } }

        public List<MovieMovie> SimilarMovies { get; set; }
        
        [NotMapped]
        public List<Movie> _SimilarMovies { get; set; }
        public ICollection<MovieTag> Tags { get; set; }

        override public string ToString() => $"{{id: {IMDB_ID}, " +
                $"title: {Title}, " +
                $"language: {Lang}, " +
                $"director: {Director.ToString()}, " +
                $"rate: {Rate / 10.0}," +
                $"actors: { (Actors == null ? 0 : Actors.Count()) }," +
                $"tags: {(Tags == null ? "" : string.Join(",", Tags))} }}";

        public static byte parseRate(string decimalString)
        {
            return byte.Parse(decimalString.Replace(".", ""));
        }


        public double Similarity(Movie movie)
        {
            int count = ActorsProfiles.Intersect(movie.ActorsProfiles).Count();
            count += Tags.Select(t=>t.TagId).Intersect(movie.Tags.Select(t => t.TagId)).Count();
            if (Director.Equals(movie.Director)) count += 1;

            int max_intersect = Math.Max(Tags.Count(), movie.Tags.Count()) + Math.Max(ActorsProfiles.Count(), movie.ActorsProfiles.Count()) + 1;
            var similarity = count == 0 ? 0 : ((double) count)/(max_intersect);

            return (similarity + Rate/100.0)*0.5;
        }


        public static int parseId(string idString)
        {
            return int.Parse(idString.Substring(2));
        }
    }

}


