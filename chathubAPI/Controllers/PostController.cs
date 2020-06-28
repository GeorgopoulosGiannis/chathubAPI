using chathubAPI.INTERFACES;
using chathubAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace chathubAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        readonly IPostRepo _postRepo;

        public PostController(IPostRepo postRepo)
        {
            _postRepo = postRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string postId)
        {
            try
            {
                Post post = await _postRepo.Read(postId).ConfigureAwait(false);
                return Ok(post);
            }
            catch (Exception e)   
            {
                throw new Exception(e.Message);
            }

        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody]Post post)
        {
            try
            {
                if (post != null)
                {
                    post.Id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    await _postRepo.Create(post).ConfigureAwait(false);
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody]Post post)
        {
            try
            {
                if (post != null)
                {
                    await _postRepo.Update(post).ConfigureAwait(false);
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}