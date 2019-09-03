using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using chathubAPI.DATA;
using chathubAPI.DTO;
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
    public class ImageController : ControllerBase
    {
        readonly IUserRepo _userRepo;
        readonly IImageRepo _imageRepo;
        readonly ILikeImageRepo _likeImageRepo;
        public ImageController(IImageRepo imageRepo, IUserRepo userRepo, ILikeImageRepo likeImageRepo)
        {
            _imageRepo = imageRepo;
            _userRepo = userRepo;
            _likeImageRepo = likeImageRepo;
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetProfileImages(string email)
        {
            if (email != null)
            {
                string userId = _userRepo.GetUserIdFromEmail(email);
                List<Image> images = _imageRepo.GetAllByProfile(userId);
                List<ImageWithIdDTO> imageDTOs = new List<ImageWithIdDTO>();
                foreach (var image in images)
                {
                    ImageWithIdDTO img = new ImageWithIdDTO
                    {
                        Id = image.Id,
                        Path = image.Path
                    };
                    imageDTOs.Add(img);
                }
                return Ok(imageDTOs);
            }
            else
            {
                return NoContent();
            }

        }

        [HttpGet("id")]
        public async Task<IActionResult> GetById(int id)
        {
            Image image = _imageRepo.Get(id);
            int imageLikes = _likeImageRepo.CountImageLikes(image.Id);
            try
            {
                ImageWithEntitiesDTO imageDTO = new ImageWithEntitiesDTO
                {
                    Id = image.Id,
                    Path = image.Path,
                    LikesCount = imageLikes
                };

                return Ok(imageDTO);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add([FromBody]ImageDTO imageDTO)
        {

            if (ModelState.IsValid)
            {
                string userId = _userRepo.GetUserIdFromEmail(imageDTO.Email);

                if (_imageRepo.Add(imageDTO.Path, userId))
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