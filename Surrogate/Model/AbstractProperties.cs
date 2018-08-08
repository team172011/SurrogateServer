// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using System.Collections.Generic;
using System.IO;
using Surrogate.View.PropertieView;

namespace Surrogate.Model
{
    public abstract class AbstractProperties : IModuleProperties
    {
        public string KeyName => "Name";
        public string KeyDescription => "Description";
        public string KeyMotor => "Motor";
        public string KeyFaceCam => "FaceCam";
        public string KeyFloorCam => "FloorCam";
        public string KeyMicrophone => "Microphone";
        public string KeyDatabase => "Database";
        public string KeyImagePath => "ImagePath";
        public string KeyInternet => "Internet";

        public static readonly string propertiesPath = Directory.GetCurrentDirectory()+"/Resources/properties/";


        public AbstractProperties()
        {
            if (!Directory.Exists(propertiesPath))
            {
                Directory.CreateDirectory(propertiesPath);
                System.Diagnostics.Debug.WriteLine(propertiesPath + "created");
            }
        }
        public abstract string Description { get; }
        public abstract string ImagePath { get; }
        public abstract string Name { get; }

        public abstract IDictionary<string, string> GetAllProperties();
        public abstract bool GetBooleanProperty(string name, bool standard);
        public abstract double GetDoubleProperty(string name, double standard);
        public abstract int GetIntegerProperty(string name, int standard);
        public abstract string GetProperty(string name, string standard);
        public abstract bool HasPermission(string name);

        public void Save()
        {
            var filename = GetProperty(KeyName, string.Empty);
            if (filename.Equals(string.Empty))
            {
                throw new ResourceNotFoundException("No file name found");
            }
            Save(filename + ".txt");
        }

        public void Load()
        {
            var filename = GetProperty(KeyName, string.Empty);
            if (filename.Equals(string.Empty))
            {
                throw new ResourceNotFoundException("No file name found");
            }
            Load(filename + ".txt");
        }

        public abstract void Save(string name);
        public abstract void Load(string name);

        public abstract void SetProperty(string key, bool value, bool replace = true);
        public abstract void SetProperty(string key, double value, bool replace = true);
        public abstract void SetProperty(string key, int value, bool replace = true);
        public abstract void SetProperty(string key, string value, bool replace = true);

        public abstract bool ContainsProperty(string key);
    }
}