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

            int worldX = (int)Math.Round(pos.X);
            int worldY = (int)Math.Round(pos.Y);

            var entity = _grid.Get(worldX, worldY); // optional but safe
            foreach (var behavior in _behaviors[particleClassification.Type])
            {
                behavior.Update(entity, ref pos, ref vel, ref particleClassification, _grid, _world);
            }

            hasMoved.Value = true;

            // If the particle didn't move, keep its cell dirty so it's redrawn
            if (_world.Has<Position>(entity) && _world.Has<ParticleClassification>(entity))
            {
                int localX = (worldX % Chunk.ChunkSize + Chunk.ChunkSize) % Chunk.ChunkSize;
                int localY = (worldY % Chunk.ChunkSize + Chunk.ChunkSize) % Chunk.ChunkSize;

                var chunk = _grid.GetOrCreateChunk(worldX / Chunk.ChunkSize, worldY / Chunk.ChunkSize);
                lock (chunk.DirtyCells)
                    chunk.DirtyCells.Add((localX, localY));
            }
        }
    }
}
