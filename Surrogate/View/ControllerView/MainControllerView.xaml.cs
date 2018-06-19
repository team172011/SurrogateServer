using Surrogat.Handler;
using Surrogate.Controller;
using Surrogate.Implementations;
using Surrogate.Implementations.Controller;
using Surrogate.Modules;
using Surrogate.View.PropertieView;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Surrogate.View.ControllerView
{
    public partial class MainControllerView : MainView
    {
        private readonly ObservableCollection<IVisualModule> _visualModules = new ObservableCollection<IVisualModule>();
        public ObservableCollection<IVisualModule> VisualModules { get => _visualModules; }

        private IVisualModule _selectedModule;

        public MainControllerView(MainController controller):base(controller)
        {
            controller.ModulHandler.ModuleAdded += new EventHandler<ModuleArgs>(OnModuleAdded);
            controller.ModulHandler.ModuleRemoved += new EventHandler<ModuleArgs>(OnModuleRemoved);
            InitializeComponent();
            Logger.Debug("GUI Initiaization finished");
            lvModules.ItemsSource = VisualModules;
            foreach (IController module in controller.ModulHandler.GetModules())
            {
                if(module is IVisualModule)
                {
                    _visualModules.Add(module as IVisualModule);
                }
            }
            
        }

        private void OnModuleAdded(object sender, ModuleArgs mArgs)
        {
            if (mArgs.Module is IVisualModule visualModule)
            {
                _visualModules.Add(visualModule);
            }
            MenuItem item = new MenuItem();
            item.Header = mArgs.Module.ToString();
            item.Name = mArgs.Module.ToString().Replace(" ",String.Empty);
            item.Click += (o, s) => { new PropertiesView(mArgs.Module.Properties).Show(); };
            miModuleSettings.Items.Add(item);
        }

        private void OnModuleRemoved(object sender, ModuleArgs mArgs){
            _visualModules.Remove(mArgs as IVisualModule);
            miModuleSettings.Items.Remove(mArgs.Module.ToString());
        }


        /// <summary>
        /// This function will be called if the user selects a new module from the lvModules
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _selectedModule?.OnDisselected();
            var item = sender as ListViewItem;
            IVisualModule module = item.Content as IVisualModule;
            spModule.Children.Clear();
            _selectedModule = module;
            spModule.Children.Add(((IMainController)_controller).ModulHandler.SelectView(module));
        }

        private void MiModuleSettings_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var item = e.Source as MenuItem;
            var name = item.Name;
            System.Diagnostics.Debug.WriteLine(name);
        }

        private void BtnHideLog_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(spLog.Visibility == System.Windows.Visibility.Visible)
            {
                spLog.Visibility = System.Windows.Visibility.Collapsed;
                btnHideLog.Content = 5;
            }
            else
            {
                spLog.Visibility = System.Windows.Visibility.Visible;
                btnHideLog.Content = 6;
            }
            
        }

        private void MenuItem_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            foreach( IController modul in SurrogateFramework.MainController.ModulHandler.GetModules())
            {
                modul.Stop();
            }

            Application.Current.Shutdown();
        }

        private void Onshutdown(object sender, ExitEventArgs e)
        {
            
        }
    }
}
