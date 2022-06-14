using FPIS.Redis.Nuget.Package;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Redis.Test
{
    [TestClass]
    public class RedisCacheServiceTests
    {
        const string cacheKey = "insiderdevuat.redis.cache.windows.net:6380,password=WiFr3GDaTodXsAzIW2O0BCF2ztJmAEUt+00dmfpu53E=,ssl=True,sslprotocols=tls12,abortConnect=False";//_config.GetSection("RedisCacheConnection").Value;;
        RedisCacheService<string> _service = new RedisCacheService<string>(cacheKey);

        [TestMethod]
        [DataRow("223", "", "10-10-2022")]
        public void AddToCache(string key, string data, string expDate)
        {
            DateTime expirationDate = Convert.ToDateTime(expDate);
            _service.AddToCache(key, data, expirationDate);

            var cacheData = _service.GetFromCache(key);
            Assert.AreEqual(string.Empty, cacheData);
        }
    }
}