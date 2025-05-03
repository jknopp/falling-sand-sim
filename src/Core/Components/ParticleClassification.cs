using System.Runtime.InteropServices;

namespace FallingSandSim.Core.Components
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ParticleClassification
    {
        public ParticleType Type;
        // TODO: Don't depend on Raylib
        public ParticleColor Color;
    }
}