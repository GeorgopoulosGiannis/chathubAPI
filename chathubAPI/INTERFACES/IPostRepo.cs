
using chathubAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace chathubAPI.INTERFACES
{
    public interface IPostRepo
    {
        Task Create(Post post);
        Task<Post> Read(string postId);
        Task Update(Post newPost);
        Task Delete(Post post);
        Task<IList<Post>> ReadAll(string userId);
    }
}
