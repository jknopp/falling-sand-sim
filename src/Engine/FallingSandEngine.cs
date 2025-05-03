using System.Collections.Concurrent;
using Arch.Core;
using FallingSandSim.Core;
using FallingSandSim.Core.Behaviors;
using FallingSandSim.Core.Components;
using FallingSandSim.Core.Systems;
using FallingSandSim.Rendering;
using FallingSandSim.Rendering.Raylib;
using Schedulers;

namespace FallingSandSim.Engine
{
    public class FallingSandEngine : IDisposable
    {
        private readonly IWorldDimensions _dimensions;
        private readonly World _world;
        private readonly Grid _grid;
        private readonly JobScheduler _scheduler;
        private readonly QueryDescription _movedQuery;
        private readonly QueryDescription _particleMoveQuery;
        private readonly QueryDescription _particleDrawQuery;

        private ParticleMoveSystem _particleMoveSystem;
        private DrawSystem _drawSystem;

        private static Dictionary<ParticleType, List<IBehavior>> _behaviors = new()
        {
            { ParticleType.Sand, new List<IBehavior> { new MovesDown() } },
            { ParticleType.Fire, new List<IBehavior> { new Burns(), new RisesUp() } },
            { ParticleType.Smoke, new List<IBehavior> { new RisesUp() } },
        };


        public FallingSandEngine(IWorldDimensions dimensions, IRenderer renderer)
        {
            _dimensions = dimensions;
            _world = World.Create();
            _grid = new Grid(renderer);

            _movedQuery = new QueryDescription().WithAll<HasMoved>();
            _particleMoveQuery = new QueryDescription().WithAll<Position, Velocity, ParticleClassification, HasMoved>();
            _particleDrawQuery = new QueryDescription().WithAll<Position, ParticleClassification>();

            _scheduler = new JobScheduler(new JobScheduler.Config
            {
                ThreadPrefixName = "Falling Sand Sim",
                ThreadCount = 0,
                MaxExpectedConcurrentJobs = 64,
                StrictAllocationMode = false
            });
            World.SharedJobScheduler = _scheduler;

            for (int x = 0; x < _dimensions.Width; x++)
            {
                int y = _dimensions.Height - 1;
                var ground = _world.Create(
                    new Position { X = x, Y = y },
                    new ParticleClassification
                    {
                        Type = ParticleType.Dirt,
                        Color = RaylibRenderer.GetVariedColor(RaylibRenderer.GetColor(ParticleType.Dirt)) // Would prefer not having this dependency here
                    }
                );
                _grid.Set(x, y, ground);
            }

            _particleMoveSystem = new ParticleMoveSystem(_grid, _world, _behaviors);
            _drawSystem = new DrawSystem(renderer);
        }

        public void SpawnParticle(int mouseX, int mouseY, ParticleType type)
        {
            if (mouseX < 0 || mouseX >= _dimensions.Width || mouseY < 0 || mouseY >= _dimensions.Height)
                return;

            if (!_grid.IsEmpty(mouseX, mouseY))
                return;

            var color = RaylibRenderer.GetVariedColor(RaylibRenderer.GetColor(type));
            var entity = _world.Create(
                new Position { X = mouseX, Y = mouseY },
                new Velocity { Dx = 0, Dy = 1 },
                new ParticleClassification { Type = type, Color = color },
                new HasMoved { Value = false });

            _grid.Set(mouseX, mouseY, entity);
        }

        public void UpdateOneFrame()
        {
            _world.InlineParallelQuery<ResetMoveSystem, HasMoved>(in _movedQuery, ref ResetMoveSystem.Instance);
            _world.InlineParallelQuery<ParticleMoveSystem, Position, Velocity, ParticleClassification, HasMoved>(in _particleMoveQuery, ref _particleMoveSystem);

            var drawQueue = new ConcurrentQueue<(Position pos, ParticleClassification tag)>();

            // Parallel render preparation over dirty chunks
            Parallel.ForEach(_grid.DirtyChunks(), chunkEntry =>
            {
                var ((chunkX, chunkY), chunk) = chunkEntry;
                var toKeep = new List<(int, int)>();

                foreach (var (localX, localY) in chunk.DirtyCells)
                {
                    int worldX = chunkX * Chunk.ChunkSize + localX;
                    int worldY = chunkY * Chunk.ChunkSize + localY;

                    var entity = _grid.Get(worldX, worldY);
                    if (entity == default ||
                        !_world.Has<Position>(entity) ||
                        !_world.Has<ParticleClassification>(entity))
                        continue;

                    ref var pos = ref _world.Get<Position>(entity);
                    ref var tag = ref _world.Get<ParticleClassification>(entity);
                    drawQueue.Enqueue((pos, tag));

                    if (!_world.Has<HasMoved>(entity))
                        toKeep.Add((localX, localY));
                }

                // Safely replace DirtyCells after scanning
                chunk.DirtyCells.Clear();
                foreach (var cell in toKeep)
                    chunk.DirtyCells.Add(cell);
            });

            // Main-thread rendering to comply with Raylib
            while (drawQueue.TryDequeue(out var item))
            {
                _drawSystem.Update(ref item.pos, ref item.tag);
            }
        }
        public void Dispose()
        {
            _scheduler.Dispose();
        }
    }
}