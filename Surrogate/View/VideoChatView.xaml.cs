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
    using Surrogate.Implementations;
    using Surrogate.Modules;
    /// <summary>
    /// Interaktionslogik für VideoChatView.xaml
    /// </summary>
    public partial class VideoChatView : UserControl, IView<VideoChatProperties, VideoChatInfo>
    {
        private readonly VideoChatModule _parentModule;
        

        public VideoChatView(VideoChatModule parentModule)
        {
            _parentModule = parentModule;
            InitializeComponent();
            IsVisibleChanged += new DependencyPropertyChangedEventHandler(StartModule);
            
        }

        public Module<VideoChatProperties, VideoChatInfo> GetModule()
        {
            return _parentModule;
        }

        private void StartModule(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(IsVisible && !_parentModule.IsRunning)
            {
                _parentModule.Start(new VideoChatInfo());
            }
            else if (!IsVisible && _parentModule.IsRunning)
            {
                _parentModule.Stop();
            }
            {

            }
                
        }
    }
}
