// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using Surrogat.Handler;
using Surrogate.Model.Handler;
using Surrogate.View.ControllerView;

namespace Surrogate.Controller
{
    /// <summary>
    /// Interface specification for a MainController
    /// </summary>
    public interface IMainController : IController
    {
        /// <summary>
        /// Property holding the main windows that will be shown after start up
        /// </summary>
        MainControllerView MainWindow { get; }

        /// <summary>
        /// Property for a module handler
        /// </summary>
        IControllerHandler ModulHandler { get; }

        /// <summary>
        /// Property for a connection handler
        /// </summary>
        IConnectionHandler ConnectionHandler { get; }

        /// <summary>
        /// Property for a process handler
        /// </summary>
        IProcessHandler ProcessHandler { get; }


    }
}
