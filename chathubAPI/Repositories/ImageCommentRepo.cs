using chathubAPI.DATA;
using chathubAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Repositories
{
    public class ImageCommentRepo : IImageCommentRepo
    {
        readonly ApplicationDbContext _dbContext;

        public ImageCommentRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool CommentImage(string commentId,string imageId)
        {
           
            
                try
                {
                    ImageComment imageComment = new ImageComment
                    {
                        CommentId = commentId,
                        ImageId = imageId,
                    };
                    _dbContext.ImageComments.Add(imageComment);
                    _dbContext.SaveChanges();
                    return true;
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            
           
        }

        public bool DeleteImageComment(ImageComment imageComment)
        {
            try
            {
                _dbContext.ImageComments.Remove(imageComment);
                _dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<ImageComment> GetAllImageComments(string imageId)
        {
            try
            {
                List<ImageComment> imageComments = _dbContext.ImageComments.Where(x => x.ImageId == imageId)
                                                                            .Include(x=>x.Comment)
                                                                            .ThenInclude(x=>x.Profile)
                                                                            .ThenInclude(x=>x.User)
                                                                            .ToList();
                return imageComments;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Save()
        {
            try
            {
                _dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
            
      
    }
}
