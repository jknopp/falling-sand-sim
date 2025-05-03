namespace FallingSandSim
{
    public interface IWorldDimensions
    {
        int Width { get; }
        int Height { get; }
        int PixelWidth { get; }
        int PixelHeight { get; }
        int CellSize { get; }
    }
}
