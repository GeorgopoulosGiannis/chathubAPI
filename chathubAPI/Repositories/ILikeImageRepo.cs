using chathubAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Repositories
{
    public interface ILikeImageRepo
    {
        bool LikeImage(int imageId, string userId);

        bool DeleteLikeImage(LikedImage likedImage);

        LikedImage CheckIfLiked(int imageId, string userId);
        int CountImageLikes(int imageId);
        void Save();
    }
}
