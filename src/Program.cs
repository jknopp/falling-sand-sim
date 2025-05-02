using FallingSandSim;
using FallingSandSim.Rendering;

class Program
{
    static void Main()
    {
        var dimensions = new RaylibWorldDimensions(1600, 1200, cellSize: 10);
        var renderer = new RaylibRenderer(dimensions.PixelWidth, dimensions.PixelHeight, dimensions.CellSize);
        using var engine = new MatrixRainEngine(1000, dimensions, renderer);

        renderer.Init();
        while (!renderer.ShouldClose())
        {
            renderer.BeginFrame();

            if (renderer.IsLeftMouseButtonDown())
            {
                var (mouseX, mouseY) = renderer.GetMouseXY(dimensions.CellSize);
                engine.SpawnParticle(mouseX, mouseY, ParticleType.Sand);
            }

            engine.UpdateOneFrame();
            renderer.EndFrame();
        }

        renderer.Dispose();
    }
}
