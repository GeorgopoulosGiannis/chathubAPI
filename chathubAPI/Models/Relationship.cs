using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chathubAPI.Models
{
    public class Relationship
    {
        public string User_OneId { get; set; }
        public  User User_One { get; set; }
        public string User_TwoId { get; set; }
        public User User_Two { get; set; }


        /* 
          Relationship statuses =
          0 = Pending
          1 = Accepted
          2 = Declined
          3 = Blocked
                              */
        public int Status { get; set; }

        public string Action_UserId { get; set; }
        public User Action_User { get; set; }
    }
}
