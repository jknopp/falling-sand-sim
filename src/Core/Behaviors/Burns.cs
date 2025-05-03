using Arch.Core;
using FallingSandSim.Core.Components;
using FallingSandSim.Engine;

namespace FallingSandSim.Core.Behaviors;

public class Burns : IBehavior
{
    private int lifetime = 100;

    public void Update(Entity entity, ref Position pos, ref Velocity vel, ref ParticleClassification particleClassification, Grid grid, World world)
    {
        if (lifetime-- <= 0)
        {
            grid.Remove((int)Math.Round(pos.X), (int)Math.Round(pos.Y));
            world.Destroy(entity);
        }
    }
}