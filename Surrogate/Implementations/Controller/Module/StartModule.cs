// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

using System;
using System.Collections.Generic;
using System.Data;
using Surrogate.Model;
using Surrogate.Model.Module;
using Surrogate.Modules;
using Surrogate.Roboter.MDatabase;
using Surrogate.Roboter.MInternet;
using Surrogate.View;

namespace Surrogate.Implementations
{


    class StartModule : VisualModule<ModuleProperties, ModuleInfo>
    {
        private readonly StartModuleView _view = new StartModuleView();

        public StartModule() : base(new ModuleProperties("Start","Startbildschirm",false,false,false,false))
        {
            Database db = (Database) SurrogateFramework.MainController.ConnectionHandler.GetConnection(FrameworkConstants.DatabaseName);
            IDictionary<string, SqlDbType> columns = new Dictionary<string, SqlDbType>
            {
                { "ID", SqlDbType.Int },
                { "Number", SqlDbType.Int },
                { "Firstname", SqlDbType.Text  },
                { "Name", SqlDbType.Text }
            };
            db.CreateTable("Test", columns);
        }

        public override ModuleView GetPage()
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
