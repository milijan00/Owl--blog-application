using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.Dto
{
    public class UpdateUseCasesDto
    {
        public int RoleId { get; set; }
        public IEnumerable<int> UseCasesIds { get; set; }
    }
}
