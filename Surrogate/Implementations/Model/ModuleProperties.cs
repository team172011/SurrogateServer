using Surrogate.Implementations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.Model
{
    public class ModulePropertiesBase : AbstractProperties
    {

        private Dictionary<string, string> _properties = new Dictionary<string, string>();

        public override string Description => _properties[KeyDescription];

        public override string ImagePath => _properties[KeyImagePath];

        public override string Name => _properties[KeyName];

        public ModulePropertiesBase(string name, string description, bool motor = false, bool faceCam = false, bool floorCam = false, bool microphone = false, bool database = false, bool internet = false)
        {
            _properties[KeyName] = name;
            _properties[KeyDescription] = description;
            _properties[KeyMotor] = motor.ToString();
            _properties[KeyFaceCam] = faceCam.ToString();

            _properties[KeyInternet] = internet.ToString();
            _properties[KeyMicrophone] = microphone.ToString();
            _properties[KeyInternet] = internet.ToString();
            _properties[KeyDatabase] = database.ToString();

            if (!_properties.ContainsKey(KeyImagePath)) // if no icon set, set default icon 
            {
                _properties[KeyImagePath] = FrameworkConstants.DefaultImagePath;
            }
            
        }

        public override void SetProperty(string key, string value, bool replace = true)
        {
            if(!_properties.ContainsKey(key))
            {
                _properties.Add(key, value);
            }
            else if (replace)
            {
                _properties[key] = value;
            } // do nothing if containsKey == true and !replace
            
        }

        public override void SetProperty(string key, int value, bool replace = true)
        {
            SetProperty(key, value.ToString(), replace);
        }

        public override void SetProperty(string key, double value, bool replace = true)
        {
            SetProperty(key, value.ToString(), replace);
        }

        public override void SetProperty(string key, bool value, bool replace = true)
        {
            SetProperty(key, value.ToString(), replace);
        }

        public override String GetProperty(string name, string standard)
        {
            if (_properties.ContainsKey(name))
            {
                return _properties[name];
            }
            else
            {
                return standard;
            }
        }

        public override bool GetBooleanProperty(string name, bool standard)
        {
            return GetProperty(name, standard.ToString()).ToLower().Equals("true");
        }

        public override int GetIntegerProperty(string name, int standard)
        {
            return int.Parse(GetProperty(name, standard.ToString()));
        }

        public override double GetDoubleProperty(string name, double standard)
        {
            return double.Parse(GetProperty(name, standard.ToString()));
        }

        /// <summary>
        /// Checks if a boolean property is true aka "if it has permission"
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public override bool HasPermission(string name)
        {
            if (!_properties.ContainsKey(name)) return false;
            return GetBooleanProperty(name, false);
        }

        public override IDictionary<string, string> GetAllProperties()
        {
            return _properties;
        }
        /// <summary>
        /// Saves all properties to a textfile identifier by fileName parameter.
        /// NOTE: Existing files will be replaced
        /// </summary>
        /// <param name="fileName"></param>
        public override void Save(string fileName)
        {
            var filePath = propertiesPath + fileName;
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            using (StreamWriter writer = File.AppendText(filePath))
            {
                foreach (var entry in _properties)
                {
                    writer.WriteLine(entry.Key + "=" + entry.Value);
                }
            }
        }

        /// <summary>
        /// Loads all key value pairs of the specified text file and stores them
        /// in the _properties field. Existing properties with the same key will 
        /// be replaced
        /// </summary>
        /// <param name="name"></param>
        public override void Load(string fileName)
        {
            var filePath = propertiesPath + fileName;
            if (!File.Exists(filePath))
            {
                return;
            }
            string[] lines = File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                var pair = line.Split(new[] { '=' }, 2);
                string key = pair[0];
                string value = pair[1];
                if (_properties.ContainsKey(key))
                {
                    _properties[key] = value;
                }
                else
                {
                    _properties.Add(key, value);
                }
            }
        }

        public override bool ContainsProperty(string key)
        {
            return _properties.ContainsKey(key);
        }
    }
}
