using Arch.Core;
using FallingSandSim.Core.Components;
using FallingSandSim.Engine;

namespace FallingSandSim.Core.Behaviors
{
    public class RisesUp : IBehavior
    {
        public void Update(Entity entity, ref Position pos, ref Velocity vel, ref ParticleClassification particleClassification, Grid grid, World world)
        {
            int x = (int)Math.Round(pos.X);
            int y = (int)Math.Round(pos.Y);

            if (grid.IsEmpty(x, y - 1))
            {
                grid.Move(x, y, x, y - 1);
                pos.Y -= 1;
            }
        }
    }
}