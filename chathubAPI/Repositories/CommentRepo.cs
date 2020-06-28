using chathubAPI.DATA;
using chathubAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Repositories
{
    public class CommentRepo : ICommentRepo
    {
        readonly ApplicationDbContext _dbContext;

        public CommentRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string Add(string content, string profileId)
        {

            try
            {
                Comment comment = new Comment
                {
                    Content = content,
                    CreatedById = profileId,
                    DateCreated = DateTime.UtcNow,
                    DateUpdated = DateTime.UtcNow,
                    ProfileId = profileId,
                    UpdatedById = profileId
                };
                _dbContext.Comments.Add(comment);
                _dbContext.SaveChanges();
                return comment.Id;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    

    public Comment Update(Comment comment, string updatedById)
    {
        try
        {
            Comment comm = _dbContext.Comments.Find(comment.Id);
            comm.DateUpdated = DateTime.UtcNow;
            comm.UpdatedById = updatedById;
            comm.Content = comment.Content;
            comm.DateCreated = comment.DateCreated;
            comm.CreatedById = comment.CreatedById;
            comm.ProfileId = comment.ProfileId;
            _dbContext.Comments.Update(comm);
            _dbContext.SaveChangesAsync();
            return comm;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }

    public Comment FindById(int commentId)
    {
        try
        {
            return _dbContext.Comments.Find(commentId);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    public bool HardDelete(Comment comment)
    {
        if (comment != null)
        {
            try
            {
                _dbContext.Comments.Remove(comment);
                _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        else
        {
            return false;
        }
    }


    public void Save()
    {
        _dbContext.SaveChangesAsync();
    }
}
}
