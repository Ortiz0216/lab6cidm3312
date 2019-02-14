using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace lab6
{
    public class Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source = database.db");
        }
        public DbSet<Studio> Studios {get; set;}
        public DbSet<Movie> Movies {get; set;}
    }

    public class Studio
    {
        public int StudioId {get; set;}
        public string Name {get; set;}
        public List<Movie> Movies {get; set;} // Navigation property. Each Blog can have MANY posts.
                                            // The many is represented by a List of Posts
        public override string ToString()
        {
            return $"Studio {StudioId} - {Name}";
        }
    }

    public class Movie
    {
        public int MovieId {get; set;}
        public string Title {get; set;}
        public string Genre {get; set;}
        public int StudioId {get; set;} // Foreign key
        public Studio Studio {get; set;} // Navigation property. Each post is associated with ONE Blog
        public override string ToString()
        {
            return $"Post {MovieId} - {Title}";
        }
    }
}
