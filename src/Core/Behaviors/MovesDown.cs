using Arch.Core;
using FallingSandSim.Core.Components;
using FallingSandSim.Engine;

namespace FallingSandSim.Core.Behaviors
{
    public class MovesDown : IBehavior
    {
        private static readonly Random _random = new();

        public void Update(Entity entity, ref Position pos, ref Velocity vel, ref ParticleClassification classification, Grid grid, World world)
        {
            int x = (int)Math.Round(pos.X);
            int y = (int)Math.Round(pos.Y);

            if (grid.IsEmpty(x, y + 1) && _random.NextDouble() < 0.9)
            {
                grid.Move(x, y, x, y + 1);
                pos.Y += 1;
            }
            else if (grid.IsEmpty(x - 1, y + 1) && _random.NextDouble() < 0.45)
            {
                grid.Move(x, y, x - 1, y + 1);
                pos.X -= 1;
                pos.Y += 1;
            }
            else if (grid.IsEmpty(x + 1, y + 1) && _random.NextDouble() < 0.45)
            {
                grid.Move(x, y, x + 1, y + 1);
                pos.X += 1;
                pos.Y += 1;
            }
        }
    }
}
