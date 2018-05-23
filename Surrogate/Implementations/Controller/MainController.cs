using Surrogate.Main;
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
    

    public class MainController : IMainController
    {
        private readonly MainControllerView _view;

        public MainController()
        {
            _view = new MainControllerView();
        }


        public Window GetWindow()
        {
            return _view;
        }
    }
}
