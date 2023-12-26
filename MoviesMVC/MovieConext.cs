using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Internal;
using System.Data.Entity.Infrastructure;
using MoviesMVC.Models;

namespace MoviesMVC
{
    public class MovieConext : DbContext
    {

        public MovieConext(): base("MoviesDatabase")
        {
        }

        public  DbSet<Movie> Movies { get; set; }
        public  DbSet<User> Users { get; set; }
    }

}