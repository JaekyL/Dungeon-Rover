using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

internal class DungeonConfigAuthoring : MonoBehaviour
{
    public GameObject tilePrefab;
    public float startingPointFreeSpaceDistance;
    
}

internal class DungeonConfigBaker : Baker<DungeonConfigAuthoring>
{
    public override void Bake(DungeonConfigAuthoring authoring)
    {
        AddComponent(new DungeonConfig
        {
            TilePrefab = GetEntity(authoring.tilePrefab),
            StartingPointFreeSpaceDistance = authoring.startingPointFreeSpaceDistance,
            
        });
    }
}