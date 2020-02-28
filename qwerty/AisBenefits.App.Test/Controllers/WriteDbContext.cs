using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DataLayer.Entities;
using DataLayer.Infrastructure.DbContexts;

namespace AisBenefits.App.Test.Controllers
{
    class WriteDbContext : IWriteDbContext<IBenefitsEntity>
    {
        private readonly ICollection<IBenefitsEntity> repositiory;

        private readonly Dictionary<Guid, IBenefitsEntity> toAdd = new Dictionary<Guid, IBenefitsEntity>();
        private readonly Dictionary<Guid, IBenefitsEntity> toAttach = new Dictionary<Guid, IBenefitsEntity>();
        private readonly Dictionary<Guid, IBenefitsEntity> toRemove = new Dictionary<Guid, IBenefitsEntity>();

        public WriteDbContext(ICollection<IBenefitsEntity> repositiory)
        {
            this.repositiory = repositiory;
        }

        public void Attach<TEntity>(TEntity entity)
            where TEntity : class, IBenefitsEntity
        {
            toAttach.Add((entity as dynamic).Id, entity);

        }

        public void Add<TEntity>(TEntity entity)
            where TEntity : class, IBenefitsEntity
        {
            toAdd.Add((entity as dynamic).Id, entity);
        }

        public void Remove<TEntity>(TEntity entity)
            where TEntity : class, IBenefitsEntity
        {
            toRemove.Add((entity as dynamic).Id, entity);
        }

        public void RemoveRange<TEntity>(TEntity[] entityArray)
            where TEntity : class, IBenefitsEntity
        {
            foreach (var entity in entityArray)
            {
                Remove(entity);
            }
        }

        public void SaveChanges()
        {
            foreach (var (id, entity) in toRemove)
            {
                var removeEntity = repositiory.Single(e => (e as dynamic).Id == id);
                repositiory.Remove(removeEntity);
            }
            toRemove.Clear();
            foreach (var (id, entity) in toAdd)
            {
                if(repositiory.Any(e => (e as dynamic).Id == id))
                    throw new InvalidOperationException("Модель с тем же id уже добавлена");
                repositiory.Add(entity);
            }
            toAdd.Clear();
            foreach (var (id, entity) in toAttach)
            {
                var updateEntity = repositiory.Single(e => (e as dynamic).Id == id);

                Mapper.Map(entity, updateEntity, entity.GetType(), updateEntity.GetType());
            }
            toAttach.Clear();
        }

        public void Dispose()
        {
        }
    }
}
