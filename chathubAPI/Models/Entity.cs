using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Models
{
    public class Entity
    {
       public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string CreatedById { get; set; }
        public string UpdatedById { get; set; }
        public string ProfileId { get; set; }
        public Profile Profile { get; set; }

    }
}
