using Arch.Core;
using FallingSandSim.Components;
using FallingSandSim.Systems;
using Schedulers;

namespace FallingSandSim
{
    public class MatrixRainEngine : IDisposable
    {
        private readonly IWorldDimensions _dimensions;
        private readonly World _world;
        private readonly JobScheduler _scheduler;
        private readonly QueryDescription _queryMove;
        private readonly QueryDescription _queryDraw;

        private MoveDownSystem _moveSystem;
        private DrawSystem _drawSystem;

        public MatrixRainEngine(int entityCount, IWorldDimensions dimensions, IRenderer renderer)
        {
            _dimensions = dimensions;
            _world = World.Create();
            _queryMove = new QueryDescription().WithAll<Position, Velocity>();
            _queryDraw = new QueryDescription().WithAll<Position>();

            _scheduler = new JobScheduler(new JobScheduler.Config
            {
                ThreadPrefixName = "MatrixRain",
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

            _moveSystem = new MoveDownSystem(dimensions);
            _drawSystem = new DrawSystem(renderer);
        }

        public void UpdateOneFrame()
        {
            _world.InlineParallelQuery<MoveDownSystem, Position, Velocity>(in _queryMove, ref _moveSystem);
            _world.InlineQuery<DrawSystem, Position>(in _queryDraw, ref _drawSystem);
        }

        public void Dispose()
        {
            _scheduler.Dispose();
        }
    }
}