using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using chathubAPI.DTO;
using chathubAPI.Models;
using chathubAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chathubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageCommentController : ControllerBase
    {
        readonly IImageCommentRepo _imageCommentRepo;
        readonly ICommentRepo _commentRepo;

        public ImageCommentController(IImageCommentRepo imageCommentRepo, ICommentRepo commentRepo)
        {
            _imageCommentRepo = imageCommentRepo;
            _commentRepo = commentRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int imageId)
        {
            try
            {
                List<ImageComment> imageComments = _imageCommentRepo.GetAllImageComments(imageId);
                List<CommentDTO> commentsDTO = new List<CommentDTO>();

                foreach (var imageComment in imageComments)
                {
                    CommentDTO commentDTO = new CommentDTO
                    {
                        CommentByEmail = imageComment.Comment.Profile.User.Email,
                        CommentByAlias = imageComment.Comment.Profile.Alias,
                        CommentId = imageComment.CommentId,
                        Content = imageComment.Comment.Content
                    };
                    commentsDTO.Add(commentDTO);
                }
                if(commentsDTO.Count < 1)
                {
                    return NoContent();
                }
                return Ok(commentsDTO);
         
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}