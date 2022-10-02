﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Domain
{
    public class UseCase : Entity
    {
        public string Name { get; set; }
        public  ICollection<RoleUseCase> Roles = new List<RoleUseCase>();
    }
}
