using chathubAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Repositories
{
    public interface ILikeImageRepo
    {
        bool LikeImage(string imageId, string userId);

        bool DeleteLikeImage(LikedImage likedImage);

        LikedImage CheckIfLiked(string imageId, string userId);
        int CountImageLikes(string imageId);
        void Save();
    }
}
