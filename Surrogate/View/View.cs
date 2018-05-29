using Surrogate.Controller;
using Surrogate.Implementations;
using Surrogate.Modules;
using System.Windows;
using System.Windows.Controls;

namespace Surrogate.View
{
    public abstract class MainView : Window, IView
    {

        protected readonly AbstractController _controller;
        public AbstractController Controller { get => _controller; }
        public virtual log4net.ILog Logger { get => _controller.Logger; }

        public MainView(AbstractController controller)
        {
            _controller = controller;
        }

        public MainView() => _controller = SurrogateFramework.MainController;
    }

    public abstract class ModuleView : UserControl, IView
    {
        private readonly AbstractController _controller;
        public AbstractController Controller { get => _controller; }
        public virtual log4net.ILog Logger { get => _controller.Logger; }

        public ModuleView(AbstractController controller =null)
        {
            _controller = controller;
        }

        public ModuleView() { }

    }

    public interface IView
    {
        AbstractController Controller { get; }
    }
}
