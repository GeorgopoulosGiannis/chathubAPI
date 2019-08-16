using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Repositories
{
    public interface IUserRepo
    {
        List<string> GetAllUsersEmails();
        string GetUserIdFromEmail(string email);
        string GetUserEmailFromId(string userId);
    }
}
