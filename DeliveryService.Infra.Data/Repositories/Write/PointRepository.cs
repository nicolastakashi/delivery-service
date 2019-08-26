﻿using DeliveryService.Domain.Repositories.Write;
using DeliveryService.Domain.ValueObject;
using DeliveryService.Infra.CustomExceptions;
using DeliveryService.Infra.Data.Constants;
using DeliveryService.Infra.Data.Context;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace DeliveryService.Infra.Data.Repositories.Write
{
    public class PointRepository : BaseRepository<Point>, IPointRepository
    {
        public PointRepository(IMongoContext context)
            : base(context, MongoCollections.Point)
        {
        }

        public override async Task<bool> AlreadyExistsAsync(Point point)
        {
            try
            {
                return await Context.GetCollection<Point>(Collection)
                    .Find(x => x.Name == point.Name && x.Active)
                    .AnyAsync();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error to verify if point already exists", ex);
            }
        }
    }
}
