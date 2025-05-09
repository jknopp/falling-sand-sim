using FallingSandSim.Core;
using FallingSandSim.Engine;
using FallingSandSim.Rendering.Raylib;

namespace FallingSandSim;

class Program
{
    static void Main()
    {
        var dimensions = new WorldDimensions(1200, 800, cellSize: 10);
        var renderer = new RaylibRenderer(dimensions);
        using var engine = new FallingSandEngine(dimensions, renderer);

        renderer.Init();
        while (!renderer.ShouldClose())
        {
            renderer.BeginFrame();

            if (renderer.IsLeftMouseButtonDown())
            {
                var (mouseX, mouseY) = renderer.GetMouseXY(dimensions.CellSize);
                engine.SpawnParticle(mouseX, mouseY, ParticleType.Sand);
            }

            if (renderer.IsRightMouseButtonDown())
            {
                var (mouseX, mouseY) = renderer.GetMouseXY(dimensions.CellSize);
                engine.SpawnParticle(mouseX, mouseY, ParticleType.Water);
            }

            engine.UpdateOneFrame();
            renderer.EndFrame();
        }

        renderer.Dispose();
    }
}