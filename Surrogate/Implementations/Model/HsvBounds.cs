using Emgu.CV.Structure;
using System;

namespace Surrogate.Implementations.Model
{
    public class HsvBounds : EventArgs
    {

        private readonly Hsv _lower;
        private readonly Hsv _upper;

        private readonly bool _inverted;

        public Hsv Lower { get => _lower; }
        public Hsv Upper { get => _upper; }
        public bool Inverted { get => _inverted; }

        public HsvBounds(Hsv lower, Hsv upper, bool inverted = false)
        {
            _lower = lower;
            _upper = upper;
            _inverted = false;
        }
    }
}
