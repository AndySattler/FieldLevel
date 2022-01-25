using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FieldLevel.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
      
        private readonly ILogger<AuthorController> _logger;
        private readonly Infrastructure.Repositories.IPostRepository _repo;

        public AuthorController(ILogger<AuthorController> logger, ThirdPartyHttpClient.IHttpClient client, Infrastructure.Repositories.IPostRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet]
        [Route("{id}/latestpost")]
        public async Task<ActionResult<DTO.Post>> Get(int id)
        {
            var lastPost = await _repo.GetLatestAuthorPost(id);
            
            if (lastPost == null)
                return NotFound();

            return Ok(lastPost);
        }
    }
}
