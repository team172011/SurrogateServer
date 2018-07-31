using Surrogate.Implementations.Controller.Module;
using Surrogate.Model;
using Surrogate.View.ControllerView.ModuleView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Surrogate.View
{
    /// <summary>
    /// Interaction logic for InformationsModuleView.xaml
    /// </summary>
    public partial class InformationsModuleView : ModuleViewBase
    {
        public InformationsModuleView(InformationsModule controller):base(controller)
        {
            InitializeComponent();
            lvMaterials.LoadingRow += new EventHandler<DataGridRowEventArgs>(DataGrid_LoadingRow);
            FillPatientTable();
            FillMaterialsTable();
        }

        /// <summary>
        /// Sets the color of each materials row depending of stock and minStock paramters
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {

            Material item = e.Row.Item as Material;

            if(item != null)
            {
                if(item.MinStock >= item.Stock)
                {
                    e.Row.Background = new SolidColorBrush(Colors.Red);
                }
                
            }
        }

        private ObservableCollection<Material> mats;
        private void FillMaterialsTable()
        {
            mats?.Clear();
            mats = ((InformationsModule)Controller).GetMaterialRows();
            lvMaterials.ItemsSource = mats;
        }

        private ObservableCollection<Patient> rows;
        private void FillPatientTable()
        {
            rows?.Clear();
            rows = ((InformationsModule)Controller).GetPatientRows();
            lvPatientes.ItemsSource = rows;
            
        }

        private void SavePatients(object sender, RoutedEventArgs e)
        {
            ((InformationsModule)Controller).SavePatients(rows);
            FillPatientTable();
        }
        
        private void AddPatient(object sender, RoutedEventArgs e)
        {
            AddPatientView addPatient = new AddPatientView();
            addPatient.AddedHandler += (s, p) =>rows.Add(p);
            addPatient.Show();
        }

        private void SaveMaterials(object sende, RoutedEventArgs e)
        {
            ((InformationsModule)Controller).SaveMaterials(mats);
            FillMaterialsTable();
        }

        private ObservableCollection<History> currentHistories;
        private void LvPatientes_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            long id = (lvPatientes.SelectedItem as Patient).CaseId;
            currentHistories = ((InformationsModule)Controller).GetHistoryRows(id);
            if(currentHistories.Count <= 0)
            {
                Label noContent = new Label();
                noContent.Content = "Keine Patienteneintraege verfügbar";
                spCurrentContent.Children.Clear();
                spCurrentContent.Children.Add(noContent);
            }
            else
            {
                DataGrid grid = new DataGrid();
                Label name = new Label();
                grid.ItemsSource = currentHistories;
                name.Content = (lvPatientes.SelectedItem as Patient).Name + ", " + (lvPatientes.SelectedItem as Patient).Firstname;
                name.HorizontalAlignment = HorizontalAlignment.Center;

                spCurrentContent.Children.Clear();
                spCurrentContent.Children.Add(name);
                spCurrentContent.Children.Add(grid);
            }
        }
    }

}
