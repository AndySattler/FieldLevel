using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace FieldLevel
{
    public class AppConfiguration : Infrastructure.Repositories.IRepositoryConfiguration
    {
        private readonly IConfiguration _config;

        public AppConfiguration(IConfiguration config)
        {
            _config = config;
        }

        public int GetCacheExpirationTimeInSeconds() => int.Parse(_config["CacheExpirationTimeInSeconds"]);
    }
}
