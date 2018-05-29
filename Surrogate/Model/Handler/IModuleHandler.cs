

using Surrogate.Controller;
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
}
