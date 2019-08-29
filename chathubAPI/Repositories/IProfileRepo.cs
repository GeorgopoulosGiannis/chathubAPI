using chathubAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Repositories
{
    public interface IProfileRepo
    {
        bool Add(User user);

        Profile Get(string userId);

        bool Update(Profile profile);

        List<Profile> GetRandomProfiles(string userId, int currentPage = 1);

        void Save();
    }
}
