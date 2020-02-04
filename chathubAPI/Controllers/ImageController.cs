using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using chathubAPI.DATA;
using chathubAPI.DTO;
using chathubAPI.Models;
using chathubAPI.Repositories;
using Microsoft.AspNetCore.Hosting;
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
        readonly ILikeImageRepo _likeImageRepo;
        readonly IHostingEnvironment _env;
        public ImageController(IImageRepo imageRepo, IUserRepo userRepo, ILikeImageRepo likeImageRepo, IHostingEnvironment env)
        {
            _env = env;
            _imageRepo = imageRepo;
            _userRepo = userRepo;
            _likeImageRepo = likeImageRepo;
        }

        [HttpGet("profile")]
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
        [HttpGet("image")]
        public async Task<IActionResult> Get(string imagePath)
        {
            string path = Path.Combine(_env.ContentRootPath, imagePath);
            FileStream image = System.IO.File.OpenRead(path);

            return File(image, "image/jpeg");
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
        public async Task<IActionResult> Add([FromBody]UploadImageDTO imageDTO)
        {

            if (ModelState.IsValid)
            {
                string userId = _userRepo.GetUserIdFromEmail(imageDTO.UploaderEmail);

                string fName = imageDTO.FileName;
                string relativeDirectoryPath = "Images/" + userId;
                string fullDirectoryPath = Path.Combine(_env.ContentRootPath, relativeDirectoryPath);
                Directory.CreateDirectory(fullDirectoryPath);
                byte[] byteArray = Convert.FromBase64String(imageDTO.DataBase64);

                using (var stream = new FileStream(fullDirectoryPath + "/" + imageDTO.FileName, FileMode.Create, FileAccess.Write))
                {
                    if (stream.CanWrite)
                    {
                        await stream.WriteAsync(byteArray, 0, byteArray.Length);
                        _imageRepo.Add(relativeDirectoryPath + "/" + imageDTO.FileName, userId); ;
                        return Ok(relativeDirectoryPath + "/" + imageDTO.FileName);
                    }
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