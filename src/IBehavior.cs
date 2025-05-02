using Arch.Core;
using FallingSandSim.Components;

namespace FallingSandSim
{
    public interface IBehavior
    {
        void Update(Entity entity, ref Position pos, ref Velocity vel, ref ParticleClassification tag, Grid grid, World world);
    }
}