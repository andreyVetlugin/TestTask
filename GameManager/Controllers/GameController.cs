using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;
using GamesManager.ActionResults;
using GamesManager.Infrastructure.Services;
using GamesManager.Models;
using GamesManager.Models.Games;
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
        private readonly GameEditFormEditHandler gameEditFormEditHandler;
        private readonly GameEditFormGetAllHandler gameEditFormGetAllHandler;
        private readonly GameEditFormGetHandler gameEditFormGetHandler;
        private readonly GameEditFormRemoveHandler gameEditFormRemoveHandler;
        

        public GamesController
            (IGameEditFormCreateHandler gameEditFormCreateHandler, GameEditFormGetAllHandler gameEditFormGetAllHandler, 
            GameEditFormGetHandler gameEditFormGetHandler, GameEditFormRemoveHandler gameEditFormRemoveHandler,
            GameEditFormEditHandler gameEditFormEditHandler)
        {
            this.gameEditFormCreateHandler = gameEditFormCreateHandler;
            this.gameEditFormGetAllHandler = gameEditFormGetAllHandler;
            this.gameEditFormGetHandler = gameEditFormGetHandler;
            this.gameEditFormRemoveHandler = gameEditFormRemoveHandler;
            this.gameEditFormEditHandler = gameEditFormEditHandler;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var result = gameEditFormGetAllHandler.Handle();
            return ApiModelResult.Create(result);            
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(Guid id)
        {
            var result = gameEditFormGetHandler.Handle(id, ModelState);
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
            var result = gameEditFormEditHandler.Handle(form, ModelState);
            return new ApiOperationResult(result);
        }

        //[HttpGet("{genre:regex(\\D)}")]
        //public async Task<ActionResult<IEnumerable<Game>>> GetItemsByGenre(string genre)
        //{
        //    return await repository.Items.Where(game => game.GameGenres.Any(g => g.Genre.GenreTitle == genre)).ToListAsync();
        //}
    }
}
