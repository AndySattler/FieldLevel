using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Infrastructure.Repositories
{
    public interface IPostRepository
    {
        Task<DTO.Post> GetLatestAuthorPost(int authorId);
    }

    public class PostRepository : IPostRepository
    {
        private readonly ThirdPartyHttpClient.IHttpClient _httpClient;
        private readonly IMapper _mapper;
        private DateTime _lastUpdated;
        private static List<DTO.Post> _posts = new List<DTO.Post>();
        private readonly IRepositoryConfiguration _repositoryConfiguration;

        public PostRepository(ThirdPartyHttpClient.IHttpClient httpclient, IMapper mapper, IRepositoryConfiguration repositoryConfiguration)
        {
            _httpClient = httpclient;
            _mapper = mapper;
            _lastUpdated = DateTime.MinValue;
            _repositoryConfiguration = repositoryConfiguration;
        }

        public async Task<DTO.Post> GetLatestAuthorPost(int authorId)
        {
            await UpdatePostsIfNecessary();

            var lastPost = _posts.Where(p => p.UserId == authorId).OrderByDescending(p => p.Id).FirstOrDefault();

            if (lastPost == null)
                return null;

            return lastPost;
        }

        private async Task UpdatePostsIfNecessary()
        {
            if(_lastUpdated.AddSeconds(_repositoryConfiguration.GetCacheExpirationTimeInSeconds()) <= DateTime.Now)
            {
                var clientPosts = await _httpClient.GetPosts();
                _posts = _mapper.Map<IEnumerable<DTO.Post>>(clientPosts).ToList();
            }
        }

        
    }
}
