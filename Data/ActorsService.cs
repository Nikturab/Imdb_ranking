using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace MovieRanking.Data
{
    public class ActorsService
    {
        private readonly ApplicationContext _context;
        private const int PageSize = 10;
        public ActorsService(ApplicationContext context)
        {
            _context = context;
        }
        
        public Task<Person> GetPerson(int id)
        {
            var p = _context.Persons.Find(id);
            var movies = _context.MoviePerson
                .Include(m => m.Movie)
                .Where(m => m.PersonId == id);

            p.MoviePersons = movies.ToList();
            return Task.FromResult(p);
        }

        public Task<Movie[]> GetMoviesDirectedBy(int personId)
        {
            return Task.FromResult(_context.Movies.Where(m=>m.PersonId == personId).ToArray());
        }

        public Task<Person[]> GetPersonsAsync(int page=1, string search="")
        {
            return Task.FromResult(_context.Persons
                .Where(m=> m.Name.Contains(search))
                .Skip((page-1)*PageSize).Take(PageSize).ToArray());

        }
        
        public Task<int> GetTotalCount()
        {
            return Task.FromResult(_context.Persons.Count());
        }
    }
}