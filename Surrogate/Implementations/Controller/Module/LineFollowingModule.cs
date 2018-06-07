
using Emgu.CV;
using Surrogate.Model;
using Surrogate.Model.Module;
using Surrogate.Modules;
using Surrogate.View;
using System;

using System.Windows.Controls;

namespace Surrogate.Implementations.Controller.Module
{
    public class LineFollowingModule : VisualModule<ModuleProperties, LineFollowingInfo>
    {
        private readonly LineFollowingModuleView _view;

        private Image _currentImage;
        private VideoCapture _currentVideoCapture;


        public LineFollowingModule(object imView) : base(new ModuleProperties("Linie folgen", "Modul zum folgen einer speziellen Linie auf dem Boden", motor: true, floorCam: true))
        {
            _view = new LineFollowingModuleView(this);
        }

        public override UserControl GetPage()
        {
            return _view;
        }

        public override void Start(LineFollowingInfo info)
        {
            
        }
    }

    public class LineFollowingInfo : ModuleInfo
    {
        private readonly Image _image;
        public Image Image => _image;

        public LineFollowingInfo(System.Windows.Controls.Image image)
        {
            _image = image;
        }
    }
}
