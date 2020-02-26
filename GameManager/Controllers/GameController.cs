using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using GamesManager.ActionResults;
using GamesManager.Infrastructure.Services;
using GamesManager.Models;
using GamesManager.Services.Handlers.Games.Create;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Game = DataLayer.Entities.Game;


namespace GamesManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController] //ApiErrorHandler
    public class GamesController : Controller/*ApiController<Game>*/
    {
        private readonly IGameEditCreateFormHandler gameEditCreateFormHandler;
        //private readonly IGameCreateCreateFormHandler gameCreateCreateFormHandler;
        private readonly IReadDbContext<IDbEntity> readDbContext;
        private readonly IWriteDbContext<IDbEntity> writeDbContext;

        
        public GamesController(IReadDbContext<IDbEntity> readDbContext,IWriteDbContext<IDbEntity> writeDbContext,IGameEditCreateFormHandler gameEditCreateFormHandler)// : base(repo)
        {
            this.readDbContext = readDbContext;
            this.writeDbContext = writeDbContext;
            this.gameEditCreateFormHandler = gameEditCreateFormHandler;
        }

        [HttpGet]
        [Route("getAll")]
        public IActionResult GetAll()
        {
            var result = ModelDataResult<List<Game>>.BuildSucces(readDbContext.Get<Game>().ToList());
            //readDbContext.GetGameModelData(form.Id); // methodExtension

            return ApiModelResult.Create(result);
        }

        //[HttpPost]
        //public override async Task<ActionResult<Game>> PostItem(Game item)
        //{
        //    if (!await Task.Run(() => repository.TryToAddItem(item)))
        //    {
        //        return BadRequest();
        //    }

        //    return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
        //}

        //[HttpGet("{genre:regex(\\D)}")]
        //public async Task<ActionResult<IEnumerable<Game>>> GetItemsByGenre(string genre)
        //{
        //    return await repository.Items.Where(game => game.GameGenres.Any(g => g.Genre.GenreTitle == genre)).ToListAsync();
        //}
    }
}
