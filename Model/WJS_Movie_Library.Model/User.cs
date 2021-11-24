﻿using System;
using System.Collections.Generic;

namespace WJS_Movie_Library.Model
{
    public class User
    {
        public long Id { get; set; }
        public long Age { get; set; }
        public string Gender { get; set; }
        public string ZipCode { get; set; }

        public virtual Occupation Occupation { get; set; }
        public virtual ICollection<UserMovie> UserMovies {get;set;}
    }
}
