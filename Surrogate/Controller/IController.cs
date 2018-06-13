using Surrogate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.Controller
{
    public interface IController
    {
        IModuleProperties Properties { get; }
        void Log(string message);

        /// <summary>
        /// Returns true if the IController is executing a/the function
        /// </summary>
        /// <returns></returns>
        bool IsRunning();

        void Start();
        void Stop();

        string ToString();
    }
}
