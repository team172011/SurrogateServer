// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Surrogate.Model;

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
        public abstract IModuleProperties Properties { get; }

        /// <summary>
        /// Field indicating if this AbstractController object should stop runnig the current
        /// function
        /// </summary>
        protected volatile bool ShouldStop;

        public AbstractController Parent { get; }

        public AbstractController(AbstractController parent = null)
        {
            Parent = parent;
        }

        public abstract bool IsRunning();


        public virtual void Log(string message)
        {
            log.Info(message);
        }

        public abstract void Start();

        public virtual void Stop()
        {
            ShouldStop = true;
        }

        /// <summary>
        /// Implementing classes have to override the ToString() function
        /// </summary>
        /// <returns></returns>
        public new abstract string ToString();
    }
}
