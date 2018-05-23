using Surrogate.Implementations.Controller;
using Surrogate.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.Implementations
{
    public static class SurrogateFramework
    {

        public static IMainController GetMainController()
        {
            return new MainController();
        }
    }
}
