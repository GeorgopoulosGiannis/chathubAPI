using chathubAPI.DATA;
using chathubAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Repositories
{
    public class ImageRepo : IImageRepo
    {
        readonly ApplicationDbContext _dbContext;
        public ImageRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Image Get(string imageId)
        {
            return _dbContext.Images.Where(x => x.Id == imageId).FirstOrDefault();
        }

        // TODO add paging
        public List<Image> GetAllByProfile(string profileId)
        {

            return _dbContext.Images.Where(x => x.ProfileId == profileId).ToList();
        }

        public bool Add(string path, string profileId)
        {
            if (path != null && profileId != null)
            {
                try
                {
                    Image img = new Image
                    {
                        Path = path,
                        CreatedById = profileId,
                        DateCreated = DateTime.UtcNow,
                        ProfileId = profileId,
                        DateUpdated = DateTime.UtcNow,
                        UpdatedById = profileId
                    };
                    _dbContext.Images.AddAsync(img);
                    _dbContext.SaveChangesAsync();

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

        public bool HardDelete(Image image)
        {
            if (image != null)
            {
                try
                {
                    _dbContext.Images.Remove(image);
                    _dbContext.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            return false;

        }

        public void Save()
        {
            _dbContext.SaveChangesAsync();
        }
    }
}
