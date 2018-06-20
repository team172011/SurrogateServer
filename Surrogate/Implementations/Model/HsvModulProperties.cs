using Emgu.CV.Structure;
using Surrogate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Surrogate.Implementations.Model
{
    public abstract class HsvModulProperties : ModulePropertiesBase, IHsvProperties
    {

        public readonly string Key_H_Lower = "H_Lower";
        public readonly string Key_S_Lower = "S_Lower";
        public readonly string Key_V_Lower = "V_Lower";

        public readonly string Key_H_Upper = "H_Upper";
        public readonly string Key_S_Upper = "S_Upper";
        public readonly string Key_V_Upper = "V_Upper";

        public readonly string Key_Inverted = "Inverted";
        public readonly string Key_CamNum = "Camera";

        public Hsv Lower { get => new Hsv(GetIntegerProperty(Key_H_Lower, 0), GetIntegerProperty(Key_S_Lower, 0), GetIntegerProperty(Key_V_Lower, 0)); }
        public Hsv Upper { get => new Hsv(GetIntegerProperty(Key_H_Upper, 255), GetIntegerProperty(Key_S_Upper, 255), GetIntegerProperty(Key_V_Upper, 255)); }
        public bool Inverted { get => GetBooleanProperty(Key_Inverted, false); }

        public int CamNum {
            get => GetIntegerProperty(Key_CamNum, 0);
            set {SetProperty(Key_CamNum, value); Save();} }

        public HsvModulProperties(string name, string description, bool motor = false, bool faceCam = false, bool floorCam = false, bool microphone = false, bool database = false, bool internet = false) : base(name, description, motor,faceCam,floorCam,microphone,database,internet)
        {

        }

        public void SetBounds(HsvBounds e)
        {
            SetProperty(Key_H_Lower, e.Lower.Hue);
            SetProperty(Key_S_Lower, e.Lower.Satuation);
            SetProperty(Key_V_Lower, e.Lower.Value);

            SetProperty(Key_H_Upper, e.Upper.Hue);
            SetProperty(Key_S_Upper, e.Upper.Satuation);
            SetProperty(Key_V_Upper, e.Upper.Value);

            SetProperty(Key_Inverted, e.Inverted);
            Save();
        }
    }
}
