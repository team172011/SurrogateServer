// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using Surrogate.Model;
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

namespace Surrogate.View.PropertieView
{
    /// <summary>
    /// Interaction logic for PropertiesView.xaml
    /// </summary>
    public partial class PropertiesView : Window
    {
        public string Header { get => _properties.GetProperty(_properties.KeyName, "Moduleinstellungen"); }
        public IDictionary<string, string> AlllProperties { get => _properties.GetAllProperties(); }
        private IModuleProperties Properties { get => _properties; }
        private readonly IModuleProperties _properties;

        public PropertiesView(IModuleProperties properties)
        {
            _properties = properties;
            InitializeComponent();
            lbHeader.Content = Header;
            lvSettings.ItemsSource = AlllProperties;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _properties.Save();
        }
    }
}
