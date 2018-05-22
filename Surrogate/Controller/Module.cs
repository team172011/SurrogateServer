// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Surrogate.Modules
{
    public class ModulProperties
    {
        public ModulProperties(string name, string description, bool motor = false, bool faceCam = false, bool floorCam = false, bool microphone = false, bool database = false, bool internet = false)
        {
            _name = name;
            _description = description;
            _motor = motor;
            _faceCam = faceCam;
            _floorCam = floorCam;
            _microphone= microphone;
            _database = database;
            _imagePath = @"C:\Users\ITM1\source\repos\Surrogate\Surrogate\resources\robot.jpg";
            _internet = false;
        }

        /// permissions
        private readonly bool _motor;
        private readonly bool _faceCam;
        private readonly bool _floorCam;
        private readonly bool _database;
        private readonly bool _microphone;
        private readonly bool _internet;

        /// descriptions
        private readonly string _name;
        private readonly string _description;

        /// Image
        private string _imagePath;


        public bool Motor { get => _motor; }
        public bool FaceCam { get => _faceCam; }
        public bool FloorCam { get => FloorCam; }
        public bool Database { get => Database; }
        public string Name { get => _name; }
        public string Description { get => _description; }
        public string ImagePath { get => _imagePath; set => _imagePath = value; }
    }

    public class ModuleInfo
    {
        static readonly ModuleInfo EmptyModuleInfo;
    }

    /// <summary>
    /// An Module extending class represents a single functionallity that can be executed from/for the robot
    /// </summary>
    /// <typeparam name="P">Generic class parameter extending <see cref="MofulProperties"/></typeparam>
    /// <typeparam name="I">Generic class parameter extending <see cref="MofulProperties"/></typeparam>
    public abstract class Module<P, I> : IModule where P : ModulProperties where I : ModuleInfo
    {
        /// <summary>
        /// Static reference to a logger. Enables logging to console, gui (TextBox) and file
        /// </summary>
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Event hanlder triggered if the module has been selected
        /// </summary>
        protected event EventHandler ModuleSelected;
        protected event EventHandler ModuleDisselected;

        /// <summary>
        /// Field for the properties of a module
        /// </summary>
        private readonly P _properties;

        public P Properties { get => _properties; }

        /// <summary>
        /// Constructor. Expects a ModulProperties instance
        /// </summary>
        /// <param name="modulProperties"></param>
        public Module(P modulProperties)
        {
            _properties = modulProperties;
        }

        public void Start()
        {
            Start(null);
        }

        /// <summary>
        /// Starts the functionality 
        /// </summary>
        /// <param name="info">Informations that can be necessary for starting the module</param>
        public abstract void Start(I info);


        public abstract ContentControl GetPage();

        public string GetDescription()
        {
            return _properties.Name + ": " + _properties.Description;
        }

        public abstract void Stop();

        public virtual void OnSelected()
        {
            ModuleSelected?.Invoke(this, EventArgs.Empty);
        }

        public virtual void OnDisselected()
        {
            ModuleDisselected?.Invoke(this, EventArgs.Empty);
        }

        public virtual void Log(string message)
        {
            log.Info(message);
        }
    }

    public interface IModule
    {
        /// <summary>
        /// Starts the functionality
        /// </summary>
        void Start();

        /// <summary>
        /// Calling this function should stop all current activities run by a IModule
        /// </summary>
        void Stop();

        /// <summary>
        /// Called if the module is selected (e.g. user clicked on the entry).
        /// Can be used to initialize and (re-)load content
        /// </summary>
        void OnSelected();

        /// <summary>
        /// Called if the module is deselected (e.g. user clicked on another entry of the module list).
        /// Can be used to store content or close connections etc.
        /// </summary>
        void OnDisselected();

        /// <summary>
        /// Possibility to get the view element of the IModule
        /// </summary>
        /// <returns>a ContentControl object that can be added to center of the mainWindow</returns>
        ContentControl GetPage();

        string GetDescription();

        /// <summary>
        /// Logs a message to the console, gui or file, depending on the
        /// logger configuration
        /// </summary>
        /// <param name="message"></param>
        void Log(string message);
    }
}
