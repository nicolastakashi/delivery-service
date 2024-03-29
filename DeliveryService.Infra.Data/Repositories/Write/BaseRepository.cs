﻿using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Repositories.Write;
using DeliveryService.Infra.CustomExceptions;
using DeliveryService.Infra.Data.Context;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DeliveryService.Infra.Data.Repositories.Write
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        protected readonly IMongoContext Context;
        protected readonly IRedisContext RedisContext;
        protected IMongoCollection<TEntity> Collection;
        private readonly string _collection;

        public BaseRepository(IMongoContext mongoContext, string collection)
        {
            Context = mongoContext;
            Collection = Context.GetCollection<TEntity>(collection);
            _collection = collection;
        }


        public async Task<bool> AlreadyExistsAsync(Expression<Func<TEntity, bool>> expression)
        {
            try
            {
                var result = await Task.Run(() =>
                {
                    return Context.GetCollection<TEntity>(_collection)
                                  .AsQueryable()
                                  .Where(expression.Compile())
                                  .Any();
                });

                return result;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException($"Error to check if {nameof(TEntity)} exists.", ex);
            }
        }

        public virtual async Task CreateAsync(TEntity entity)
        {
            try
            {
                await Context.GetCollection<TEntity>(_collection).InsertOneAsync(entity);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException($"Error to create {nameof(entity)}", ex);
            }
        }

        public virtual async Task<TEntity> FindAsync(string id)
        {
            try
            {
                return await Context.GetCollection<TEntity>(_collection)
                    .Find(e => e.Id == ObjectId.Parse(id) && e.Active)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException($"Error to find {nameof(TEntity)}", ex);
            }
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            try
            {
                await Context.GetCollection<TEntity>(_collection)
                    .FindOneAndReplaceAsync(x => x.Id == entity.Id, entity);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException($"Error to update {entity}", ex);
            }
        }
    }
}
