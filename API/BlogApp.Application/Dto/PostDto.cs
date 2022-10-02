using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Dto
{
    public class PostDto : Dto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public int? UserId { get; set; }
        public int? CategoryId { get; set; }

    }
    public class UsersPostDto : PostDto
    {
        public int NumberOfLikes { get; set; }
        public int NumberOfComments { get; set; }
        public string Category { get; set; }
        public string CreatedAt { get; set; }
        public string ProfilePicture { get; set; }
        public string Username { get; set; }
    }
}
