using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

internal class DungeonConfigAuthoring : MonoBehaviour
{
    public GameObject tilePrefab;
    public int dungeonDepth;
    
}

internal class DungeonConfigBaker : Baker<DungeonConfigAuthoring>
{
    public override void Bake(DungeonConfigAuthoring authoring)
    {
        AddComponent(new DungeonConfig
        {
            TilePrefab = GetEntity(authoring.tilePrefab),
            DungeonDepth = authoring.dungeonDepth,
            
        });
    }
}