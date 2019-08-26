using chathubAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Repositories
{
    public interface IProfileRepo
    {
        bool Add(string userId);

        Profile Get(string userId);

        bool Update(Profile profile);
    }
}
