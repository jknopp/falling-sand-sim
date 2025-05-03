using System.Runtime.InteropServices;

namespace FallingSandSim.Core.Components
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Position
    {
        public float X, Y;
    }
}