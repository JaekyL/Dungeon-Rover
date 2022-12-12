using System.Collections;
using System.Collections.Generic;
using Config;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

readonly partial struct TileAspect : IAspect
{
    public readonly Entity Self;

    private readonly TransformAspect _transform;

    private readonly RefRW<Tile> _tile;

    public float3 Position
    {
        get => _transform.LocalPosition;
        set => _transform.LocalPosition = value;
    }
}
