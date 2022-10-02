﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApp.Application.UseCases
{
    public interface IUseCase
    {
        public int Id { get; }
        public string Name { get; }
        public string Description { get;}
        public bool AdminOnly { get;  }
    }
}
