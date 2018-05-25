// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

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
    public partial class VideoChatView : ModuleView
    {
        private readonly VideoChatModule _parentModule;
        

        public VideoChatView(VideoChatModule parentModule)
        {
            _parentModule = parentModule;
            InitializeComponent();
            btnStartCall.Click += _handleCall;


        }

        private void _handleCall(Object sender, RoutedEventArgs e)
        {
            _parentModule.Start(new VideoChatInfo()); // TODO add contact details in VideoChatInfo
        }

    }
}
