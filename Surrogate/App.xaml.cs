// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

using Surrogate.Modules;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Surrogate.Parameters;
using Surrogate.Utils;

namespace Surrogate.Main
{
    /// <summary>
    /// Interaktionslogik für "App.xaml" declarative starting point of application  
    /// .NET will go to this class for starting instructions and then start the desired Window or Page from there.
    /// </summary>
    public partial class App : Application
    {
        private static readonly log4net.ILog log = 
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        MainWindow mainWindow;
        /// <summary>
        /// Application_Startup function called from xaml part
        /// </summary>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            init();
            if (e.Args.Length >= 1)
            {
                log.Info("Command line arg found:");
                foreach(var i in e.Args)
                {
                    log.Info("--"+i);
                }
                
            }
            
            mainWindow = new MainWindow();
            mainWindow.Show();
        }

        /// <summary>
        /// General initializations
        /// </summary>
        private void init()
        {
            log4net.GlobalContext.Properties["LogFileName"] = String.Format(Dirs.programDir+"{0}",@"\\surrogate");
            // check if neccesary files and folder exists
            try
            {
                System.IO.Directory.CreateDirectory(Dirs.programDir);
                System.IO.Directory.CreateDirectory(Dirs.propertiesDir);
            } catch (System.IO.IOException ioe)
            {
                log.Error("Could not create DIRs: " + ioe.Message);
            }
            
        }

        /// <summary>
        /// This function handles all unhandled exceptions of the Application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Ein unerwarteter Fehler ist aufgetreten: "+e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            foreach(var m in mainWindow.Modules)
            {
                m.Stop();
            }
        }
    }
}
