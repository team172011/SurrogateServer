// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using Surrogate.Controller;
using Surrogate.Implementations;
using Surrogate.Implementations.Controller;
using Surrogate.Modules;
using System.Windows;
using System.Windows.Controls;

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

    /// <summary>
    /// WPF/XAML Wrapper. Normally the XYView.cs should extend ModuleView<XYModule> with the generic class parameter XYModule
    /// that describes the Controller class (maybe XYModule<XYModulInfo, XYModulProperties>
    /// But partial classes do not support generic class arguments and we have to specifie a general controller class in this base class
    /// </summary>
    public class ModuleViewBase : ModuleView<AbstractController>
    {
        public ModuleViewBase()
        {

        }

        public ModuleViewBase(AbstractController controller=null) : base(controller)
        {

        }
    }

    public abstract class ModuleView<C> : UserControl, IView<C> where C : AbstractController 
    {
        private readonly C _controller;
        public C Controller { get => _controller; }
        public virtual log4net.ILog Logger { get => _controller.Logger; }

        public ModuleView(C controller =null)
        {
            _controller = controller;
        }

        public ModuleView() { }

    }

    public interface IView<C> where C : AbstractController
    {
        C Controller { get; }
    }
}
