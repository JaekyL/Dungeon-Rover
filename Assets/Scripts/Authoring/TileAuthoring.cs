using System.Collections;
using System.Collections.Generic;using Unity.Entities;
using UnityEngine;

internal class TileAuthoring : MonoBehaviour
{
    
}

internal class TileBaker : Baker<TileAuthoring>
{
    public override void Bake(TileAuthoring authoring)
    {
        AddComponent<Tile>();
    }
}