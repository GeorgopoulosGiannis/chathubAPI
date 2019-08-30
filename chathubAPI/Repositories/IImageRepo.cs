using chathubAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Repositories
{
    public interface IImageRepo 
    {
        Image Get(int imageId);
        List<Image> GetAllByProfile(string profileId);
        bool Add(string path, string profileId);
        bool HardDelete(Image image);
        void Save();
    }
}
