namespace FallingSandSim.Rendering
{
    public class RaylibWorldDimensions : IWorldDimensions
    {
        private readonly int _screenWidth;
        private readonly int _screenHeight;
        private readonly int _cellSize;

        public RaylibWorldDimensions(int screenWidth, int screenHeight, int cellSize = 10)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _cellSize = cellSize;
        }

        public int Width => _screenWidth / _cellSize;
        public int Height => _screenHeight / _cellSize;
        public int PixelWidth => _screenWidth;
        public int PixelHeight => _screenHeight;
        public int CellSize => _cellSize;
    }
}
