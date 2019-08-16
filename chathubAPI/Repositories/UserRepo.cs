using chathubAPI.DATA;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Repositories
{

   
    public class UserRepo : IUserRepo
    {
        readonly ApplicationDbContext _dbContext;
        readonly UserManager<IdentityUser> _userManager;
        public UserRepo(ApplicationDbContext dbContext,UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public List<string> GetAllUsersEmails()
        {
            List<string> listOfEmails = _userManager.Users.Select(e => e.Email).ToList();

            return listOfEmails;
        }

        public string GetUserIdFromEmail(string email)
        {
            return _userManager.Users.Where(s => s.Email == email).Select(i => i.Id).FirstOrDefault();
        }

        public string GetUserEmailFromId(string userId)
        {
            return _userManager.Users.Where(s => s.Id == userId).Select(e=>e.Email).FirstOrDefault();
        }
    }
}
