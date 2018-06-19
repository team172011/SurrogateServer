// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Controls;
using Surrogate.Model;
using Surrogate.Model.Module;
using Surrogate.Modules;
using Surrogate.Roboter.MDatabase;
using Surrogate.Roboter.MInternet;
using Surrogate.View;

namespace Surrogate.Implementations
{


    public class StartModule : VisualModule<ModulePropertiesBase, ModuleInfo>
    {
        private readonly StartModuleView _view = new StartModuleView();

        public StartModule() : base(new StartProperties())
        {

        }

        public override IModuleProperties Properties => GetProperties();

        public override UserControl GetPage()
        {
            return _view;
        }

        public override bool IsRunning()
        {
            throw new NotImplementedException();
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

    public class StartProperties : ModulePropertiesBase
    {
        public StartProperties() : base("Start", "Startbildschirm")
        {
            SetProperty(base.KeyImagePath, @"C:\Users\ITM1\source\repos\Surrogate\Surrogate\resources\start_controller_icon.png");
        }
    }
}
