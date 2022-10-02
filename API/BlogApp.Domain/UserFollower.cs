using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Domain
{
    public class UserFollower
    {
        public User User { get; set; }
        public int UserId { get; set; }
        public int FollowerId { get; set; }
        public User Follower { get; set; }
    }
}
