using System;
using System.Collections.Generic;
using System.Text;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer
{
    public class GamesContext: DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<GameGenre> GameGenreses { get; set; }
        public DbSet<Genre> Genres { get; set; } 
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=gamesdb;Username=postgres;Password=1");
        }

        public GamesContext(DbContextOptions<GamesContext> options) 
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
