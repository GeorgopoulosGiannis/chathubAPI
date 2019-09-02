using chathubAPI.DATA;
using chathubAPI.Models;
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

        public bool CommentImage(int commentId,int imageId)
        {
           
            
                try
                {
                    ImageComment imageComment = new ImageComment
                    {
                        CommentId = commentId,
                        ImageId = imageId,
                    };
                    _dbContext.ImageComments.AddAsync(imageComment);
                    _dbContext.SaveChangesAsync();
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

        public List<ImageComment> GetAllImageComments(int imageId)
        {
            try
            {
                List<ImageComment> imageComments = _dbContext.ImageComments.Where(x => x.ImageId == imageId).ToList();
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
