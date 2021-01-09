using System;
using System.Collections.Generic;

namespace MovieRanking.Data
{
    public class Tag
    { 

        public string TagId { get; set; }

        //public int IMDB_ID { get; set; }
        public string Title { get; set; }


        public ICollection<MovieTag> MovieTags { get; set; }

    }
}
