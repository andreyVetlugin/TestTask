using GamesManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesManager.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    //public abstract class ApiController<T>: Controller
    //{
    //    protected readonly IRepository<T> repository;
    //    // GET: /<controller>/

    //    public ApiController(IRepository<T> repo)
    //    {
    //        repository = repo;            
    //    }

    //    [HttpGet]
    //    public async Task<ActionResult<IEnumerable<T>>> GetItems()
    //    {
    //        return await repository.Items.ToListAsync();
    //    }

    //    [HttpGet("{id:int}")]
    //    public ActionResult<T> Get(int id)
    //    {
    //        T item = repository.GetItem(id);
    //        //T item = await repository.Items.FirstOrDefaultAsync(it => it.Id == id); !!!!!!!!!
    //        //return (gameRepository.Items.FirstAsync(item => item.Id == id)) ?? NotFound();
    //        if (item == null)
    //        {
    //            return NotFound();
    //        }
    //        return item;
    //    }

    //    [HttpPost]
    //    public abstract Task<ActionResult<T>> PostItem(T item);
      
    //    [HttpPut("{id:int}")]
    //    public virtual async Task<IActionResult> Put(int id,T item)
    //    {
    //        if (!await Task.Run(() => repository.TryToUpdate(id, item)))
    //        {
    //            return BadRequest();
    //        }

    //        return NoContent();
    //    }

    //    [HttpDelete("{id:int}")]
    //    public async Task<IActionResult> DeleteItem(int id)
    //    {
    //        if (!await Task.Run(() =>repository.TryToRemoveItem(id)))
    //        {
    //            return NotFound();
    //        }
    //        return NoContent();
    //    }
    //}
} 
