using Surrogate.Implementations;
using Surrogate.Implementations.Controller.Module;
using Surrogate.Implementations.Model;
using Surrogate.Model.Module;
using Surrogate.View.ControllerView.ModuleView;
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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class BallFollowingView : ModuleViewBase
    {
        public BallFollowingView(BallFollowingModule controller):base(controller)
        {
            InitializeComponent();
        }

        private void OnStartPressed(object sender, RoutedEventArgs e)
        {
            ((BallFollowingModule)Controller).Start(new BallFollowingInfo(imView));
            btnStart.Content = "Ball folgen beenden";
            btnStart.Click -= OnStartPressed;
            btnStart.Click += OnStopPressed;
        }

        private void OnStopPressed(object sender, RoutedEventArgs e)
        {
            Controller.Stop();
            btnStart.Click += OnStartPressed;
            btnStart.Click -= OnStopPressed;
            btnStart.Content = "Ball folgen starten";
        }

        private void OnHSVViewPressed(object sender, RoutedEventArgs e)
        {
            Controller.Stop();
            Window window = new Window();
            var props = Controller.Properties as IHsvProperties;
            HsvPickerView picker = new HsvPickerView(window, props: props);
            BallFollowingModule controller = Controller as BallFollowingModule;
            picker.SaveClicked += controller.UpdateHsvSpace;
            window.Content = picker;
            window.Show();
        }

        private void BtnChangeCam_Click(object sender, RoutedEventArgs e)
        {
            Controller.Stop();
            var props = Controller.Properties as BallFollowingProperties;
            props.CamNum = ++props.CamNum % FrameworkConstants.Numbercams;
            ((BallFollowingModule)Controller).Start(new BallFollowingInfo(imView));
            btnStart.Content = "Line folgen beenden";
            btnStart.Click -= OnStartPressed;
            btnStart.Click += OnStopPressed;
        }
    }
}
