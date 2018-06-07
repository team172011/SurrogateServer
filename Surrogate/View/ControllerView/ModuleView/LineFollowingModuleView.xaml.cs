using Surrogate.Implementations.Controller.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Surrogate.View
{
    /// <summary>
    /// Interaction logic for LineFollowingModuleView.xaml
    /// </summary>
    public partial class LineFollowingModuleView : ModuleViewBase
    {
        public LineFollowingModuleView(LineFollowingModule controller) : base(controller)
        {
            InitializeComponent();
        }


        private void OnStartPressed(object sender, RoutedEventArgs e)
        {
            var controller = Controller as LineFollowingModule;
            controller.Start(new LineFollowingInfo(imView));
        }

        private void OnStopPressed(object sender, RoutedEventArgs e)
        {
            var controller = Controller as LineFollowingModule;
            controller.Stop();
        }
    }


}
