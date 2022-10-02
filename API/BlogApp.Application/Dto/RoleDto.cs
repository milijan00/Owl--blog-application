using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Dto
{
    public class RoleDto : Dto
    {
        public string Name { get; set; }
    }

    public class RoleWithUsecasesDto : RoleDto
    {
        public IEnumerable<RoleUsecaseDto> Usecases { get; set; }
    }
}
