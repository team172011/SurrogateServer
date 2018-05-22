// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

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
        /// <summary>
        /// Static logger instance to log on gui, console or write log to file
        /// </summary>
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// List of all available modules
        /// </summary>
        private static ObservableCollection<IModule> _modules = new ObservableCollection<IModule>();

        /// <summary>
        /// Module that is selected and shown in gui
        /// </summary>
        private IModule _currentModule;

        public ObservableCollection<IModule> Modules { get => _modules; }


        public MainWindow()
        {
            InitializeComponent();
            InitLocal();
            log.Info("GUI Initiaization finished");
        }

        private void InitLocal()
        {
            lvModules.ItemsSource = _modules;
            StartModule startModule = new StartModule();
            _modules.Add(startModule);
            _modules.Add(new ControllerTestModule(new ModulProperties("ControllerTest", "Modul zum testen eines Controllers", false, false, false, false)));
            _modules.Add(new MotorTestModule(new ModulProperties("MotorTest", "Modul zum testen des angeschlossenen Motors", true, false, false, false)));
            _modules.Add(new VideoChatModule(new VideoChatProperties()));
            SelectModule(startModule);
        }

        private void SelectModule(IModule m)
        {
            if(_currentModule != m)
            {
                if(_currentModule != null)
                {
                    _currentModule.OnDisselected();
                }
                spModule.Children.Clear();
                m.OnSelected();
                spModule.Children.Add(m.GetPage());
                _currentModule = m;
            }
            
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
                log.Info("Loading Module: " + module.GetDescription());
                SelectModule(module);
            } catch(Exception ex)
            {
                log.Error("Fehler beim Laden eines Moduls:"+ex.Message+"\n "+ex.StackTrace);
            }
            
        }
    }
}
