using Surrogate.Modules;
using System.Windows;
using System.Windows.Controls;

namespace Surrogate.View
{
    public abstract class MainView : Window, IView
    {

        protected readonly Controller _controller;
        public Controller Controller { get => _controller; }
        public virtual log4net.ILog Logger { get => _controller.Logger; }

        public MainView(Controller controller)
        {
            _controller = controller;
        }
    }

    public abstract class ModuleView : UserControl, IView
    {
        private readonly Controller _controller;
        public Controller Controller { get => _controller; }
        public virtual log4net.ILog Logger { get => _controller.Logger; }

        public ModuleView(Controller controller=null)
        {
            _controller = controller;
        }

    }

    public interface IView
    {
        Controller Controller { get; }
    }
}
