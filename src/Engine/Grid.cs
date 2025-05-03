using System.Collections.Concurrent;
using Arch.Core;

namespace FallingSandSim.Engine
{
    public class Chunk
    {
        public const int ChunkSize = 64;
        public Entity[,] Cells = new Entity[ChunkSize, ChunkSize];
        public HashSet<(int, int)> DirtyCells = new();
    }

    public class Grid
    {
        private readonly ConcurrentDictionary<(int, int), Chunk> _chunks = new();

        public Chunk GetOrCreateChunk(int chunkX, int chunkY)
        {
            return _chunks.GetOrAdd((chunkX, chunkY), _ => new Chunk());
        }

        public Entity Get(int x, int y)
        {
            var (chunkX, chunkY) = (x / Chunk.ChunkSize, y / Chunk.ChunkSize);
            var (localX, localY) = ToLocalCoords(x, y);
            return GetOrCreateChunk(chunkX, chunkY).Cells[localX, localY];
        }

        public void Set(int x, int y, Entity entity)
        {
            var chunk = GetChunkAndCoords(x, y, out var localX, out var localY);
            chunk.Cells[localX, localY] = entity;
            lock (chunk.DirtyCells)
                chunk.DirtyCells.Add((localX, localY));
        }

        public void Remove(int x, int y)
        {
            var chunk = GetChunkAndCoords(x, y, out var localX, out var localY);
            if (chunk.Cells[localX, localY] != default)
            {
                chunk.Cells[localX, localY] = default;
                lock (chunk.DirtyCells) chunk.DirtyCells.Add((localX, localY));
            }
        }

        public void Move(int x1, int y1, int x2, int y2)
        {
            Set(x2, y2, Get(x1, y1));
            Remove(x1, y1);
        }

        public bool IsEmpty(int x, int y)
        {
            return Get(x, y) == default;
        }

        private static (int localX, int localY) ToLocalCoords(int x, int y)
        {
            return (
                (x % Chunk.ChunkSize + Chunk.ChunkSize) % Chunk.ChunkSize,
                (y % Chunk.ChunkSize + Chunk.ChunkSize) % Chunk.ChunkSize
            );
        }

        private Chunk GetChunkAndCoords(int x, int y, out int localX, out int localY)
        {
            var chunkX = x / Chunk.ChunkSize;
            var chunkY = y / Chunk.ChunkSize;
            (localX, localY) = ToLocalCoords(x, y);
            return GetOrCreateChunk(chunkX, chunkY);
        }

        public IEnumerable<(int x, int y)> DirtyCells()
        {
            foreach (var ((chunkX, chunkY), chunk) in _chunks)
            {
                lock (chunk.DirtyCells)
                {
                    foreach (var (localX, localY) in chunk.DirtyCells)
                    {
                        yield return (chunkX * Chunk.ChunkSize + localX, chunkY * Chunk.ChunkSize + localY);
                    }
                    chunk.DirtyCells.Clear();
                }
            }
        }

        public IEnumerable<((int chunkX, int chunkY), Chunk)> DirtyChunks()
        {
            foreach (var kvp in _chunks)
            {
                var chunk = kvp.Value;
                if (chunk.DirtyCells.Count > 0)
                    yield return (kvp.Key, chunk);
            }
        }
    }
}
