using System.Runtime.InteropServices;

namespace FallingSandSim.Components
{
    [StructLayout(LayoutKind.Sequential)]
    public struct HasMoved
    {
        public bool Value;
    }
}