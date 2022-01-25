using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Services.Service;

namespace FieldLevel.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorController : ControllerBase
    {
      
        private readonly ILogger<AuthorController> _logger;
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public AuthorController(
            ILogger<AuthorController> logger,
            IPostService postService,
            IMapper mapper)
        {
            _logger = logger;
            _postService = postService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("{id}/latestpost")]
        public async Task<ActionResult<DTO.Post>> Get(int id)
        {
            try
            {
                var lastPost = await _postService.GetLatestAuthorPost(id);

                if (lastPost == null)
                    return NotFound();

                return Ok(_mapper.Map<DTO.Post>(lastPost));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error Getting latest post for author {id} " + ex.Message);
            }

            return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError);
        }
    }
}
