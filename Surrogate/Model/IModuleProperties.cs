using Surrogate.View.PropertieView;
using System.Collections.Generic;

namespace Surrogate.Model
{
    /// <summary>
    /// Interface specifing the properties class for a <see cref="Module"/>
    /// </summary>
    public interface IModuleProperties
    {
        string KeyName { get; }
        string KeyDescription { get; }
        string KeyMotor { get; }
        string KeyFaceCam { get; }
        string KeyFloorCam { get; }
        string KeyMicrophone { get; }
        string KeyDatabase { get; }
        string KeyImagePath { get; }
        string KeyInternet { get; }

        PropertiesView GetView();

        IDictionary<string, string> GetAllProperties();
        bool GetBooleanProperty(string name, bool standard);
        double GetDoubleProperty(string name, double standard);
        int GetIntegerProperty(string name, int standard);
        string GetProperty(string name, string standard);
        bool HasPermission(string name);

        /// <summary>
        /// Save the properties to a text file with default name.
        /// This is the preffered way to save properties on the filesystem
        /// </summary>
        void Save();

        /// <summary>
        /// Loads the properties from a text file with default name.
        /// This is the preffered way to load properties from the filesystem
        /// </summary>
        void Load();
        void Load(string fileName);
        void Save(string fileName);

        void SetBooleanProperty(string key, bool value, bool replace = true);
        void SetDoubleProperty(string key, double value, bool replace = true);
        void SetIntegerProperty(string key, int value, bool replace = true);



        void SetProperty(string key, string value, bool replace = true);
    }
}