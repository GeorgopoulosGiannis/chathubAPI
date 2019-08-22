using chathubAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Repositories
{
    public interface IRelationshipRepo
    {
        List<Relationship> GetRelationships(string userId,int status);
        bool Add(Relationship rel);
        bool UpdateStatus(Relationship rel);
        bool Save();
    }
}
