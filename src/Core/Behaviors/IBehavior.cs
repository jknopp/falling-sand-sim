using Arch.Core;
using FallingSandSim.Core.Components;
using FallingSandSim.Engine;

namespace FallingSandSim.Core.Behaviors
{
    public interface IBehavior
    {
        void Update(Entity entity, ref Position pos, ref Velocity vel, ref ParticleClassification tag, Grid grid, World world);
    }
}