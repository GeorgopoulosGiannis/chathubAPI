using chathubAPI.DATA;
using chathubAPI.INTERFACES;
using chathubAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Repositories
{
    public class FcmTokenRepo : IFcmTokenRepo
    {
        readonly ApplicationDbContext _dbContext;

        public FcmTokenRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FcmToken> Read(string token)
        {
            try
            {
                return await _dbContext.FcmTokens.Where(t => t.Token == token).FirstOrDefaultAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task Create(string token, string userId)
        {
            try
            {
                await _dbContext.FcmTokens.AddAsync(new FcmToken
                {
                    Token = token,
                    TokenOwner = userId
                });
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task Update(string token, string userId)
        {
            try
            {
                FcmToken fcmToken = await _dbContext.FcmTokens.FirstOrDefaultAsync(t => t.Token == token);
                if (fcmToken != null)
                {
                    fcmToken.TokenOwner = userId;
                    _dbContext.FcmTokens.Update(fcmToken);
                    await _dbContext.SaveChangesAsync();
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public async Task<List<FcmToken>> GetUserTokens(string userId)
        {
            try
            {
                List<FcmToken> fcmTokens = await _dbContext.FcmTokens.Where(x => x.TokenOwner == userId).ToListAsync();
                return fcmTokens;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public async Task Delete(FcmToken fcmToken)
        {
            try
            {
                _dbContext.FcmTokens.Remove(fcmToken);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
