using System.Runtime.CompilerServices;
using Arch.Core;
using FallingSandSim.Core.Components;

namespace FallingSandSim.Core.Systems
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