using chathubAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Repositories
{
    public interface IImageCommentRepo
    {
        bool CommentImage(string commentId, string imageId);

        bool DeleteImageComment(ImageComment imageComment);
        List<ImageComment> GetAllImageComments(string imageId);

        void Save();
    }
}
