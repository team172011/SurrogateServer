// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

using Surrogate.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Surrogate.View.ConnectionsChecker
{
    
    /// <summary>
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class ConnectionsCheckerView : UserControl
    {
        private ObservableCollection<IConnection> connections;

        public ConnectionsCheckerView()
        {
            InitializeComponent();
        }

    }
}
