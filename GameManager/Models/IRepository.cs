using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesManager.Models
{
    public interface IRepository<T>
    {
        IQueryable<T> Items { get; }
        bool TryToAddItem(T item);
        bool TryToRemoveItem(int id);
        bool TryToUpdate(int id,T item);
        T GetItem(int id);
    }
}
