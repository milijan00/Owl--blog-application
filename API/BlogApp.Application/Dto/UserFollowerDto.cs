using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Dto
{
    public class UserFollowerDto
    {
        public int UserId { get; set; }
        public int FollowerId { get; set; }
    }
}
