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

        public void DrawAt(int x, int y, char c)
        {
            Raylib.DrawText(c.ToString(), x * _cellSize, y * _cellSize, _cellSize, Color.Green);
        }

        public void Dispose()
        {
            Raylib.CloseWindow();
        }
    }
}
