using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using GamesManager.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DataLayer.Infrastructure;

namespace GamesManager.Models.Games
{
    public class GameEditForm
    {
        public Guid Id { get; set; }
        [Display(Name = "Название игры"), Required(ErrorMessage = "Введите {0}")]
        public string Title { get; set; }
        [Display(Name = "Дата релиза"), Required(ErrorMessage = "Введите Дату релиза")]
        public DateTime ReleaseDate { get; set; } //тип ???? нужна ли проверка ///на существование ? 
        [Display(Name = "Жанры игры"), Required(ErrorMessage = "Введите хоть один игровой жанр")]
        public string GameGenres { get; set; } //???    //List<Genre>?
        [Display(Name = "Наименование издателя"), Required(ErrorMessage = "Введите {0}")]
        public string Publisher { get; set; }

        public static GameEditForm CreateFromGame(Game game) // засунуть в класс GameEditFormHelper????? 
        {                   //где-то здесь проверку на существование игры\игр
            //game = readDbContext<Game>

            return new GameEditForm
            {
                GameGenres = String.Join(",", game.GameGenres.Select(genre => genre.Genre.GenreTitle)),
                ReleaseDate = game.ReleaseDate,
                Publisher = game.Publisher.Title,
                Title = game.Title,
                Id = game.Id
            };
        }

        public static IEnumerable<GameEditForm> CreateFromExistingGames(IReadDbContext<IDbEntity> readDbContext)
        {
            return readDbContext.Get<Game>()
                .IncludeDependencies()
                .Select(game => CreateFromGame(game));
        }

        //public static Game

        public static OperationResult JoinDependenciesToExistingGame(Game game, GameEditForm form, IReadDbContext<IDbEntity> readDbContext,IWriteDbContext<IDbEntity> writeDbContext) // вернуть игру или OperationResult?
        {
            var publisher = readDbContext.Get<Publisher>()
                .Include(p=>p.Games)
                .FirstOrDefault(p => p.Title == form.Publisher);
            if (publisher == null)
            {
                publisher = new Publisher
                {
                    Title = form.Publisher
                };
            }
            else
            {
                //writeDbContext.Attach(publisher);// проверить нужно ли 
                publisher.Games.Add(game);
            }

            var gameGenres = form.GameGenres.Split(',')
                .Select(s => s.Trim())
                .Select(genreTitleFromForm =>
                {                       // метод в дб GetFirstOrDefault<T>(func<> // функция для firstOrDefault)
                    var genreFromDb = readDbContext.Get<Genre>().FirstOrDefault(g => g.GenreTitle == genreTitleFromForm);

                    if (genreFromDb != null)
                    {
                       // writeDbContext.Attach(genreFromDb);
                        return new GameGenre
                        {
                            Game = game,
                            Genre = genreFromDb
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
            game.Publisher = publisher;
            return OperationResult.BuildSuccess(UnitOfWork.None());
        }
    }
}
