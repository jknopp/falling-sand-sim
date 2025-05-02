using System.Runtime.CompilerServices;
using Arch.Core;
using FallingSandSim.Components;

namespace FallingSandSim.Systems
{
    public struct ParticleMoveSystem : IForEach<Position, Velocity, ParticleClassification, HasMoved>
    {
        private readonly Grid _grid;
        private readonly World _world;
        private readonly Dictionary<ParticleType, List<IBehavior>> _behaviors;

        public ParticleMoveSystem(Grid grid, World world, Dictionary<ParticleType, List<IBehavior>> behaviors)
        {
            _grid = grid;
            _world = world;
            _behaviors = behaviors;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update(ref Position pos, ref Velocity vel, ref ParticleClassification particleClassification, ref HasMoved hasMoved)
        {
            if (hasMoved.Value) return;

            foreach (var behavior in _behaviors[particleClassification.Type])
            {
                var chunk = _grid.Get((int)Math.Round(pos.X), (int)Math.Round(pos.Y));
                behavior.Update(chunk, ref pos, ref vel, ref particleClassification, _grid, _world);
            }

            hasMoved.Value = true;
        }
    }
}
