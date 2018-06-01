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
    class BallFollowingModule : VisualModule<ModuleProperties, ModuleInfo>
    {
        public BallFollowingModule(ModuleProperties modulProperties) : base(modulProperties)
        {
        }

        public override ModuleView GetPage()
        {
            return null;
        }

        public override void OnDisselected()
        {
            base.OnDisselected();
        }

        public override void OnSelected()
        {
            base.OnSelected();
        }

        public override void Start(ModuleInfo info)
        {
            
        }

        public override void Stop()
        {
            base.Stop();
        }
    }
}
