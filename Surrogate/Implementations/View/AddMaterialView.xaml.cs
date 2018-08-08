// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using Surrogate.Implementations.Controller.Module;
using System;
using System.Windows;
using System.Windows.Documents;

namespace Surrogate.View.ControllerView.ModuleView
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class AddMaterialView : Window
    {

        public event EventHandler<Material> AddedHandler;

        public AddMaterialView()
        {
            InitializeComponent();
        }


        public void Added(object sender, RoutedEventArgs e)
        {
            Material m;
            try
            {
                m = new Material(int.MaxValue, tbName.Text, tbDescription.Text, tbunit.Text, long.Parse(tbStock.Text), long.Parse(tbMinStock.Text));
                AddedHandler?.Invoke(this, m);
                Close();
            } catch(Exception ex)
            {
                MessageBox.Show("Bitte Eingaben überprüfen", "Fehlerhafte Eingabe");
            }
            
        }
    }
}
