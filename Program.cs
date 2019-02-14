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
                        new Movie { Title = "Apollo 13 ", Genre = "Drama" },
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
                Movie movie = new Movie { Title = "Intro to JS. NEW!"};
                //Blog blogToUpdate = db.Blogs.Include(b => b.Posts).First();
                //blogToUpdate.Posts.Add(post);
                movie.Studio = db.Studios.First();
                db.Add(movie);
                db.SaveChanges();
            }

            // Add post to an existing blog by adding it to the List<Post>
            using (var db = new Context())
            {
                Movie post = new Movie { Title = "My First Coding Book for Babies"};
                Studio blogToUpdate = db.Studios.Include(b => b.Movies).Where(b => b.Name == "Blog 2").First();
                blogToUpdate.Movies.Add(post);
                db.SaveChanges();
            }

            // Move post from one blog to another
            using (var db = new Context())
            {
                Movie movie = db.Movies.Where(p => p.Title == "Intro to JS. NEW!").First();
                movie.Studio = db.Studios.Where(b => b.Name == "Blog 2").First();
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

            // Advanced Challenge: Filter on child entity with projection
            // GroupJoin() and SelectMany() might also be useful
            // Or Explicit loading
            //  db.Entry(b).Collection(x => x.Posts).Load(); // explicit loads
            using (var db = new Context())
            {
                // I want to select all blogs that have a post containing C# in the title
                // and just those posts
                // var query = db.Posts.Where(p => p.Title.Contains("C#")) // This will give you the posts, but not blogs
                
                // This gives blogs with only those C# posts but as a new anonymous type
                var blogsWithCSharpPosts = db.Studios.Include(b => b.Movies).Select(b => new
                    {
                        Name = b.Name,
                        // Filter on child entity within the select projection
                        Movies = b.Movies.Where(p => p.Title.Contains("C#")).ToList()
                    }).Where(b => b.Movies.Count() > 0);
                
                Console.WriteLine("\nPosts just containing C# with their blog: ");
                foreach (var b in blogsWithCSharpPosts)
                {
                    Console.WriteLine($"Blog Url: {b.Name}");
                    foreach (var p in b.Movies)
                    {
                        Console.WriteLine($"\t{p.Title}");
                    }
                }


            }
        }
    }
}


