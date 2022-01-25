using System.Threading.Tasks;
using Infrastructure.Repositories;
using AutoMapper;

namespace Services.Service
{
    public interface IPostService
    {
        Task<DTO.Post> GetLatestAuthorPost(int authorId);
    }

    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public PostService(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<DTO.Post> GetLatestAuthorPost(int authorId)
        {
            var post = await _postRepository.GetLatestAuthorPost(authorId);

            if (post == null)
                return null;

            return _mapper.Map<DTO.Post>(post);
        }
    }
}
