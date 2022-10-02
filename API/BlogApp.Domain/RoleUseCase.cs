using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Domain
{
    public class RoleUseCase
    {
        public int RoleId { get; set; }
        public int UseCaseId { get; set; }
        public Role Role { get; set; }
        public UseCase UseCase { get; set; }
    }
}
