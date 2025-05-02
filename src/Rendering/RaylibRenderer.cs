using Raylib_cs;

namespace FallingSandSim.Rendering
{
    public class RaylibRenderer : IRenderer, IDisposable
    {
        private readonly int _cellSize;
        private readonly int _screenWidth;
        private readonly int _screenHeight;

        public RaylibRenderer(int screenWidth, int screenHeight, int cellSize = 10)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _cellSize = cellSize;
        }

        public void Init()
        {
            Raylib.InitWindow(_screenWidth, _screenHeight, "Matrix Rain");
            Raylib.SetTargetFPS(60);
        }

        public bool ShouldClose()
        {
            return Raylib.WindowShouldClose();
        }

        public void BeginFrame()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.Black);
        }

        public void EndFrame()
        {
            Raylib.EndDrawing();
        }

        public bool IsLeftMouseButtonDown()
        {
            return Raylib.IsMouseButtonDown(MouseButton.Left);
        }
        public (int, int) GetMouseXY(int cellSize)
        {
            return (Raylib.GetMouseX() / cellSize, Raylib.GetMouseY() / cellSize);
        }

        public void DrawCharAt(int x, int y, char c)
        {
            Raylib.DrawText(c.ToString(), x * _cellSize, y * _cellSize, _cellSize, Color.Green);
        }

        public void DrawRectangleParticleAt(int x, int y, ParticleType type)
        {
            Color color = type switch
            {
                ParticleType.Sand => new Color(194, 178, 128, 255),
                ParticleType.Fire => new Color(255, 69, 0, 255),
                ParticleType.Smoke => new Color(128, 128, 128, 255),
                _ => Color.White,
            };

            Raylib.DrawRectangle(x * _cellSize, y * _cellSize, _cellSize, _cellSize, color);
        }

        public void Dispose()
        {
            Raylib.CloseWindow();
        }
    }
}
