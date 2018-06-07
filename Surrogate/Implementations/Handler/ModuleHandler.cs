using System;
using System.Collections.Generic;
using System.Windows.Controls;
using Surrogat.Handler;
using Surrogate.Model;
using Surrogate.Modules;
using System.Linq;
using Surrogate.View;
using Surrogate.Controller;

namespace Surrogate.Implementations.Handler
{
    public class ModuleHandler : IModuleHandler
    {
        public event EventHandler<ModuleArgs> ModuleAdded;
        public event EventHandler<ModuleArgs> ModuleRemoved;

        private readonly IDictionary<int, IModule> modules = new Dictionary<int, IModule>();
        
        public int AddModule(IModule module)
        {
            var key = module.GetHashCode();
            modules.Add(key, module);
            ModuleAdded?.Invoke(this, new ModuleArgs(module, key));
            return key;
        }

        public UserControl SelectView<C>(int i) 
        {
            IVisualModule module = ((IVisualModule)modules[i]);
            module.OnSelected();
            return module.GetPage();
        }

        public void RemoveModule(IModule module)
        {
            modules.Remove(module.GetHashCode());
            ModuleRemoved?.Invoke(this, new ModuleArgs(module, module.GetHashCode()));
        }

        public Control SelectView(IVisualModule module)
        {
            module.OnSelected();
            return module.GetPage(); // TODO the hash value approach is too simple...
        }

        public IList<IModule> GetModules()
        {
            return modules.Select(d => d.Value).ToList();
        }
    }
}