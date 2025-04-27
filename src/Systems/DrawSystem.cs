using System.Runtime.CompilerServices;
using Arch.Core;
using FallingSandSim.Components;

namespace FallingSandSim.Systems
{
    public struct DrawSystem : IForEach<Position>
    {
        private readonly IRenderer _renderer;

        public DrawSystem(IRenderer renderer)
        {
            _renderer = renderer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update(ref Position pos)
        {
            int x = (int)Math.Round(pos.X);
            int y = (int)Math.Round(pos.Y);

            _renderer.DrawAt(x, y, 'X');
        }
    }
}
