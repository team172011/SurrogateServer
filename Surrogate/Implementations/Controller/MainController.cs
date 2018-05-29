using Surrogat.Handler;
using Surrogate.Controller;
using Surrogate.Implementations.Handler;
using Surrogate.Main;
using Surrogate.Model.Handler;
using Surrogate.Modules;
using Surrogate.View.ControllerView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Surrogate.Implementations.Controller
{
    

    public class MainController : AbstractController, IMainController
    {
        private readonly IModuleHandler _modulHandler= new ModuleHandler();
        private readonly IConnectionHandler _connectionHandler = new ConnectionsHandler();
        private readonly IProcessHandler _processHandler;

        private readonly MainControllerView _view;
        public Window MainWindow => _view;
        public IModuleHandler ModulHandler => _modulHandler;
        public IConnectionHandler ConnectionHandler => _connectionHandler;
        public IProcessHandler ProcessHandler => _processHandler;

        public MainController()
        {
            _view = new MainControllerView(this);
            _view.connectionsPanel.Children.Add(_connectionHandler.GetPage());
        }



    }
}
