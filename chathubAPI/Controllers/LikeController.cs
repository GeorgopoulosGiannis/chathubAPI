using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using chathubAPI.Models;
using chathubAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chathubAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LikeController : ControllerBase
    {
        readonly ILikeImageRepo _likeImageRepo;
        readonly IUserRepo _userRepo;
        public LikeController(ILikeImageRepo likeImageRepo, IUserRepo userRepo)
        {
            _likeImageRepo = likeImageRepo;
            _userRepo = userRepo;

        }

        [HttpPost("like")]
        public async Task<IActionResult> LikeImage([FromBody]string imageId)
        {


            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (_likeImageRepo.LikeImage(imageId, userId))
            {
                _likeImageRepo.Save();
                return Ok();
            }
            else
            {
                return BadRequest();
            }


        }

        [HttpPost("unlike")]
        public async Task<IActionResult> UnlikeImage([FromBody]string imageId)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            LikedImage img = _likeImageRepo.CheckIfLiked(imageId, userId);
            if (_likeImageRepo.DeleteLikeImage(img))
            {
                return Ok();
            }
            else
            {
                return NoContent();
            }

        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetImageLikes(string imageId)
        {
            int likes = _likeImageRepo.CountImageLikes(imageId);
            if(likes > 0)
            {
                return Ok(likes);
            }
            else
            {
                return NoContent();
            }
        }
    }
}