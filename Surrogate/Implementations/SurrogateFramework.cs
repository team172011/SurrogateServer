using Surrogat.Handler;
using Surrogate.Implementations.Controller;
using Surrogate.Model;
using Surrogate.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.Implementations
{
    public class SurrogateFramework
    {
        private static readonly MainController _controller = new MainController();
        public MainController MainController { get; }
        public static IMainController GetMainController()
        {
            return _controller;
        }

        public static void AddModule(IModule module)
        {
            _controller.ModulHandler.AddModule(module);

            // if module is connection, register the connection
            if (module is IConnection connection)
            {
                _controller.ConnectionHandler.RegisterConnection(connection);
            }
            
        }
    }
}
