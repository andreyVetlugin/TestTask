using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using GamesManager.Infrastructure.Services;
using GamesManager.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace GamesManager.Services.Handlers.Games
{
    public class GameEditFormCreateHandler: IGameEditFormCreateHandler
    {
        private readonly IWriteDbContext<IDbEntity> writeDbContext;
        private readonly IReadDbContext<IDbEntity> readDbContext;

        public GameEditFormCreateHandler(IWriteDbContext<IDbEntity> writeDbContext, IReadDbContext<IDbEntity> readDbContext)
        {
            this.writeDbContext = writeDbContext;
            this.readDbContext = readDbContext;
        }

        public OperationResult Handle(GameEditForm form, ModelStateDictionary modelState)
        {
            var publisher = new Publisher();
            //Publisher publish =  обработать и создать если требуется 
            // List<> Gamegenres обработать, разделить по запятым 
            // проверить дату на будущее время 
            // если что-то не так вернуть соотв. OperationResult

            var game = new Game
            {
                Id = Guid.NewGuid(),
                Publisher = publisher,
                Title = form.Title,
                ReleaseDate = form.ReleaseDate
            };

            var gameGenres = form.GameGenres.Split(',')
                .Select(s=>s.Trim())
                .Select(genreTitleFromForm =>
                {                       // метод в дб GetFirstOrDefault<T>(func<> // функция для firstOrDefault)
                    var genreFromDb = readDbContext.Get<Genre>().FirstOrDefault(g => g.GenreTitle == genreTitleFromForm); //проверка на существование в бд такого жанра

                    if (genreFromDb != null)
                    {
                        return new GameGenre
                        {
                            Game = game,
                            Genre = genreFromDb // добавить id? 
                        };
                    }

                    return new GameGenre
                    {
                        Game = game,
                        Genre = new Genre
                        {
                            GenreTitle = genreTitleFromForm 
                        }
                    };
                }).ToList();

            game.GameGenres = gameGenres;
            //gameGenres.
            //readDbContext.Get<Publisher>()
           
            writeDbContext.Add(game);

            return OperationResult.BuildSuccess(UnitOfWork.WriteDbContext(writeDbContext));
        }
    }
}
