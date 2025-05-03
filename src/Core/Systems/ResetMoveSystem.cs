using System.Runtime.CompilerServices;
using Arch.Core;
using FallingSandSim.Components;

namespace FallingSandSim.Systems
{
    public struct ResetMoveSystem : IForEach<HasMoved>
    {
        public static ResetMoveSystem Instance = new();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Update(ref HasMoved moved)
        {
            moved.Value = false;
        }
    }
}