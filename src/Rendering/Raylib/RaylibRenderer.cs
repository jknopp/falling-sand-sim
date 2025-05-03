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
            Raylib_cs.Raylib.DrawRectangle(x * _cellSize, y * _cellSize, _cellSize, _cellSize, particleClassification.Color);
        }

        public void Dispose()
        {
            Raylib_cs.Raylib.CloseWindow();
        }

        public static Color GetColor(ParticleType type)
        {
            return type switch
            {
                ParticleType.Dirt => new Color(139, 69, 19, 255),
                ParticleType.Sand => new Color(194, 178, 128, 255),
                ParticleType.Fire => new Color(255, 69, 0, 255),
                ParticleType.Smoke => new Color(128, 128, 128, 255),
                _ => Color.White,
            };
        }

        public static Color GetVariedColor(Color baseColor)
        {
            int variation = Random.Shared.Next(-10, 10);

            return new Color(
                (byte)Clamp(baseColor.R + variation, 0, 255),
                (byte)Clamp(baseColor.G + variation, 0, 255),
                (byte)Clamp(baseColor.B + variation, 0, 255),
                baseColor.A
            );
        }

        private static int Clamp(int value, int min, int max) => Math.Max(min, Math.Min(max, value));
    }
}
