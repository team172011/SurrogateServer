using Surrogate.Controller;
using Surrogate.Model;
using Surrogate.Model.Module;
using Surrogate.Modules;
using Surrogate.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.Implementations.Controller.Module
{
    class LineFollowingModule : VisualModule<ModuleProperties, ModuleInfo>
    {
        public LineFollowingModule(ModuleProperties modulProperties) : base(modulProperties)
        {
        }

        public override ModuleView GetPage()
        {
            throw new NotImplementedException();
        }

        public override void Start(ModuleInfo info)
        {
            throw new NotImplementedException();
        }
    }
}
