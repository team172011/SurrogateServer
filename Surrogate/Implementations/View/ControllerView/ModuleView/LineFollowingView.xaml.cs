// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using Surrogate.Implementations.Controller.Module;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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

        private IDictionary<int, Image> CreateViewInfo()
        {
            return new Dictionary<int, Image>
            {
                { 0, imView0 },
                { 1, imView1 },
                { 2, imView2 },
                { 3, imView3 },
                { 4, imView4 },
                { 5, imView5 },
                { 6, imView6 },
                { 7, imView7 },
                { 8, imView8 },
                { 9, imView9 },
            };
        }

        private void OnStartPressed(object sender, RoutedEventArgs e)
        {
            ((LineFollowingModule)Controller).Start(new LineFollowingInfo(CreateViewInfo()));
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

        private void OnHSVViewPressed(object sender, RoutedEventArgs e)
        {
            Controller.Stop();
            Window window = new Window();
            var props = Controller.Properties as LineFollowingProperties;
            HsvPickerView picker = new HsvPickerView(window, props:props);
            LineFollowingModule controller = Controller as LineFollowingModule;
            picker.SaveClicked += controller.UpdateHsvSpace;
            window.Content = picker;
            window.Show();
        }

        private void BtnChangeCam_Click(object sender, RoutedEventArgs e)
        {
            var controller = Controller as LineFollowingModule;
            controller.ChangeCamera();
        }
    }


}
