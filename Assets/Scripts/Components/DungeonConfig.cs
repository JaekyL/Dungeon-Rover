using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

internal struct DungeonConfig : IComponentData
{
    public Entity TilePrefab;
    public int DungeonDepth;
}
