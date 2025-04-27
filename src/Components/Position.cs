using System.Runtime.InteropServices;

namespace FallingSandSim.Components
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Position
    {
        public float X, Y;
    }
}