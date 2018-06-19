using Emgu.CV.Structure;
using Surrogate.Implementations.Model;
using Surrogate.Model;
using Surrogate.Model.Module;
using Surrogate.Modules;
using System;
using System.Windows.Controls;

namespace Surrogate.Implementations.Controller.Module
{
    class BallFollowingModule : VisualModule<ModulePropertiesBase, ModuleInfo>
    {
        public BallFollowingModule() : base(new BallFollowingProperties())
        {
        }

        public override IModuleProperties Properties => GetProperties();

        public override UserControl GetPage()
        {
            return null;
        }

        public override bool IsRunning()
        {
            throw new NotImplementedException();
        }

        public override void OnDisselected()
        {
            base.OnDisselected();
        }

        public override void OnSelected()
        {
            base.OnSelected();
        }

        public override void Start(ModuleInfo info)
        {
            
        }

        public override void Stop()
        {
            base.Stop();
        }
    }

    public class BallFollowingProperties : ModulePropertiesBase
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

        private int _camNum = 0;
        public int CamNum { get => _camNum; set => value = _camNum; }

        public BallFollowingProperties() : base("Ball folgen", "Nutzt eine Kamera, um einem speziellen Ball zu folgen", motor: true, floorCam: true)
    {
        SetProperty(KeyImagePath, @"C:\Users\ITM1\source\repos\Surrogate\Surrogate\resources\ballFollowing_controller_icon.png");
        Load();

        // set default values if no exist
        SetProperty(Key_H_Lower, 0, false);
        SetProperty(Key_S_Lower, 0, false);
        SetProperty(Key_V_Lower, 0, false);

        SetProperty(Key_H_Upper, 255, false);
        SetProperty(Key_S_Upper, 255, false);
        SetProperty(Key_V_Upper, 255, false);

        SetProperty(Key_Inverted, false, false);
        SetProperty(Key_CamNum, CamNum, false);
        Save();
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
