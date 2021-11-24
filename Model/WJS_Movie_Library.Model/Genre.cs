using System;
using System.Collections.Generic;

namespace WJS_Movie_Library.Model
{
    public class Genre
    {
        public long Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<MovieGenre> MovieGenres {get;set;}
    }
}
