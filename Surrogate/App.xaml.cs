// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using System;
using System.Windows;
using Surrogate.Parameters;
using Surrogate.Implementations;
using Surrogate.Implementations.Controller;
using Surrogate.Controller;
using Surrogate.Roboter.MController;
using Surrogate.Roboter.MInternet;
using Surrogate.Implementations.Controller.Module;
using Surrogate.Implementations.FaceDetection;
using Surrogate.Roboter.MCamera;
using Surrogate.Implementations.Processes;
using Surrogate.Roboter.MMotor;
using Surrogate.Roboter.MDatabase;
using Surrogate.Model;

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
        /// <summary>
        /// Application_Startup function called from xaml part
        /// </summary>
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Init();
            if (e.Args.Length >= 1)
            {
                log.Info("Command line arg found:");
                foreach(var i in e.Args)
                {
                    log.Info("--"+i);
                }
                
            }

            FrameworkElement.StyleProperty.OverrideMetadata(typeof(Window), new FrameworkPropertyMetadata(TryFindResource(typeof(Window))));
            SystemDatabase database = new SystemDatabase();
            SurrogateFramework.AddConnection(Motor.Instance);
            SurrogateFramework.AddConnection(XBoxController.Instance);
            SurrogateFramework.AddConnection(database);
            SurrogateFramework.AddConnection(new Internet());
            SurrogateFramework.AddConnection(new Camera0());
            SurrogateFramework.AddConnection(new Camera1());
            SurrogateFramework.AddConnection(new Camera2());

            SurrogateFramework.AddModule(new StartModule());
            SurrogateFramework.AddModule(new ControllerTestModule());
            SurrogateFramework.AddModule(new MotorTestModule());
            SurrogateFramework.AddModule(new VideoChatModule());
            SurrogateFramework.AddModule(new BallFollowingModule());
            SurrogateFramework.AddModule(new FaceDetectionModule());
            SurrogateFramework.AddModule(new LineFollowingModule());
            SurrogateFramework.AddModule(new InformationsModule(database));

            SurrogateFramework.AddProcess(new ControllerProcess(Motor.Instance, XBoxController.Instance), FrameworkConstants.ControllerProcessName);
            SurrogateFramework.AddProcess(new EmergencyStopProcess(Motor.Instance, XBoxController.Instance));

            SurrogateFramework.Start();
        }

        /// <summary>
        /// General initializations
        /// </summary>
        private void Init()
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
            /*
            foreach(var m in m.Modules)
            {
                m.Stop();
            }
            */
        }
    }
}
