using chathubAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Repositories
{
    public interface IImageCommentRepo
    {
        bool CommentImage(int commentId, int imageId);

        bool DeleteImageComment(ImageComment imageComment);
        List<ImageComment> GetAllImageComments(int imageId);

        void Save();
    }
}
