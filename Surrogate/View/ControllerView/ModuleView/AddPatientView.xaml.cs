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
            AddedHandler?.Invoke(this, new Patient(int.MaxValue, tbName.Text, tbfirstName.Text, dpBirthday.SelectedDate, tbAdress.Text, dpEntry.SelectedDate, int.Parse(tbHistory.Text)));
        }
    }
}
