// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using Surrogate.Controller;
using System.Windows.Controls;

namespace Surrogate.View
{

    public abstract class ModuleView<C> : UserControl, IView<C> where C : AbstractController
    {
        private readonly C _controller;
        public C Controller { get => _controller; }
        public virtual log4net.ILog Logger { get => _controller.Logger; }

        public ModuleView(C controller = null)
        {
            _controller = controller;
        }

        public ModuleView() { }

    }
}
