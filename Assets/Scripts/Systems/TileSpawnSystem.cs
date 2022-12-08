using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aspects;
using Components;
using Helper;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Aspects;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Analytics;
using Random = Unity.Mathematics.Random;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]
internal partial struct TileSpawnSystem : ISystem
{
    private EntityQuery _query;
    private int _deepestFloor;
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<DungeonConfig>();
        
        EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp);
        builder.WithAll<NewFloorTiles>();
        _query = state.GetEntityQuery(builder);
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //Checking on Event Entity
        if(_query.ToComponentDataArray<NewFloorTiles>(Allocator.Temp).Length == 0) return;
        
        DungeonConfig config = SystemAPI.GetSingleton<DungeonConfig>();

        NativeArray<NewFloorTiles> newFloorTiles = _query.ToComponentDataArray<NewFloorTiles>(Allocator.Temp);
        BeginSimulationEntityCommandBufferSystem.Singleton ecbSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
        EntityCommandBuffer ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

        //Declaring startPosition to stop spawning for the starting area
        Vector3 startPosition = ArrayHelper.GetAveragePosition(newFloorTiles[0].Positions.GetValueArray(Allocator.Temp).ToArray());
        
        //Creating "Breakable" DungeonParts
        foreach (NewFloorTiles newFloor in newFloorTiles)
        {
            foreach (KVPair<int2,Vector3> position in newFloor.Positions)
            {
                if(Vector3.Distance(startPosition, position.Value) < config.StartingPointFreeSpaceDistance) continue;
                
                Entity entity = ecb.Instantiate(config.TilePrefab);
                ecb.SetComponent(entity, new LocalTransform(){_Position = position.Value + Vector3.up * 0.55f, _Rotation = quaternion.identity, _Scale = 1f});
                ecb.AddComponent<TileColor>(entity);

                if (position.Value.x > 0)
                {
                    ecb.SetComponent(entity, new TileColor(){Value = new float4(config.TileStats[0].Value.Color.r, config.TileStats[0].Value.Color.g, config.TileStats[0].Value.Color.b, 1)});
                }
                else
                {
                    ecb.SetComponent(entity, new TileColor(){Value = new float4(config.TileStats[1].Value.Color.r, config.TileStats[1].Value.Color.g, config.TileStats[1].Value.Color.b, 1)});
                }
                
                
            }
        }

        //Destroying the Event Entity
        EndSimulationEntityCommandBufferSystem.Singleton endECBSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
        EntityCommandBuffer endECB = endECBSingleton.CreateCommandBuffer(state.WorldUnmanaged);
        NativeArray<Entity> eventEntities = _query.ToEntityArray(Allocator.Temp);
        foreach (Entity entity in eventEntities)
        {
            endECB.DestroyEntity(entity);
        }
    }
}
