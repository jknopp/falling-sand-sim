using System.Runtime.InteropServices;

namespace FallingSandSim.Components
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Velocity
    {
        public float Dx, Dy;
    }
}