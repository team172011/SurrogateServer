// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

namespace Surrogate.Implementations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using Surrogate.Modules;
    using Surrogate.View;

    class StartModule : VisualModule<ModuleProperties, ModuleInfo>
    {
        private readonly StartModuleView _view = new StartModuleView();

        public StartModule() : base(new ModuleProperties("Start","Startbildschirm",false,false,false,false))
        {
            
        }

        public override ContentControl GetPage()
        {
            return _view;
        }

        public override void OnDisselected()
        {
        }

        public override void OnSelected()
        {
        }

        public override void Start(ModuleInfo info)
        {
            throw new NotImplementedException();
        }

        public override void Stop()
        {
            
        }
    }
}
