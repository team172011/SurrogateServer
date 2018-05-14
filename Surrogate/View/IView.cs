using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.View
{
    using Surrogate.Modules;

    interface IView<P, I> where P : ModulProperties where I : ModuleInfo
    {
        Module<P,I> GetModule();    
    }
}
