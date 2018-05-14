namespace Surrogate.Implementations.FaceDetection
{
    using System.Windows;
    using System.Windows.Controls;
    using Surrogate.Modules;

    public class FaceDetectionModule : Module<FaceDetectionProperties, FaceDetectionInfo>
    {
        public FaceDetectionModule() : base(new FaceDetectionProperties())
        {
                
        }

        public override ContentControl GetPage()
        {
            throw new System.NotImplementedException();
        }

        public override void Start(FaceDetectionInfo info)
        {
            throw new System.NotImplementedException();
        }

        public override void Stop()
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }

    public class FaceDetectionProperties : ModulProperties
    {
        public FaceDetectionProperties() : base("Gesichtserkennung", "Module zum visuellen Vorführen der Gesichtserkennund mithilfe der Opencv Bibliotheken", false, true, false,false, false)
        {
        }
    }

    public class FaceDetectionInfo: ModuleInfo
    {

    }
}