
using chathubAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace chathubAPI.INTERFACES
{
    interface IPostRepo
    {
        Task Create(Post post);
        Task<Post> Read(int postId);
        Task Update(Post newPost);
        Task Delete(Post post);
        Task<IList<Post>> ReadAll(string userId);
    }
}
