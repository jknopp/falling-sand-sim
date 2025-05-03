using System.Runtime.InteropServices;

namespace FallingSandSim.Core.Components
{
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct ParticleColor
    {
        public readonly byte R;
        public readonly byte G;
        public readonly byte B;
        public readonly byte A;

        public ParticleColor(byte r, byte g, byte b, byte a = 255)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public static ParticleColor FromRGB(int r, int g, int b) =>
            new((byte)Clamp(r, 0, 255), (byte)Clamp(g, 0, 255), (byte)Clamp(b, 0, 255));

        private static int Clamp(int val, int min, int max) =>
            val < min ? min : val > max ? max : val;
    }
}
