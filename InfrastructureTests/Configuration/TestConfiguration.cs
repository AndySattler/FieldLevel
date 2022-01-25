using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfrastructureTests.Configuration
{
    public class TestConfiguration : Infrastructure.Repositories.IRepositoryConfiguration
    {
        public int GetCacheExpirationTimeInSeconds() => 5;
    }
}
