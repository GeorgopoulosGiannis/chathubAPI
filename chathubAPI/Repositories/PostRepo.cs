
using chathubAPI.DATA;
using chathubAPI.INTERFACES;
using chathubAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Repositories
{
    public class PostRepo : IPostRepo
    {
        readonly ApplicationDbContext _dbContext;
        public PostRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task Create(Post post)
        {
            try
            {
                await _dbContext.Posts.AddAsync(post).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }
        public async Task Update(Post post)
        {
            try
            {
                _dbContext.Posts.Update(post);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            
            
        }
        public async Task Delete(Post post)
        {
            try
            {
                _dbContext.Posts.Remove(post);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public async Task<Post> Read(int postId)
        {
            try
            {
                return await _dbContext.Posts.Where(p => p.Id == postId).FirstOrDefaultAsync().ConfigureAwait(false);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task<IList<Post>> ReadAll(string profileId)
        {
            try
            {
                return await _dbContext.Posts.Where(p => p.ProfileId == profileId).ToListAsync().ConfigureAwait(false);
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
