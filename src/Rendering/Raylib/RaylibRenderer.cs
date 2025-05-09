using FallingSandSim.Core;
using FallingSandSim.Core.Components;
using Raylib_cs;

namespace FallingSandSim.Rendering.Raylib
{
    public class RaylibRenderer : IRenderer, IDisposable
    {
        private readonly int _cellSize;
        private readonly int _screenWidth;
        private readonly int _screenHeight;

        public RaylibRenderer(IWorldDimensions worldDimensions)
        {
            _screenWidth = worldDimensions.PixelWidth;
            _screenHeight = worldDimensions.PixelHeight;
            _cellSize = worldDimensions.CellSize;
        }

        public void Init()
        {
            Raylib_cs.Raylib.InitWindow(_screenWidth, _screenHeight, "Falling Sand Simulation");
            Raylib_cs.Raylib.SetTargetFPS(60);
        }

        public bool ShouldClose()
        {
            return Raylib_cs.Raylib.WindowShouldClose();
        }

        public void BeginFrame()
        {
            Raylib_cs.Raylib.BeginDrawing();
            Raylib_cs.Raylib.ClearBackground(Color.Black);
        }

        public void EndFrame()
        {
            Raylib_cs.Raylib.EndDrawing();
        }

        public bool IsLeftMouseButtonDown()
        {
            return Raylib_cs.Raylib.IsMouseButtonDown(MouseButton.Left);
        }

        public bool IsRightMouseButtonDown()
        {
            return Raylib_cs.Raylib.IsMouseButtonDown(MouseButton.Right);
        }

        public (int, int) GetMouseXY(int cellSize)
        {
            return (Raylib_cs.Raylib.GetMouseX() / cellSize, Raylib_cs.Raylib.GetMouseY() / cellSize);
        }

        public void DrawCharAt(int x, int y, char c)
        {
            Raylib_cs.Raylib.DrawText(c.ToString(), x * _cellSize, y * _cellSize, _cellSize, Color.Green);
        }

        public void DrawRectangleParticleAt(int x, int y, ParticleClassification particleClassification)
        {
            var color = ToRaylibColor(particleClassification.Color);
            Raylib_cs.Raylib.DrawRectangle(x * _cellSize, y * _cellSize, _cellSize, _cellSize, color);
        }

        public void Dispose()
        {
            Raylib_cs.Raylib.CloseWindow();
        }

        private static Color ToRaylibColor(ParticleColor color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }
    }
}
