using Surrogate.Implementations;
using Surrogate.Modules;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Surrogate.Main
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// partial class will be combined with xaml file at runtime
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // List of all available modules
        private static ObservableCollection<IModule> _modules = new ObservableCollection<IModule>();

        public ObservableCollection<IModule> Modules { get => _modules; }
        public MainWindow()
        {
            InitializeComponent();
            initLocal();
            log.Info("GUI Initiaization finished");
        }

        private void initLocal()
        {
            lvModules.ItemsSource = _modules;
            StartModule startModule = new StartModule();
            _modules.Add(startModule);
            _modules.Add(new ControllerTestModule(new ModulProperties("ControllerTest", "Modul zum testen eines Controllers", false, false, false, false)));
            _modules.Add(new MotorTestModule(new ModulProperties("MotorTest", "Modul zum testen des angeschlossenen Motors", true, false, false, false)));
            _modules.Add(new VideoChatModule(new VideoChatProperties()));
            selectModule(startModule);
        }

        private void selectModule(IModule m)
        {
            spModule.Children.Clear();
            spModule.Children.Add(m.GetPage());
            
        }

        /// <summary>
        /// This function will be called if the user selects a new module from the lvModules
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            IModule module = (IModule) item.Content;

            try
            {
                log.Info("Loading Module: " + module.ToString());
                selectModule(module);
            } catch(Exception ex)
            {
                log.Error("Fehler beim Laden eines Moduls:"+ex.Message);
            }
            
        }
    }
}
