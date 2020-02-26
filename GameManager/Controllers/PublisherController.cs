using GamesManager.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesManager.Controllers
{
    //public class PublishersController : ApiController<Publisher>
    //{
    //    public PublishersController(IRepository<Publisher> repo) : base(repo)
    //    {
    //    }

    //    [HttpPost]
    //    public async override Task<ActionResult<Publisher>> PostItem(Publisher item)
    //    {
    //        if (!await Task.Run(() => repository.TryToAddItem(item)))
    //        {
    //            return BadRequest();
    //        }
    //        return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item); /////  ППРОВЕРИТЬ!"!!!!!!!!!
    //    }
    //}
}
