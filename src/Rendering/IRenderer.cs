using FallingSandSim.Core.Components;

namespace FallingSandSim.Rendering
{
    public interface IRenderer
    {
        void DrawCharAt(int x, int y, char c);
        void DrawRectangleParticleAt(int x, int y, ParticleClassification particleClassification);
    }
}