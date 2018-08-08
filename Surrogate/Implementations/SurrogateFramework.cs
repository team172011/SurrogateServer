// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using Surrogate.Controller;
using Surrogate.Implementations.Controller;
using Surrogate.Model;
using System;
using System.ComponentModel;
using System.Windows.Controls;

namespace Surrogate.Implementations
{
    /// <summary>
    /// Static class. Representing the Surrogate Framework API.
    /// </summary>
    public static class SurrogateFramework
    {
        public static MainController MainController { get; } = new MainController();

        public static void AddModule(IController module)
        {
            MainController.ModulHandler.AddModule(module);

            // if module is connection, register the connection also
            if (module is IConnection connection)
            {
                AddConnection(connection);
            }
        }

        public static void AddProcess(BackgroundWorker process, string name = FrameworkConstants.Empty)
        {
            if(name == FrameworkConstants.Empty)
            {
                MainController.ProcessHandler.AddProcess(process);
            }
            else
            {
                MainController.ProcessHandler.AddProcess(name, process);
            }
            
        }

        public static void AddConnection(IConnection connection, string name = FrameworkConstants.Empty)
        {
            if(name == FrameworkConstants.Empty)
            {
                MainController.ConnectionHandler.RegisterConnection(connection.Name, connection);
            }
            else
            {
                MainController.ConnectionHandler.RegisterConnection(name, connection);
            }
            
        }

        /// <summary>
        /// Start all processes and show the main screen
        /// </summary>
        public static void Start()
        {
            MainController.ProcessHandler.StartAllProcesses();
            MainController.MainWindow.SelectDynamically(0);
            MainController.MainWindow.Show();
        }
    }

    public static class FrameworkConstants
    { 
        public const string Empty = "";

        public static readonly string MotorName = "Motor";
        public static readonly string DatabaseName = "Datenbank";
        public static readonly string InternetName = "Internet";
        public static readonly string ControllerName = "Controller";
        public static readonly string TouchpadName = "Touchpad";
        public static readonly string Camera1Name = "Kamera 1";
        public static readonly string Camera2Name = "Kamera 2";

        public static readonly string ControllerProcessName = "ControllerProcess";

        public static readonly string DefaultImagePath = System.IO.Directory.GetCurrentDirectory()+"/Resources/robot.jpg";

        public static readonly string InternalCameraName = "Interne Kamera";
        public static readonly int InternalCameraId = 0;

        public static readonly int Numbercams = 3;

        public static readonly string DbmsConnectionString = @"server=localhost\SQLEXPRESS;database=Surrogate;Integrated Security=True;";
    }
}
