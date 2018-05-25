

using System;
using System.Windows.Controls;
using Surrogat.Handler;
using Surrogate.Model;
using Surrogate.Model.Module;
using Surrogate.Modules;
using Surrogate.View;
using Surrogate.View.ConnectionsChecker;

namespace Surrogate.Implementations.Handler
{
    public class ConnectionsHandler : VisualModule<ModuleProperties, ModuleInfo>, IConnectionHandler
    {
        public event EventHandler<ConnectionArgs> ConnectionEstablished;
        public event EventHandler<ConnectionArgs> ConnectionReady;
        public event EventHandler<ConnectionArgs> ConnectionClosed;

        private readonly ConnectionsCheckerView _view = new ConnectionsCheckerView();

        public ConnectionsHandler(ModuleProperties modulProperties) : base(modulProperties)
        {

        }

        public ConnectionsHandler():this(new ModuleProperties("Verbindungsmanager", "API zum managen verschiedener Verbindungen"))
        {
        }

        public override ModuleView GetPage()
        {
            return _view;
        }

        public override void Log(string message)
        {
            base.Log(message);
        }

        public override void OnDisselected()
        {
            base.OnDisselected();
        }

        public override void OnSelected()
        {
            base.OnSelected();
        }

        public override void Start(ModuleInfo info)
        {
            throw new System.NotImplementedException();
        }

        public override void Stop()
        {
            throw new System.NotImplementedException();
        }

        public void RegisterConnection(IConnection connection)
        {
            throw new NotImplementedException();
        }
    }
}