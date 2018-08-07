// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using Surrogate.Controller;
using Surrogate.Model;
using Surrogate.Modules;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Surrogat.Handler
{
	public interface IControllerHandler
    {
        event EventHandler<ModuleArgs> ModuleAdded;
        event EventHandler<ModuleArgs> ModuleRemoved;

        int AddModule(Surrogate.Controller.IController module);
        void RemoveModule(Surrogate.Controller.IController module);
        IList<Surrogate.Controller.IController> GetModules();

        Control SelectView(IVisualModule module);
    }


    public class ModuleArgs : EventArgs
    {
        private readonly Surrogate.Controller.IController _module;
        public IController Module { get => _module; }
        private readonly int _key;
        public int Key { get => _key; }
        

        public ModuleArgs(Surrogate.Controller.IController arg, int key)
        {
            _module = arg;
            _key = key;
        }
    }
}
