using Surrogat.Handler;
using Surrogate.Implementations.Controller;
using Surrogate.Modules;
using System;
using System.Collections.ObjectModel;
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
            foreach (IModule module in controller.ModulHandler.GetModules())
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
        }

        private void OnModuleRemoved(object sender, ModuleArgs mArgs){
            _visualModules.Remove(mArgs as IVisualModule);
        }


        /// <summary>
        /// This function will be called if the user selects a new module from the lvModules
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
            var item = sender as ListViewItem;
            IVisualModule module = item.Content as IVisualModule;
            spModule.Children.Clear();
            spModule.Children.Add(((IMainController)_controller).ModulHandler.GetView(module));
        }
    }
}
