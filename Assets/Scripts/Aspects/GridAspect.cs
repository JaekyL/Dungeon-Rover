using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Grid = Components.Grid;

namespace Aspects
{
    internal readonly partial struct GridAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<Grid> _grid;

        public void CreateGrid(NativeHashMap<int2,Vector3> grid)
        {
            
        }
    }
}