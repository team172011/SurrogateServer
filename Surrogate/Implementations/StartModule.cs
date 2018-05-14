namespace Surrogate.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using Surrogate.Modules;
    using Surrogate.View;

    class StartModule : Module<ModulProperties, EmptyModuleInfo>
    {
        public StartModule() : base(new ModulProperties("Start","Startbildschirm",false,false,false,false))
        {
            
        }

        public override ContentControl GetPage()
        {
            return new StartModuleView();
        }

        public override void Start(EmptyModuleInfo info)
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            
        }
    }
}
