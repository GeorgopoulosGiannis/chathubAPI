using chathubAPI.DATA;
using chathubAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Repositories
{
    public class RelationshipRepo : IRelationshipRepo
    {
        readonly ApplicationDbContext _dbContext;
        public RelationshipRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool Add(Relationship rel)
        {
            try
            {
                _dbContext.Relationships.Add(rel);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public List<Relationship> GetRelationships(string userId,int status)
        {
            try
            {
              return  _dbContext.Relationships.Where(x => x.Status == status && (x.User_OneId == userId || x.User_TwoId == userId)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public bool UpdateStatus(Relationship rel)
        {
            try
            {
                var relationship = _dbContext.Relationships.Where(x => x.User_OneId == rel.User_OneId && x.User_TwoId == rel.User_TwoId).FirstOrDefault();
                relationship.Status = rel.Status;
                _dbContext.Relationships.Update(relationship);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        public bool Save()
        {
            try
            {
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
