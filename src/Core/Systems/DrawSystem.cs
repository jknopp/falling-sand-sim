using System.Runtime.CompilerServices;
using Arch.Core;
using FallingSandSim.Core.Components;
using FallingSandSim.Rendering;

namespace FallingSandSim.Core.Systems
{
    public struct DrawSystem : IForEach<Position, ParticleClassification>
    {
        private readonly IRenderer _renderer;

        public DrawSystem(IRenderer renderer)
        {
            _renderer = renderer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update(ref Position pos, ref ParticleClassification particleClassification)
        {
            var px = (int)Math.Round(pos.X);
            var py = (int)Math.Round(pos.Y);
            _renderer.DrawRectangleParticleAt(px, py, particleClassification);
        }
    }
}