﻿using Surrogate.Implementations.Controller.Module;
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
    /// Interaction logic for LineFollowingModuleView.xaml
    /// </summary>
    public partial class LineFollowingModuleView : ModuleViewBase
    {
        public LineFollowingModuleView(LineFollowingModule controller) : base(controller)
        {
            InitializeComponent();
            controller.ModuleSelected += (e, s) =>
            {
                btnStart.Click += OnStartPressed;
                btnStart.Click -= OnStopPressed;
            };
        }


        private void OnStartPressed(object sender, RoutedEventArgs e)
        {
            var controller = Controller as LineFollowingModule;
            IDictionary<int, Image> images = new Dictionary<int, Image>
            {
                /*{ 0,imView0},
                { 1,imView1},
                { 2,imView2},
                { 3,imView3},
                { 4,imView4},*/
                { 5,imView5},
            };
            controller.Start(new LineFollowingInfo(images));
            btnStart.Content = "Line folgen beenden";
            btnStart.Click -= OnStartPressed;
            btnStart.Click += OnStopPressed;
        }

        private void OnStopPressed(object sender, RoutedEventArgs e)
        {
            Controller.Stop();
            btnStart.Click += OnStartPressed;
            btnStart.Click -= OnStopPressed;
            btnStart.Content = "Linie folgen starten";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Controller.Stop();
            Window window = new Window();
            HsvPickerView picker = new HsvPickerView(window);
            LineFollowingModule controller = Controller as LineFollowingModule;
            picker.SaveClicked += controller.UpdateHsvSpace;
            window.Content = picker;
            window.Show();
        }
    }


}