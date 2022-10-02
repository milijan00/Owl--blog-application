using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Dto
{
    public class CommentDto : Dto
    {
        public int? UserId { get; set; }
        public int? PostId { get; set; }
        public int? ParentId { get; set; }
        public string Content { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string ProfilePicture { get; set; }
        public IEnumerable<object> Likes { get; set; }
        public IEnumerable<CommentDto> ChildComments { get; set; }
    }
}
