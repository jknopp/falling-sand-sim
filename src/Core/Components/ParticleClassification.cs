using System.Runtime.InteropServices;
using Raylib_cs;

namespace FallingSandSim.Components
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ParticleClassification
    {
        public ParticleType Type;
        // TODO: Don't depend on Raylib
        public Color Color;
    }
}