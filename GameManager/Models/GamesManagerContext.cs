using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;


namespace GamesManager.Models
{
    public class GamesManagerContext : DbContext
    {
        public GamesManagerContext(DbContextOptions<GamesManagerContext> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .HasOne(g => g.Publisher)
                .WithMany(p => p.Games);

            modelBuilder.Entity<GameGenre>()
                .HasOne(gg=>gg.Game)
                .WithMany(game=>game.GameGenres)
                .HasForeignKey(gg => gg.GameId);

            modelBuilder.Entity<GameGenre>()
                .HasOne(gg => gg.Genre)
                .WithMany(genre => genre.GameGenres)
                .HasForeignKey(gg => gg.GenreId);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=gamesdb;Username=postgres;Password=1");
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<GameGenre> GameGanres { get; set; }
        public DbSet<Genre> Genres { get; set; }
        
        public bool TryToAddItem<T>(object item)
        {
            if (typeof(T) == typeof(Game))                       /// Dictionary<Type,Set>
            {
                Game game = item as Game;
                if (Games.Find(game.Id) != null)
                {
                    return false;
                }
                Games.Add(game);
            }
            else if (typeof(T) == typeof(Publisher))
            {
                Publisher publisher = item as Publisher;
                if (Publishers.Find(publisher.Id) != null)
                {
                    return false;
                }
                Publishers.Add(publisher);
            }
            else
            {
                return false;
            }
            SaveChanges();
            return true;
        }

        public bool TryToRemoveItem<T>(int id)
        {
            if (typeof(T) == typeof(Game))
            {
                Game game = Games.Find(id);
                if (game == null)
                {
                    return false;
                }
                Games.Remove(game);
            }
            else if (typeof(T) == typeof(Publisher))
            {
                Publisher publisher = Publishers.Find(id);
                if (publisher == null)
                {
                    return false;
                }
                Publishers.Remove(publisher);
            }
            else
            {
                return false;
            }

            SaveChanges();
            return true;
        }

        public T GetValue<T>(int id)
        {
            if (typeof(T) == typeof(Game))
            {
                return (T)(Games.Include(g=>g.Publisher).Include(g=>g.GameGenres).ThenInclude(g=>g.Genre).FirstOrDefault(p=> p.Id == id) as object);
            }
            else if (typeof(T) == typeof(Publisher))
            {
                return (T)(Publishers.Find(id) as object);
            }
            return default(T);
        }
    }
}
