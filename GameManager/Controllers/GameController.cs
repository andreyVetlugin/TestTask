using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using GamesManager.ActionResults;
using GamesManager.Infrastructure.Services;
using GamesManager.Models;
using GamesManager.Services.Handlers.Games;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace GamesManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController] //ApiErrorHandler
    public class GamesController : Controller/*ApiController<Game>*/
    {
        private readonly IGameEditFormCreateHandler gameEditFormCreateHandler;//gameEditCreateFormHandler;
        private readonly GameEditFormEditHandler gameEditFormFormEditHandler;
        private readonly IReadDbContext<IDbEntity> readDbContext;
        private readonly IWriteDbContext<IDbEntity> writeDbContext;


        public GamesController(IReadDbContext<IDbEntity> readDbContext, IWriteDbContext<IDbEntity> writeDbContext, IGameEditFormCreateHandler gameEditFormCreateHandler)// : base(repo)
        {
            this.readDbContext = readDbContext;
            this.writeDbContext = writeDbContext;
            this.gameEditFormCreateHandler = gameEditFormCreateHandler;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = ModelDataResult<List<GameEditForm>>.BuildSucces(GameEditForm.CreateFromGamesIntoDb(readDbContext).ToList());
            return ApiModelResult.Create(result);
        }

        [HttpPost]
        public IActionResult Create(GameEditForm form)
        {
            var result = gameEditFormCreateHandler.Handle(form, ModelState);
            return new ApiOperationResult(result);
        }

        [HttpPut]
        public IActionResult Edit(GameEditForm form)
        {
            
            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
        }

        //[HttpGet("{genre:regex(\\D)}")]
        //public async Task<ActionResult<IEnumerable<Game>>> GetItemsByGenre(string genre)
        //{
        //    return await repository.Items.Where(game => game.GameGenres.Any(g => g.Genre.GenreTitle == genre)).ToListAsync();
        //}
    }
}
