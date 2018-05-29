// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer

namespace Surrogate.Implementations.FaceDetection
{
    using System.Windows;
    using System.Windows.Controls;
    using Surrogate.Model;
    using Surrogate.Model.Module;
    using Surrogate.Modules;
    using Surrogate.View;

    public class FaceDetectionModule : VisualModule<FaceDetectionProperties, FaceDetectionInfo>
    {
        public FaceDetectionModule() : base(new FaceDetectionProperties())
        {
                
        }

        public override ModuleView GetPage()
        {
            throw new System.NotImplementedException();
        }

        public override void OnDisselected()
        {
            
        }

        public override void OnSelected()
        {
            
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

    public class FaceDetectionProperties : ModuleProperties
    {
        public FaceDetectionProperties() : base("Gesichtserkennung", "Module zum visuellen Vorführen der Gesichtserkennund mithilfe der Opencv Bibliotheken", false, true, false,false, false)
        {
        }
    }

    public class FaceDetectionInfo: ModuleInfo
    {

    }
}