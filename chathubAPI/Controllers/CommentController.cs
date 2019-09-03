using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
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
    public class CommentController : ControllerBase
    {

        readonly IImageCommentRepo _imageCommentRepo;
        readonly ICommentRepo _commentRepo;

        public CommentController(IImageCommentRepo imageCommentRepo, ICommentRepo commentRepo)
        {
            _imageCommentRepo = imageCommentRepo;
            _commentRepo = commentRepo;

        }

        [HttpPost("comment")]
        public async Task<IActionResult> Add([FromBody]CommentImageDTO commentImageDTO)
        {
            if (ModelState.IsValid)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                try
                {
                    int commentId = _commentRepo.Add(commentImageDTO.Content, userId);
                    _commentRepo.Save();
                    if ( commentId != 0)
                    {
                        _imageCommentRepo.CommentImage(commentId, commentImageDTO.ImageId);
                        return Ok();
                    }
                    return NoContent();
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
        
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody]int commentId)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    if (_commentRepo.HardDelete(_commentRepo.FindById(commentId)))
                    {
                        _commentRepo.Save();
                        return Ok();
                    }
                    return NoContent();
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody]UpdateCommentDTO updateCommentDTO)
        {
            if (ModelState.IsValid)
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                try
                {
                    Comment comm = _commentRepo.FindById(updateCommentDTO.CommentId);
                    comm.Content = updateCommentDTO.Content;
                    _commentRepo.Update(comm, userId);
                    _commentRepo.Save();
                    return Ok();
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else
            {
                return BadRequest();
            }
        }
    }
}