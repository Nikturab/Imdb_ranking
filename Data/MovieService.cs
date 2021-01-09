using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace MovieRanking.Data
{
    public class MovieService
    {
        private readonly ApplicationContext _context;
        private const int PageSize = 10;
        public MovieService(ApplicationContext context)
        {
            _context = context;
        }

        public List<Movie> Top(Movie movie)
        {
            var ids = movie.ActorsProfiles.Select(p => p.Id);
            
            
            var candidates_by_dir = _context.Movies // matches by director
                .Where(m => movie.PersonId != 229 && m.PersonId == movie.PersonId)
                .Select(m=>m.Id) // avoid Steven Spielberg
                .Distinct();

            var movie_ids = _context.MoviePerson // matches by actors
                .Where(m => ids.Contains(m.PersonId)).Select(m => m.MovieId).Distinct();

            var candidates = _context.Movies.Include(m => m.Person)
                .Include(m => m.Actors)
                .Include(m => m.Tags)
                .Where(m => movie_ids.Contains(m.Id) || candidates_by_dir.Contains(m.Id)) // if match by director or actor
                .ToList(); 
            
            var top = new Dictionary<Movie, double>();
            foreach (var m in candidates)
            {
                if (m.Id == movie.Id) continue;
                top.TryAdd(m, movie.Similarity(m));
            }

            var top10 = (from entry in top orderby entry.Value descending select entry.Key).Take(10).ToList();

            return top10;
        }
        public Task<Movie> GetOne(int id)
        {
            var m = _context.Movies
                .Include(m => m.Tags)
                .Include(m => m.Person)
                .First(m => m.Id == id);

                //_context.MovieMovie.Where(m => m.BaseMovieId == id).Include(m=>m.Movie).ThenInclude(m=>m.Person).ToList();
            m.Actors = _context.MoviePerson.Where(m => m.MovieId == id).Include(m => m.Person).ToList();
            m._SimilarMovies = Top(m);

            return Task.FromResult(m);
        }

        

        public Task<Movie[]> GetTopByTag(string tagId)
        {
            var movies = _context.MovieTag
                .Where(m => m.TagId == tagId)
                .Include(m => m.Movie)
                .Select(m=>m.Movie)
                .OrderByDescending(m=>m.Rate)
                .Take(20)
                .ToArray();

            return Task.FromResult(movies);
        }

        public Task<Movie[]> GetMoviesAsync(int page=1, string search="")
        {
            return Task.FromResult(_context.Movies
                .Include(m=>m.Person)
                .Where(m=> m.Title.Contains(search))
                .OrderByDescending(m=>m.Rate).Skip((page-1)*PageSize).Take(PageSize).ToArray());
        }
        
        
        public Task<int> GetTotalCount()
        {
            return Task.FromResult(_context.Movies.Count());
        }

        public Task<Tag[]> GetAllTags()
        {
            return Task.FromResult(_context.Tags.ToArray());
        }
    }
}