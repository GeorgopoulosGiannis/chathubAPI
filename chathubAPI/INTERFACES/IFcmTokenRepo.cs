using chathubAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.INTERFACES
{
    public interface IFcmTokenRepo
    {
        Task Create(string token, string userId);
        Task<FcmToken> Read(string token);
        Task Update(string token, string userId);
        Task Delete(FcmToken fcmToken);
        Task<List<FcmToken>> GetUserTokens(string userId);

    }
}
