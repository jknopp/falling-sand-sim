using System.Runtime.CompilerServices;
using Arch.Core;
using FallingSandSim.Components;

namespace FallingSandSim.Systems
{
    public struct MoveDownSystem : IForEach<Position, Velocity>
    {
        private readonly IWorldDimensions _worldDimensions;

        public MoveDownSystem(IWorldDimensions worldDimensions)
        {
            _worldDimensions = worldDimensions;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update(ref Position pos, ref Velocity vel)
        {
            pos.Y += vel.Dy;

            if (pos.Y >= _worldDimensions.Height)
            {
                pos.Y = 0;
                pos.X = Random.Shared.Next(0, _worldDimensions.Width);
            }
        }
    }
}
