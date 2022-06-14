using StackExchange.Redis;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FPIS.Redis.Nuget.Package
{
    public class RedisCacheService<T>
    {
        Lazy<ConnectionMultiplexer> _connection;
        public RedisCacheService(string cacheKey)
        {
            _connection = new Lazy<ConnectionMultiplexer>(ConnectionMultiplexer.Connect(cacheKey));
        }
        public void AddToCache(string key, T data, DateTime expirationDate)
        {
            var cache = _connection.Value.GetDatabase();
            var serializedObject = JsonConvert.SerializeObject(data);
            var expirationTime = expirationDate.Subtract(DateTime.UtcNow);

            try
            {
                cache.StringSet(key, serializedObject, expirationTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T GetFromCache(string key)
        {
            var cache = _connection.Value.GetDatabase();
            try
            {
                var cacheObject = cache.StringGet(key);
                if (cacheObject.HasValue && cacheObject.ToString() != null)
                {
                    var deserializedObject = JsonConvert.DeserializeObject<T>(cacheObject.ToString());

                    if (deserializedObject != null)
                        return deserializedObject;
                }

                return default(T);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}