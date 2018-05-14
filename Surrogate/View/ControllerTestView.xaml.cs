using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Surrogate.Implementations;
using Surrogate.Modules;
using Surrogate.Roboter.MController;

namespace Surrogate.View
{
    /// <summary>
    /// View Component of the ControllerTestModule
    /// </summary>
    public partial class ControllerTestView
    {

        private ControllerTestModule parentModule;

        public ControllerTestView(ControllerTestModule parentModule)
        {
            this.parentModule = parentModule;
            InitializeComponent();
        }

        public void startTest(object sender, RoutedEventArgs e)
        {
            parentModule.Start(new ControllerTestInfo(true));
        }

        public void startMotorTest(object sender, RoutedEventArgs e)
        {
            parentModule.Start(new ControllerTestInfo(false));
        }

        public void stopTest(object sender, RoutedEventArgs e)
        {
            parentModule.Stop();
        }
    }
}
