using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.Model
{
    public class ModuleProperties
    {
        public ModuleProperties(string name, string description, bool motor = false, bool faceCam = false, bool floorCam = false, bool microphone = false, bool database = false, bool internet = false)
        {
            _name = name;
            _description = description;
            _motor = motor;
            _faceCam = faceCam;
            _floorCam = floorCam;
            _microphone = microphone;
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
}
