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