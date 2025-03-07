using TicketBookingSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;

public class DataSeeder
{
    public static void SeedData(TicketContext context)
    {
        // Step 1: Add Theaters (only if they don't already exist)
        if (!context.Theaters.Any())
        {
            var theaters = new List<Theater>
            {
                new Theater { TheaterId = "T1", TheaterName = "Theater 1", Location = "Location 1", TotalSeats = 100, CreatedTime = DateTime.Now, UpdatedTime = DateTime.Now },
                new Theater { TheaterId = "T2", TheaterName = "Theater 2", Location = "Location 2", TotalSeats = 150, CreatedTime = DateTime.Now, UpdatedTime = DateTime.Now }
            };

            // Add Theaters to the database
            context.Theaters.AddRange(theaters);
            context.SaveChanges(); // Save so that TheaterIds are generated
        }

        // Step 2: Add Showtimes for each Movie and Theater
        var movies = context.Movies.ToList(); // Fetch movies from the database
        var theaterList = context.Theaters.ToList(); // Fetch theaters from the database (renamed from theaters to theaterList)

        // Ensure both movies and theaters exist before proceeding
        if (movies.Any() && theaterList.Any())
        {
            // Add showtimes for each movie in each theater
            foreach (var movie in movies)
            {
                foreach (var theater in theaterList)  // Use theaterList here
                {
                    for (int k = 1; k <= 3; k++) // For 3 showtimes per theater per movie
                    {
                        var showtime = new Showtime
                        {
                            ShowTimeId = $"{movie.MovieId}_{theater.TheaterId}_{k}", // Unique ShowtimeId
                            MovieId = movie.MovieId,
                            TheaterId = theater.TheaterId,
                            ShowDate = DateOnly.FromDateTime(DateTime.Now.AddDays(k)), // Show on consecutive days
                            ShowTime = TimeOnly.FromDateTime(DateTime.Now.AddHours(k + 1)), // Showtime 1, 2, and 3 hours later
                            AvailableSeats = theater.TotalSeats, // Seats available based on theater size
                            CreatedTime = DateTime.Now,
                            UpdatedTime = DateTime.Now
                        };

                        // Add Showtime to the movie and theater collections
                        movie.Showtimes ??= new List<Showtime>(); // Initialize Showtimes if null
                        movie.Showtimes.Add(showtime); // Associate showtime with movie

                        theater.Showtimes ??= new List<Showtime>(); // Initialize Showtimes if null
                        theater.Showtimes.Add(showtime); // Associate showtime with theater

                        // Add the showtime to the context
                        context.Showtimes.Add(showtime);
                    }
                }
            }

            // Save the showtimes to the database
            context.SaveChanges();
        }
    }
}
