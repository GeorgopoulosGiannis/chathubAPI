using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chathubAPI.DATA;
using chathubAPI.DTO;
using chathubAPI.Models;
using chathubAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chathubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        readonly IUserRepo _userRepo;
        readonly IImageRepo _imageRepo;
        public ImageController(IImageRepo imageRepo, IUserRepo userRepo)
        {
            _imageRepo = imageRepo;
            _userRepo = userRepo;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfileImages(string email)
        {
            if (email != null)
            {
                string userId = _userRepo.GetUserIdFromEmail(email);
                List<Image> images = _imageRepo.GetAllByProfile(userId);
                return Ok(images);
            }
            else
            {
                return NoContent();
            }

        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody]ImageDTO imageDTO)
        {
            
            if (ModelState.IsValid)
            {
                if (_imageRepo.Add(imageDTO.Path, imageDTO.Email))
                {
                    _imageRepo.Save();
                    return Ok();
                }
            }
            return BadRequest();
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody]int imageId)
        {
            if (_imageRepo.HardDelete(_imageRepo.Get(imageId)))
            {
                _imageRepo.Save();
                return Ok();
            }
            return BadRequest();
        }

        
    }
}