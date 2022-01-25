using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThirdPartyHttpClient;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using System.Linq;

namespace ThirdPartyHttpClientTests
{
    [TestClass]
    public class HttpClientTests
    {

        [TestMethod]
        public async Task GetTest()
        {
            IHttpClient client = new HttpClient();

            var result = await client.GetPosts();
            Assert.AreEqual(true, result.Any(), "There are no resulst");
        }
    }
}
