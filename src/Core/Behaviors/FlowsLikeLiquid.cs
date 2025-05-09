using Arch.Core;
using FallingSandSim.Core.Components;
using FallingSandSim.Engine;

namespace FallingSandSim.Core.Behaviors;

public class FlowsLikeLiquid : IBehavior
{
    public void Update(Entity entity, ref Position pos, ref Velocity vel, ref ParticleClassification tag, Grid grid, World world)
    {
        int x = (int)Math.Round(pos.X);
        int y = (int)Math.Round(pos.Y);

        // Prefer falling straight down
        if (grid.IsEmpty(x, y + 1))
        {
            grid.Move(x, y, x, y + 1);
            pos.Y += 1;
            return;
        }

        // Try falling diagonally (randomized direction)
        bool tryLeftFirst = Random.Shared.Next(2) == 0;
        if (tryLeftFirst)
        {
            if (grid.IsEmpty(x - 1, y + 1))
            {
                grid.Move(x, y, x - 1, y + 1);
                pos.X -= 1; pos.Y += 1;
                return;
            }
            if (grid.IsEmpty(x + 1, y + 1))
            {
                grid.Move(x, y, x + 1, y + 1);
                pos.X += 1; pos.Y += 1;
                return;
            }
        }
        else
        {
            if (grid.IsEmpty(x + 1, y + 1))
            {
                grid.Move(x, y, x + 1, y + 1);
                pos.X += 1; pos.Y += 1;
                return;
            }
            if (grid.IsEmpty(x - 1, y + 1))
            {
                grid.Move(x, y, x - 1, y + 1);
                pos.X -= 1; pos.Y += 1;
                return;
            }
        }

        // Spread left/right on flat surface (with persistence)
        int dir = Random.Shared.Next(2) == 0 ? -1 : 1;
        for (int i = 1; i <= 3; i++) // Adjustable spread radius
        {
            if (grid.IsEmpty(x + dir * i, y))
            {
                grid.Move(x, y, x + dir * i, y);
                pos.X += dir * i;
                return;
            }

            // Try the opposite side
            if (grid.IsEmpty(x - dir * i, y))
            {
                grid.Move(x, y, x - dir * i, y);
                pos.X -= dir * i;
                return;
            }
        }
    }
}
