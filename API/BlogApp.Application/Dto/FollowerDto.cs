using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Dto
{
    public class FollowerDto
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string ProfilePicture { get; set; }
        public string Username { get; set;}
    }

    public class FollowersFollowingDto
    {
        public IEnumerable<FollowerDto> Followers { get; set; }
        public IEnumerable<FollowerDto> Following { get; set; }

    }
}
