using DeliveryService.Domain.Repositories.Write;
using DeliveryService.Domain.ValueObject;
using DeliveryService.Infra.CustomExceptions;
using DeliveryService.Infra.Data.Constants;
using DeliveryService.Infra.Data.Context;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace DeliveryService.Infra.Data.Repositories.Write
{
    public class PointRepository : IPointRepository
    {
        private readonly IMongoContext _context;

        public PointRepository(IMongoContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Point point)
        {
            try
            {
                await _context.GetCollection<Point>(MongoCollections.Point).InsertOneAsync(point);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error to create a point", ex);
            }
        }

        public async Task<Point> FindAsync(string id)
        {
            try
            {
                return await _context.GetCollection<Point>(MongoCollections.Point)
                    .Find(x => x.Id == ObjectId.Parse(id))
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error to find the point.", ex);
            }
        }

        public async Task<bool> PointAlreadyExistsAsync(Point point)
        {
            try
            {
                return await _context.GetCollection<Point>(MongoCollections.Point)
                    .Find(x => x.Name == point.Name)
                    .AnyAsync();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error to verify if point already exists", ex);
            }
        }

        public async Task InactivePointAsync(Point point)
        {
            try
            {
                await _context.GetCollection<Point>(MongoCollections.Point)
                    .FindOneAndReplaceAsync(x => x.Id == point.Id, point);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error to remove a point", ex);
            }
        }

        public async Task UpdateAsync(Point point)
        {
            try
            {
                await _context.GetCollection<Point>(MongoCollections.Point)
                    .FindOneAndReplaceAsync(x => x.Id == point.Id, point);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error to update a point", ex);
            }
        }
    }
}
