using Arch.Core;

namespace FallingSandSim
{
    public class Chunk
    {
        public const int ChunkSize = 64;
        public Entity[,] Cells = new Entity[ChunkSize, ChunkSize];
    }

    public class Grid
    {
        private readonly Dictionary<(int, int), Chunk> _chunks = new();

        public Grid() { }
        public bool InBounds(int x, int y) => true; // Assume infinite for now

        public Chunk GetOrCreateChunk(int chunkX, int chunkY)
        {
            if (!_chunks.TryGetValue((chunkX, chunkY), out var chunk))
            {
                chunk = new Chunk();
                _chunks[(chunkX, chunkY)] = chunk;
            }
            return chunk;
        }

        public Entity Get(int x, int y)
        {
            var (chunkX, chunkY) = (x / Chunk.ChunkSize, y / Chunk.ChunkSize);
            var (localX, localY) = (Math.Abs(x % Chunk.ChunkSize), Math.Abs(y % Chunk.ChunkSize));
            return GetOrCreateChunk(chunkX, chunkY).Cells[localX, localY];
        }

        public void Set(int x, int y, Entity entity)
        {
            var (chunkX, chunkY) = (x / Chunk.ChunkSize, y / Chunk.ChunkSize);
            var (localX, localY) = (Math.Abs(x % Chunk.ChunkSize), Math.Abs(y % Chunk.ChunkSize));
            GetOrCreateChunk(chunkX, chunkY).Cells[localX, localY] = entity;
        }

        public void Remove(int x, int y)
        {
            var (chunkX, chunkY) = (x / Chunk.ChunkSize, y / Chunk.ChunkSize);
            var (localX, localY) = (Math.Abs(x % Chunk.ChunkSize), Math.Abs(y % Chunk.ChunkSize));
            GetOrCreateChunk(chunkX, chunkY).Cells[localX, localY] = default;
        }

        public void Move(int x1, int y1, int x2, int y2)
        {
            Set(x2, y2, Get(x1, y1));
            Remove(x1, y1);
        }

        public bool IsEmpty(int x, int y) => Get(x, y) == default;

    }
}