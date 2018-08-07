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
    public partial class AddPatientView : Window
    {

        public event EventHandler<Patient> AddedHandler;

        public AddPatientView()
        {
            InitializeComponent();
        }


        public void Added(object sender, RoutedEventArgs e)
        {
            AddedHandler?.Invoke(this, new Patient(int.MaxValue, tbName.Text, tbfirstName.Text, dpBirthday.SelectedDate, tbAdress.Text, dpEntry.SelectedDate));
        }
    }
}
