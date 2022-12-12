using System.Numerics;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Components
{
    internal struct GridCell
    {
        public int2 Index;
        public Vector3 Position;
        public bool Free;
        //public NativeArray<int2> NeighbourIDs;
    }
}