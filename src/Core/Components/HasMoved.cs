using System.Runtime.InteropServices;

namespace FallingSandSim.Core.Components
{
    [StructLayout(LayoutKind.Sequential)]
    public struct HasMoved
    {
        public bool Value;
    }
}