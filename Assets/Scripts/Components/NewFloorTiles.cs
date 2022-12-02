
using Unity.Collections;
using Unity.Entities;
using UnityEngine;


namespace Components
{
    public struct NewFloorTiles : IComponentData
    {
        public NativeArray<Vector3> Positions;
    }
}