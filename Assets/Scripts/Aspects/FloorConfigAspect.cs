using System;
using Helper;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Aspects
{
    internal readonly partial struct FloorConfigAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<RandomStairs> _random;
        private readonly RefRO<FloorConfig> _floorConfig;

        public NativeArray<int> GetRandomFloorTiles()
        {
            NativeArray<int> tiles = new NativeArray<int>(_floorConfig.ValueRO.MaxFloorTiles, Allocator.Temp);

            for (int i = 0; i < _floorConfig.ValueRO.MaxFloorTiles; i++)
            {
                tiles[i] = _random.ValueRW.Value.NextInt(_floorConfig.ValueRO.MinFloorTileSize, _floorConfig.ValueRO.MaxFloorTileSize);
            }
            
            return tiles;
        }

        public NativeArray<Vector3> CalculateTilePositions(NativeArray<int> floorTiles)
        {
            NativeArray<Vector3> tilePositions = new NativeArray<Vector3>(floorTiles.Length, Allocator.Temp);
            tilePositions[0] = new Vector2(0,0);

            for (int i = 1; i < floorTiles.Length; i++)
            {
                int rngDirection = _random.ValueRW.Value.NextInt(0, 4);

                switch (rngDirection)
                {
                    case 0: tilePositions[i] = tilePositions[i - 1] + new Vector3(floorTiles[i - 1] / 2f + floorTiles[i] / 2f, 0, 0); break;
                    case 1: tilePositions[i] = tilePositions[i - 1] - new Vector3(floorTiles[i - 1] / 2f + floorTiles[i] / 2f, 0, 0); break;
                    case 2: tilePositions[i] = tilePositions[i - 1] + new Vector3(0, 0, floorTiles[i - 1] / 2f + floorTiles[i] / 2f); break;
                    case 3: tilePositions[i] = tilePositions[i - 1] - new Vector3(0, 0, floorTiles[i - 1] / 2f + floorTiles[i] / 2f); break;
                }
            }
            
            return tilePositions;
        }

        public NativeArray<Vector3> CalculateFloorElementPositions(NativeArray<Vector3> tilePositions)
        {
            NativeArray<Vector3> floorElementPositions = new NativeArray<Vector3>(tilePositions.Length, Allocator.Temp); //TODO LENGTH IS WRONG

            return floorElementPositions;
        }
    }
}