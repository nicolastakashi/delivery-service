using DeliveryService.Domain.Entities;
using DeliveryService.Domain.Repositories.Write;
using DeliveryService.Infra.CustomExceptions;
using DeliveryService.Infra.Data.Constants;
using DeliveryService.Infra.Data.Context;
using MongoDB.Driver;
using System;
using System.Threading.Tasks;

namespace DeliveryService.Infra.Data.Repositories.Write
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IMongoContext _context;

        public AccountRepository(IMongoContext context)
        {
            _context = context;
        }

        public Task<User> GetUserByEmailAndPassword(string email, string password)
        {
            try
            {
                return _context.GetCollection<User>(MongoCollections.User)
                    .Find(x => x.Email == email && x.Password == password)
                    .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("An error occurs to get user", ex);
            }
        }
    }
}
