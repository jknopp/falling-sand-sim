using Arch.Core;
using FallingSandSim.Behaviors;
using FallingSandSim.Components;
using FallingSandSim.Systems;
using Schedulers;

namespace FallingSandSim
{
    public class MatrixRainEngine : IDisposable
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


        public MatrixRainEngine(int entityCount, IWorldDimensions dimensions, IRenderer renderer)
        {
            _dimensions = dimensions;
            _world = World.Create();
            _grid = new Grid();

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

            _world.EnsureCapacity<Position, Velocity>(entityCount);

            for (int i = 0; i < entityCount; i++)
            {
                _world.Create(
                    new Position
                    {
                        X = Random.Shared.Next(0, _dimensions.Width),
                        Y = Random.Shared.Next(0, _dimensions.Height)
                    },
                    new Velocity
                    {
                        Dx = 0,
                        Dy = Random.Shared.NextSingle() * 0.5f + 0.5f
                    }
                );
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

            var entity = _world.Create(
                new Position { X = mouseX, Y = mouseY },
                new Velocity { Dx = 0, Dy = 1 },
                new ParticleClassification { Type = type },
                new HasMoved { Value = false });

            _grid.Set(mouseX, mouseY, entity);
        }

        public void UpdateOneFrame()
        {
            _world.InlineParallelQuery<ResetMoveSystem, HasMoved>(in _movedQuery, ref ResetMoveSystem.Instance);
            _world.InlineParallelQuery<ParticleMoveSystem, Position, Velocity, ParticleClassification, HasMoved>(in _particleMoveQuery, ref _particleMoveSystem);
            _world.InlineQuery<DrawSystem, Position, ParticleClassification>(in _particleDrawQuery, ref _drawSystem);
        }

        public void Dispose()
        {
            _scheduler.Dispose();
        }
    }
}