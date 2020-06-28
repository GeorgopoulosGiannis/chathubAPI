using chathubAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Repositories
{
    public interface ICommentRepo
    {
        string Add(string content, string profileId);
        Comment Update(Comment comment, string updatedById);
        Comment FindById(int commentId);
        bool HardDelete(Comment comment);
        void Save();
    }
}
