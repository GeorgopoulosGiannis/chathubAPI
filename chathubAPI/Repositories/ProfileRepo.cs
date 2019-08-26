using chathubAPI.DATA;
using chathubAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Repositories
{
    public class ProfileRepo : IProfileRepo
    {
        readonly ApplicationDbContext _dbContext;
        public ProfileRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public bool Add(string userId)
        {
            Profile profile = new Profile { UserId = userId }; 
            try
            {

                _dbContext.AddAsync(profile);
                _dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Profile Get(string userId)
        {
            try
            {
              return _dbContext.Profiles.Where(x => x.UserId == userId).FirstOrDefault();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool Update(Profile profile)
        {
            try
            {
                Profile oldProf = _dbContext.Profiles.Where(x => x.UserId == profile.UserId).FirstOrDefault();
                oldProf.Alias = profile.Alias;
                oldProf.Avatar = profile.Avatar;
                oldProf.Description = profile.Description;
                _dbContext.Update(oldProf);
                _dbContext.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
