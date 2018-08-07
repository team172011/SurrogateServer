// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
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
        public ObservableCollection<IVisualModule> VisualModules { get; } = new ObservableCollection<IVisualModule>();

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
                    VisualModules.Add(module as IVisualModule);
                }
            }
        }

        private void OnModuleAdded(object sender, ModuleArgs mArgs)
        {
            if (mArgs.Module is IVisualModule visualModule)
            {
                VisualModules.Add(visualModule);
            }
            MenuItem item = new MenuItem();
            item.Header = mArgs.Module.ToString();
            item.Name = mArgs.Module.ToString().Replace(" ",String.Empty);
            item.Click += (o, s) => { new PropertiesView(mArgs.Module.Properties).Show(); };
            miModuleSettings.Items.Add(item);
        }

        private void OnModuleRemoved(object sender, ModuleArgs mArgs){
            VisualModules.Remove(mArgs as IVisualModule);
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
            lblModulTitle.Content = _selectedModule.GetTitle();
            spModule.Children.Add((_controller).ModulHandler.SelectView(module));
        }

        public void SelectDynamically(int index)
        {
            lvModules.SelectedIndex = index;
            IVisualModule module = VisualModules[0];
            spModule.Children.Clear();
            _selectedModule = module;
            lblModulTitle.Content = _selectedModule.GetTitle();
            spModule.Children.Add((_controller).ModulHandler.SelectView(module));
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

            Application.Current.Dispatcher.ShutdownFinished += new EventHandler((o, ea) => Application.Current.Shutdown());
            Application.Current.Dispatcher.BeginInvokeShutdown(System.Windows.Threading.DispatcherPriority.Send);
            
        }
    }
}
