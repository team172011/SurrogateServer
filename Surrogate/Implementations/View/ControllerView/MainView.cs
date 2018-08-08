// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using Surrogate.Controller;
using Surrogate.Implementations;
using Surrogate.Implementations.Controller;
using System.Windows;

namespace Surrogate.View
{
    public abstract class MainView : Window, IView<MainController>
    {

        protected readonly MainController _controller;
        public MainController Controller { get => _controller; }
        public virtual log4net.ILog Logger { get => _controller.Logger; }

        MainController IView<MainController>.Controller => SurrogateFramework.MainController;

        public MainView(MainController controller)
        {
            _controller = controller;
        }

    }
}
