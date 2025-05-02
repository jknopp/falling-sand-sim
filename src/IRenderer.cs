namespace FallingSandSim
{
    public interface IRenderer
    {
        void DrawCharAt(int x, int y, char c);
        void DrawRectangleParticleAt(int x, int y, ParticleType type);
    }
}