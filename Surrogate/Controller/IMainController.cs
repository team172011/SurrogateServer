using Surrogat.Handler;
using Surrogate.Model.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Surrogate.Controller
{
    public interface IMainController : IController
    {

        Window MainWindow { get; }
        IControllerHandler ModulHandler { get; }
        IConnectionHandler ConnectionHandler { get; }
        IProcessHandler ProcessHandler { get; }


    }
}
