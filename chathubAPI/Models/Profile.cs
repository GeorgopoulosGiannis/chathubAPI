using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Models
{
    public class Profile
    {
        [Required]
        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
        public string Alias { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
    }
}
