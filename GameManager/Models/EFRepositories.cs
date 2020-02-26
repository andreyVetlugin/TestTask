using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesManager.Models
{
    public abstract class EFRepository<T> : IRepository<T>
    {
        protected GamesManagerContext context;
        public virtual IQueryable<T> Items { get; }

        public EFRepository(GamesManagerContext context) => this.context = context;

        public virtual bool TryToAddItem(T item) => context.TryToAddItem<T>(item);

        public virtual bool TryToRemoveItem(int id) => context.TryToRemoveItem<T>(id); // !!!

        public abstract bool TryToUpdate(int id, T item);

        public virtual T GetItem(int id) => context.GetValue<T>(id);      
    }

    public class EFGameRepository : EFRepository<Game>
    {
        public EFGameRepository(GamesManagerContext context) : base(context)
        {

        }

        public override bool TryToAddItem(Game game)
        {
            //сделать проверку на null в бд 
            Publisher publisher = context.Publishers.FirstOrDefault(p => p.Title == game.Publisher.Title);// проверяем есть ли уже в базе данных такой Publisher

            if (publisher != null)
            {
                game.Publisher = publisher;
            }
            for (int i = 0; game.GameGenres!=null && i < game.GameGenres.Count(); i++)            // проверяем есть ли уже в базе данных такие жанры
            {
                Genre genre = context.Genres.FirstOrDefault(p => p.GenreTitle == game.GameGenres[i].Genre.GenreTitle);
                if (genre != null)
                {
                    game.GameGenres[i].Genre = genre;
                }
            }

            return base.TryToAddItem(game);
        }

        public override bool TryToUpdate(int id, Game game)
        {

            Game gameFromDB = context.Games.Find(id);

            if (gameFromDB == null)
            {
                return false;
            }

            if (game.GameGenres != null)
            {
                context.GameGanres.RemoveRange(context.GameGanres.Where(gg => gg.GameId == id));
               
                for (int i = 0; i < game.GameGenres.Count(); i++)            
                {
                    Genre genre = context.Genres.FirstOrDefault(p => p.GenreTitle == game.GameGenres[i].Genre.GenreTitle);
                    if (genre != null)
                    {
                        gameFromDB.GameGenres[i].Genre = genre;
                    }
                    else
                    {
                        gameFromDB.GameGenres[i] = game.GameGenres[i];
                    }
                }

            }

            if (game.Publisher != null)
            {
                Publisher publisher = context.Publishers.FirstOrDefault(p => p.Title == game.Publisher.Title);
                gameFromDB.Publisher = publisher ?? game.Publisher;
            }

            gameFromDB.Title = game.Title ?? gameFromDB.Title;
            gameFromDB.ReleaseDate = game.ReleaseDate == default(DateTime) ? gameFromDB.ReleaseDate : game.ReleaseDate ;

            context.SaveChanges();
            return true;
        }

        public override IQueryable<Game> Items => context.Games.Include(g => g.Publisher).Include(g => g.GameGenres).ThenInclude(g => g.Genre);
    }

    public class EFPublisherRepository : EFRepository<Publisher>
    {
        public EFPublisherRepository(GamesManagerContext context) : base(context)
        {

        }

        public override IQueryable<Publisher> Items => context.Publishers;

        public override bool TryToUpdate(int id, Publisher publisher)
        {
            Publisher publisherFromDB = context.Publishers.Find(id);

            if (publisherFromDB == null)
            {
                return false;
            }

            publisherFromDB.Title = publisher.Title ?? publisherFromDB.Title;

            context.SaveChanges();

            return true;
        }
    }
}
