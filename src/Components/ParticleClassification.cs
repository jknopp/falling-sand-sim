using System.Runtime.InteropServices;

namespace FallingSandSim.Components
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ParticleClassification
    {
        public ParticleType Type;
    }
}