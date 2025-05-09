using FallingSandSim.Core.Components;

namespace FallingSandSim.Core
{
    public static class ParticleColorGenerator
    {
        public static ParticleColor GetVariedColor(ParticleType type)
        {
            var (r, g, b, a) = type switch
            {
                ParticleType.Dirt => (139, 69, 19, 255),
                ParticleType.Sand => (194, 178, 128, 255),
                ParticleType.Fire => (255, 69, 0, 255),
                ParticleType.Smoke => (128, 128, 128, 255),
                ParticleType.Water => (30, 144, 255, 255),
                _ => (255, 255, 255, 255)
            };

            int variation = Random.Shared.Next(-10, 10);
            return new ParticleColor(
                ClampByte(r + variation),
                ClampByte(g + variation),
                ClampByte(b + variation),
                (byte)a
            );
        }

        private static byte ClampByte(int value) => (byte)Math.Clamp(value, 0, 255);
    }
}