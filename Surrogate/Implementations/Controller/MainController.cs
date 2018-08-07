// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using Surrogat.Handler;
using Surrogate.Controller;
using Surrogate.Implementations.Handler;
using Surrogate.Model;
using Surrogate.Model.Handler;
using Surrogate.View.ControllerView;

namespace Surrogate.Implementations.Controller
{

    /// <summary>
    /// Class representing the frameworks main controller
    /// </summary>
    public class MainController : AbstractController, IMainController
    {
        public MainControllerView MainWindow { get; }
        public IControllerHandler ModulHandler { get; } = new ModuleHandler();
        public IConnectionHandler ConnectionHandler { get; } = new ConnectionsHandler();
        public IProcessHandler ProcessHandler { get; } = new ProcessHandler();

        public override IModuleProperties Properties { get; } = new ModulePropertiesBase("MainController", "MainController for internal use");

        public MainController()
        {
            MainWindow = new MainControllerView(this);
            MainWindow.connectionsPanel.Children.Add(ConnectionHandler.GetPage());
        }

        public override bool IsRunning()
        {
            return true;
        }

        public override void Start()
        {
            ProcessHandler.StartAllProcesses();
        }

        public override string ToString()
        {
            return "Main Controller";
        }
    }
}
