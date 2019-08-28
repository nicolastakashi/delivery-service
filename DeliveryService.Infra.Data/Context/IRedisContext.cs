using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryService.Infra.Data.Context
{
    public interface IRedisContext
    {
        bool Save<TEntity>(string key, TEntity value);
        TEntity Get<TEntity>(string key);
        void DeleteAllByPattern(string pattern);
    }
}
