using System.Runtime.InteropServices;

namespace FallingSandSim.Core.Components
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Velocity
    {
        public float Dx, Dy;
    }
}