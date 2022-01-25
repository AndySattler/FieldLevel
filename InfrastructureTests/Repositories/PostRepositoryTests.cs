using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutoMapper;
using ThirdPartyHttpClient;
using Infrastructure.Repositories;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace InfrastructureTests.Repositories
{
    [TestClass]
    public class PostRepositoryTests
    {
       
        [TestMethod]
        public async Task RepositoryCachingTest()
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new Infrastructure.DTO.AutoMapperMapping())));
            var httpClient = new Moq.Mock<IHttpClient>();

            httpClient.SetupSequence(p => p.GetPosts())
                .Returns(Task.FromResult(new ThirdPartyHttpClient.DTO.Post[] {
                    new ThirdPartyHttpClient.DTO.Post()
                    {
                        UserId = 1,
                        Id = 1,
                        Title = "first post",
                        Body = "first post"
                    }
                }.AsEnumerable()))
                .Returns(Task.FromResult(new ThirdPartyHttpClient.DTO.Post[] {
                     new ThirdPartyHttpClient.DTO.Post()
                    {
                        UserId= 1,
                        Id = 1,
                        Title = "first post",
                        Body = "first post"
                    },
                    new ThirdPartyHttpClient.DTO.Post()
                    {
                        UserId =1,
                        Id = 2,
                        Title = "second post",
                        Body = "second post"
                    }
                }.AsEnumerable()));


            IPostRepository postRepository = new PostRepository(httpClient.Object, mapper, new Configuration.TestConfiguration());

            var firstResult = await postRepository.GetLatestAuthorPost(1);
            Assert.AreEqual(1, firstResult.Id, "The First Post Should be one");


            var secondResult = await postRepository.GetLatestAuthorPost(1);
            Assert.AreEqual(1, secondResult.Id, "The second Post Should be one because it is cached");


            var milliseconds = new Configuration.TestConfiguration().GetCacheExpirationTimeInSeconds() * 1000;
            await Task.Delay(milliseconds);  

            var thirdResult = await postRepository.GetLatestAuthorPost(1);
            Assert.AreEqual(2, thirdResult.Id, "The third Post Should be two because it resynced");
        }

        [TestMethod]
        public async Task RepositoryGetLastPost()
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new Infrastructure.DTO.AutoMapperMapping())));
            var httpClient = new Moq.Mock<IHttpClient>();

            httpClient.Setup(p => p.GetPosts())
                .Returns(Task.FromResult(new ThirdPartyHttpClient.DTO.Post[] {
                    new ThirdPartyHttpClient.DTO.Post()
                    {
                        UserId = 1,
                        Id = 1,
                        Title = "first post",
                        Body = "first post"
                    },
                     new ThirdPartyHttpClient.DTO.Post()
                    {
                        UserId = 1,
                        Id = 2,
                        Title = "second post",
                        Body = "second post"
                    }
                     ,
                          new ThirdPartyHttpClient.DTO.Post()
                    {
                        UserId = 1,
                        Id = 3,
                        Title = "third post",
                        Body = "third post"
                      },
                              new ThirdPartyHttpClient.DTO.Post()
                    {
                        UserId = 2,
                        Id = 4,
                        Title = "4th post another author",
                        Body = "4th post another author"
                      }

                }.AsEnumerable()));
                
            IPostRepository postRepository = new PostRepository(httpClient.Object, mapper, new Configuration.TestConfiguration());

            var post = await postRepository.GetLatestAuthorPost(1);
            Assert.AreEqual(3, post.Id, "The last post is the third one");
        }

        [TestMethod]
        public async Task RepositoryGetAuthorPost()
        {
            IMapper mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new Infrastructure.DTO.AutoMapperMapping())));
            var httpClient = new Moq.Mock<IHttpClient>();

            httpClient.Setup(p => p.GetPosts())
                .Returns(Task.FromResult(new ThirdPartyHttpClient.DTO.Post[] {
                    new ThirdPartyHttpClient.DTO.Post()
                    {
                        UserId = 1,
                        Id = 1,
                        Title = "first post",
                        Body = "first post"
                    },
                     new ThirdPartyHttpClient.DTO.Post()
                    {
                        UserId = 2,
                        Id = 2,
                        Title = "second post",
                        Body = "second post"
                    }
                     ,
                          new ThirdPartyHttpClient.DTO.Post()
                    {
                        UserId = 3,
                        Id = 3,
                        Title = "third post",
                        Body = "third post"
                      },
                              new ThirdPartyHttpClient.DTO.Post()
                    {
                        UserId = 4,
                        Id = 4,
                        Title = "4th post another author",
                        Body = "4th post another author"
                      }

                }.AsEnumerable()));

            IPostRepository postRepository = new PostRepository(httpClient.Object, mapper, new Configuration.TestConfiguration());

            var post = await postRepository.GetLatestAuthorPost(2);
            Assert.AreEqual(2, post.Id, "Should grab the correct author post");
        }
    }
}
