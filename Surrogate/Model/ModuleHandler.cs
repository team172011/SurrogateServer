

using Surrogate.Model;
using Surrogate.Modules;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Surrogat.Handler
{
	public interface IModuleHandler
    {
        event EventHandler<ModuleArgs> ModuleAdded;
        event EventHandler<ModuleArgs> ModuleRemoved;

        int AddModule(IModule module);
        void RemoveModule(IModule module);
        IList<IModule> GetModules();

        Control GetView(IVisualModule module);
    }

	public interface IConnectionHandler
    {
        event EventHandler<ConnectionArgs> ConnectionEstablished;
        event EventHandler<ConnectionArgs> ConnectionReady;
        event EventHandler<ConnectionArgs> ConnectionClosed;

        void RegisterConnection(IConnection connection);
    }

	public interface IProcessHandler
    {
        void AddProcess();
        void RemoveProcess();
    }

    public class ModuleArgs : EventArgs
    {
        private readonly IModule _module;
        public IModule Module { get => _module; }
        private readonly int _key;
        public int Key { get => _key; }
        

        public ModuleArgs(IModule arg, int key)
        {
            _module = arg;
            _key = key;
        }
    }

    public class ConnectionArgs : EventArgs
    {
        private readonly IConnection _connection;
        public IConnection Module { get => _connection; }

        public ConnectionArgs(IConnection arg)
        {
            _connection = arg;
        }
    }
}
