﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.Model.Module
{
    public class ModuleInfo : EventArgs
    {
        public static readonly ModuleInfo EmptyModuleInfo = new ModuleInfo();
        public ModuleInfo() { }
    }
}
