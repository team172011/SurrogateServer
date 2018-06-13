// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

using Surrogate.Controller;
using Surrogate.Model;
using Surrogate.Model.Module;
using Surrogate.View;
using System;
using System.Windows.Controls;

namespace Surrogate.Modules
{

    /// <summary>
    /// A Module extending class represents a single functionallity that can be executed from/for the robot
    /// and that has a visual componente e.g. <see cref="ContentControl"/>
    /// Extends the <see cref="AbstractController"/> class, according to this it takes over a "controller" role in the MVC pattern.
    /// </summary>
    /// <typeparam name="P">Generic class parameter extending <see cref="MofulProperties"/></typeparam>
    /// <typeparam name="I">Generic class parameter extending <see cref="MofulProperties"/></typeparam>
    public abstract class VisualModule<P, I> : AbstractController, IVisualModule where P : IModuleProperties where I : ModuleInfo
    {

        /// <summary>
        /// Event hanlder triggered if the module has been selected
        /// </summary>
        public event EventHandler ModuleSelected;
        public event EventHandler ModuleDisselected;

        /// <summary>
        /// Field for the properties of a module
        /// </summary>
        private readonly P _properties;

        /// <summary>
        /// Returns the specific Properties implementation
        /// </summary>
        /// <returns></returns>
        public P GetProperties()
        {
            return _properties;
        }

        /// <summary>
        /// Constructor. Expects a ModulProperties instance
        /// </summary>
        /// <param name="modulProperties"></param>
        public VisualModule(P modulProperties)
        {
            _properties = modulProperties;
        }

        public override void Start()
        {
            Type infoType = typeof(I);
            var info = Activator.CreateInstance(infoType);
            Start((I)info);
        }

        /// <summary>
        /// Starts the functionality 
        /// </summary>
        /// <param name="info">Informations that can be necessary for starting the module</param>
        public abstract void Start(I info);


        public abstract UserControl GetPage();

        public virtual void OnSelected()
        {
            ModuleSelected?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnDisselected()
        {
            ModuleDisselected?.Invoke(this, EventArgs.Empty);
        }

        public override string ToString()
        {
            return GetProperties().GetProperty(Properties.KeyName,"Ohne Name");
        }
    }
}
