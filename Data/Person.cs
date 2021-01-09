using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieRanking.Data
{
    public class Person
    {
        public static Person Spielberg = new Person { Id = 10252598 + 1, IMDB_ID = 10252598 + 1, Name = "Spielberg" };
        // if there is no director in the database for some film, Steven Spelberg is the default value


        public int Id { get; set; }
        public int IMDB_ID { get; set; }
        public string Name { get; set; }

     
        public List<MoviePerson> MoviePersons { get; set; }  // films the persons played in



        override public string ToString()
        {
            return $"{Name}";
        }

    }

}
