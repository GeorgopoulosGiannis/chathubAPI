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
            if (rel.User_OneId != rel.User_TwoId)
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
            else
            {
                return false;
            }


        }
        public List<Relationship> GetRelationships(string userId, int status)
        {
            try
            {
                List<Relationship> rels = new List<Relationship>();
                if (status == 0)
                {
                    return _dbContext.Relationships.Where(x => x.Status == status && (x.User_OneId == userId || x.User_TwoId == userId) && x.Action_UserId != userId).ToList();
                }

                return _dbContext.Relationships.Where(x => x.Status == status && (x.User_OneId == userId || x.User_TwoId == userId)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public List<Relationship> GetRelationshipsAllStatus(string userId)
        {
            try
            {
                return _dbContext.Relationships.Where(x => (x.User_OneId == userId || x.User_TwoId == userId)).ToList();
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
                if (relationship == null)
                {
                    relationship = _dbContext.Relationships.Where(x => x.User_OneId == rel.User_TwoId && x.User_TwoId == rel.User_OneId).FirstOrDefault();
                }
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
