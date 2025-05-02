using System.Diagnostics;
using FallingSandSim.Components;
using Xunit;

namespace FallingSandSim.Tests
{
    public class MatrixRainEngineStressTests
    {
        [Fact]
        public void MatrixRainEngine_StressUnderHighEntityCount_MaintainsStableFrameTime()
        {
            // Arrange
            const int entityCount = 10000;
            const int frameCount = 500;
            const double acceptableSpikeThresholdMs = 4;
            const double allowedSpikeRate = 0.01;

            var dimensions = new DummyWorldDimensions(800, 600, cellSize: 10);
            var renderer = new MockRenderer();
            using var engine = new MatrixRainEngine(dimensions, renderer);

            // Pre-spawn particles into the world
            for (int i = 0; i < entityCount; i++)
            {
                int x = i % dimensions.Width;
                int y = i / dimensions.Width;
                engine.SpawnParticle(x, y, ParticleType.Sand);
            }

            var frameTimes = new List<double>();
            var stopwatch = new Stopwatch();

            // Warmup frames (optional)
            for (int i = 0; i < 10; i++)
            {
                engine.UpdateOneFrame();
            }

            // Act
            for (int i = 0; i < frameCount; i++)
            {
                stopwatch.Restart();
                engine.UpdateOneFrame();
                stopwatch.Stop();
                frameTimes.Add(stopwatch.Elapsed.TotalMilliseconds);
            }

            // Analyze results
            double average = frameTimes.Average();
            double stdDev = Math.Sqrt(frameTimes.Average(v => Math.Pow(v - average, 2)));
            int spikeFrames = frameTimes.Count(ft => ft > acceptableSpikeThresholdMs);
            double spikeRate = (double)spikeFrames / frameTimes.Count;

            // Assert stability
            Assert.InRange(average, 0.2, 1.0);
            Assert.True(stdDev < 2.0, $"StdDev too high: {stdDev:F2} ms");
            Assert.True(spikeRate <= allowedSpikeRate, $"Too many spikes: {spikeFrames}/{frameTimes.Count} frames ({spikeRate:P2}) exceeded {acceptableSpikeThresholdMs} ms");

            Console.WriteLine($"✅ Stress Test: Avg={average:F2}ms, StdDev={stdDev:F2}ms, Spikes={spikeFrames} frames >{acceptableSpikeThresholdMs}ms.");
        }

        private class DummyWorldDimensions : IWorldDimensions
        {
            public DummyWorldDimensions(int width, int height, int cellSize = 10)
            {
                PixelWidth = width;
                PixelHeight = height;
                CellSize = cellSize;
            }

            public int Width => PixelWidth / CellSize;
            public int Height => PixelHeight / CellSize;
            public int PixelWidth { get; }
            public int PixelHeight { get; }
            public int CellSize { get; }
        }

        private class MockRenderer : IRenderer
        {
            public void DrawCharAt(int x, int y, char c) { }
            public void DrawRectangleParticleAt(int x, int y, ParticleClassification particleClassificationx) { }
        }
    }
}