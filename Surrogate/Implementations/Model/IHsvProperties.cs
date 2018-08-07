// This file belongs to the source code of the "Surrogate Project"
// Copyright (c) 2018 All Rights Reserved
// Martin-Luther-Universitaet Halle-Wittenberg
// Lehrstuhl Wirtschaftsinformatik und Operation Research
// Autor: Wimmer, Simon-Justus Wimmer (simonjustuswimmer@googlemail.com)
using Emgu.CV.Structure;
using Surrogate.Implementations.Model;

namespace Surrogate.Implementations.Model
{
    public interface IHsvProperties
    {
        int CamNum { get; set; }
        bool Inverted { get; }
        Hsv Lower { get; }
        Hsv Upper { get; }

        void SetBounds(HsvBounds e);
    }
}