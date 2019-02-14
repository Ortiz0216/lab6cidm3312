using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace lab6
{
    class Program
    {
        static void Main(string[] args)
        {
        
            using (var db = new Context())
            {
                // Useful tactic ONLY in development.
                // At start of your program, always delete the database and then re-create it
                // This ensures a fresh database everytime you run your program.
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }

            // Add a blog with posts right away
            using (var db = new Context())
            {
                Studio studio = new Studio 
                {
                    Name = "20th Century Fox",
                    Movies = new List<Movie>
                    {
                        new Movie { Title = "Avatar", Genre = "Action" },
                        new Movie { Title = "Deadpool", Genre = "Action" },
                        new Movie { Title = "Apollo 13", Genre = "Drama" },
                        new Movie { Title = "The Martian", Genre = "Sci-Fi" }
                    }
                };

                Studio studio2 = new Studio 
                {
                    Name = "Universal Pictures"
                };

                db.Add(studio);
                db.Add(studio2);
                db.SaveChanges();
            }

            // Add posts to an existing blog by updating post.Blog object
            using (var db = new Context())
            {
                Movie movie = new Movie { Title = "Jurassic Park", Genre = "Action"};
                movie.Studio = db.Studios.Where(b => b.Name == "Universal Pictures").First();
                db.Add(movie);
                db.SaveChanges();
            }

            // Move post from one studio to another
            using (var db = new Context())
            {
                Movie Movie = db.Movies.Where(p => p.Title == "Apollo 13").First();
                Movie.Studio = db.Studios.Where(b => b.Name == "Universal Pictures").First();
                //db.Add(Movie);
                db.SaveChanges();
            }
            using (var db = new Context())
            {
                Movie Movie = db.Movies.Where(p => p.Title == "Deadpool").First();
                db.Remove(Movie);
                db.SaveChanges();
            }
            
            // Eager Loading
            using (var db = new Context())
            {
                var studios = db.Studios.Include(b => b.Movies);
                foreach (var b in studios)
                {
                    Console.WriteLine(b);
                    foreach (var p in b.Movies)
                    {
                       Console.WriteLine("\t" + p);
                    }
                }
            }
        }
    }
}


