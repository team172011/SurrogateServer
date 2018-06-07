

using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Surrogat.Handler;
using Surrogate.Model;
using Surrogate.Model.Handler;
using Surrogate.Model.Module;
using Surrogate.Modules;
using Surrogate.View;
using Surrogate.View.Handler;

namespace Surrogate.Implementations.Handler
{
    /// <summary>
    /// Class managing different connections
    /// <see cref="ConnectionsHandlerView"/>
    /// </summary>
    public class ConnectionsHandler : VisualModule<ModuleProperties, ModuleInfo>, IConnectionHandler
    {
        public event EventHandler<ConnectionArgs> ConnectionChangedStatus;
        public event EventHandler<ConnectionArgs> ConnectionAdded;

        private readonly ConnectionsHandlerView _view; 
        private readonly Dictionary<string, IConnection> _connections = new Dictionary<string, IConnection>();
        public Dictionary<string, IConnection> Connections { get => _connections; }


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="modulProperties"></param>
        public ConnectionsHandler(ModuleProperties modulProperties) : base(modulProperties)
        {
            _view = new ConnectionsHandlerView(this);
        }

        /// <summary>
        /// Empty Constructor.
        /// </summary>
        public ConnectionsHandler():this(new ModuleProperties("Verbindungsmanager", "API zum managen verschiedener Verbindungen"))
        {
            _view = new ConnectionsHandlerView(this);
        }

        public override UserControl GetPage()
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

        public void RegisterConnection(string name, IConnection connection)
        {
            _connections.Add(name, connection);
            connection.ConnectionStatusHandler += OnConnectionStatusChanged;
            ConnectionAdded?.Invoke(this, new ConnectionArgs(connection));
        }

        public void Connect(string name)
        {
            _connections[name].Connect();
        }

        public IConnection GetConnection(string name)
        {
            return _connections[name];
        }

        private void OnConnectionStatusChanged(object sender, ConnectionStatus e)
        {
            if(sender is IConnection connection)
            {
                ConnectionChangedStatus?.Invoke(this, new ConnectionArgs(connection));
            }
        }
    }
}