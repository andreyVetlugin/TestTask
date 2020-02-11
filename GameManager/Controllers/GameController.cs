using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamesManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace GamesManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ApiController<Game>
    {
        public GamesController(IRepository<Game> repo) : base(repo)
        {
         
        }

        [HttpPost]
        public async override Task<ActionResult<Game>> PostItem(Game item)
        {
            if (!await Task.Run(() => repository.TryToAddItem(item)))
            {
                return BadRequest();
            }

            return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item);
        }

        [HttpGet("{genre:regex(\\D)}")]
        public async Task<ActionResult<IEnumerable<Game>>> GetItemsByGenre(string genre)
        {
            return await repository.Items.Where(game => game.GameGenres.Any(g => g.Genre.GenreTitle == genre)).ToListAsync();
        }
    }
}
