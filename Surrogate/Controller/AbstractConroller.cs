using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.Controller
{
    /// <summary>
    /// Class representing a Controller rule in the MVC concept
    /// </summary>
    public abstract class AbstractController : IController
    {
        /// <summary>
        /// Static reference to a logger. Enables logging to console, gui (TextBox) and file
        /// </summary>
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public virtual log4net.ILog Logger { get => log; }


        protected volatile bool ShouldStop;

        private AbstractController _parent;
        //private IList<Controller> Children;

        public AbstractController(AbstractController parent = null)
        {

        }

        public virtual void Log(string message)
        {
            log.Info(message);
        }

        public virtual void Stop()
        {
            ShouldStop = true;
        }
    }
}
