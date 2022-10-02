using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Dto
{
    public class UseCaseDto : Dto
    {
        public string Name { get; set; }
    }

    public class UseCaseWithRolesDto : UseCaseDto
    {
        public IEnumerable<RoleDto> Roles { get; set; }
    }
}
