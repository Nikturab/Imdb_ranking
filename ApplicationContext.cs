using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using MovieRanking.Data;

namespace MovieRanking
{


        public class ApplicationContext: DbContext
    {
        private const string dataFolder = "/Volumes/MyPassport/progr/ml-latest";

        public DbSet<MovieMovie> MovieMovie { get; set; } // similar movies
        public DbSet<MovieTag> MovieTag { get; set; }
        public DbSet<MoviePerson> MoviePerson { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Person> Persons { get; set; }


        public ApplicationContext()
        {
            Database.EnsureCreated();

            
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source=\"{dataFolder}/im.db\"");
            
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Movie>()
                .HasOne(pt => pt.Person).WithMany()
                .HasForeignKey(pt => pt.PersonId);




            modelBuilder.Entity<MovieTag>()
                .HasKey(t => new { t.MovieId, t.TagId });

            modelBuilder.Entity<MovieTag>()
                .HasOne(pt => pt.Movie)
                .WithMany(p => p.Tags)
                .HasForeignKey(pt => pt.MovieId);

            modelBuilder.Entity<MovieTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.MovieTags)
                .HasForeignKey(pt => pt.TagId);



            modelBuilder.Entity<MoviePerson>()
               .HasKey(t => new { t.MovieId, t.PersonId });

            modelBuilder.Entity<MoviePerson>()
                .HasOne(pt => pt.Movie)
                .WithMany(p => p.Actors)
                .HasForeignKey(pt => pt.MovieId);

            modelBuilder.Entity<MoviePerson>()
                .HasOne(pt => pt.Person)
                .WithMany(t => t.MoviePersons)
                .HasForeignKey(pt => pt.PersonId);




            modelBuilder.Entity<MovieMovie>()
               .HasKey(t => new { t.BaseMovieId, t.MovieId });

            modelBuilder.Entity<MovieMovie>()
                .HasOne(pt => pt.BaseMovie)
                .WithMany(p => p.SimilarMovies)
                .HasForeignKey(pt => pt.BaseMovieId);

        }
    }
}
