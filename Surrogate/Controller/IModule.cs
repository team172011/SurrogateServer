using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.Controller
{
    public interface IModule
    {
        /// <summary>
        /// Starts the functionality
        /// </summary>
        void Start();

        /// <summary>
        /// Calling this function should stop all current activities run by a IModule
        /// </summary>
        void Stop();

        /// <summary>
        /// 
        /// </summary>
        /// <returns>Description of this Module in a human friendly string</returns>
        string GetDescription();
    }
}
