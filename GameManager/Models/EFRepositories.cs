using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesManager.Models
{
    public class EFRepository<T> : IRepository<T>
    {
        protected GamesManagerContext context;
        public virtual IQueryable<T> Items { get; }

        public EFRepository(GamesManagerContext context) => this.context = context;

        public virtual bool TryToAddItem(T item) => context.TryToAddItem<T>(item);

        public virtual bool TryToRemoveItem(int id) => context.TryToRemoveItem<T>(id); // !!!

        public virtual bool TryToUpdate(int id, T item) => context.TryToUpdateItem<T>(id,item);

        public virtual T GetItem(int id) => context.GetValue<T>(id);      
    }

    public class EFGameRepository : EFRepository<Game>
    {
        public EFGameRepository(GamesManagerContext context) : base(context)
        {

        }

        public override bool TryToAddItem(Game game)
        {

            Publisher publisher = context.Publishers.FirstOrDefault(p => p.Title == game.Publisher.Title);// проверяем есть ли уже в базе данных такой Publisher

            if (publisher != null)
            {
                game.Publisher = publisher;
            }
            for (int i = 0; i < game.GameGenres.Count(); i++)            // проверяем есть ли уже в базе данных такие жанры
            {
                Genre genre = context.Genres.FirstOrDefault(p => p.GenreTitle == game.GameGenres[i].Genre.GenreTitle);
                if (genre != null)
                {
                    game.GameGenres[i].Genre = genre;
                }
            }

            return base.TryToAddItem(game);
        }

        //public new bool TryToUpdate(int id, Game game)
        //{
        //    game.Id = id;
        //    return context.TryToUpdateItem<Game>(game);
        //}

        //public new bool TryToUpdate(int id, Game game) => id != game.Id || context.TryToUpdateItem<Game>(game);

        public override IQueryable<Game> Items => context.Games.Include(g => g.Publisher).Include(g => g.GameGenres).ThenInclude(g => g.Genre);
    }

    public class EFPublisherRepository : EFRepository<Publisher>
    {
        public EFPublisherRepository(GamesManagerContext context) : base(context)
        {

        }

        public override IQueryable<Publisher> Items => context.Publishers;
    }
}
