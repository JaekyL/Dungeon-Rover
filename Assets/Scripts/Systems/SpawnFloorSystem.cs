using System.Collections.Generic;
using Aspects;
using Components;
using Helper;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(InitializeGameSystem))]
    internal partial struct SpawnFloorSystem : ISystem
    {
        private EntityQuery _query;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            EntityQueryBuilder builder = new EntityQueryBuilder(Allocator.Temp);
            builder.WithAll<NewFloor>();
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
            if(_query.ToComponentDataArray<NewFloor>(Allocator.Temp).Length == 0) return;

            
            //Creating new Floor
            FloorConfig config = SystemAPI.GetSingleton<FloorConfig>();
            Entity configEntity = SystemAPI.GetSingletonEntity<FloorConfig>();
            FloorConfigAspect floorConfigAspect = SystemAPI.GetAspectRW<FloorConfigAspect>(configEntity);

            NativeArray<int> floorTiles = floorConfigAspect.GetRandomFloorTiles();
            NativeArray<Vector3> tilePositions = floorConfigAspect.CalculateTilePositions(floorTiles);

            BeginSimulationEntityCommandBufferSystem.Singleton beginECBSingleton = SystemAPI.GetSingleton<BeginSimulationEntityCommandBufferSystem.Singleton>();
            EntityCommandBuffer spawnECB = beginECBSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            for (int i = 0; i < tilePositions.Length; i++)
            {
                Entity floorTile = spawnECB.Instantiate(config.FloorPrefab);
                spawnECB.SetComponent(floorTile, new LocalTransform(){_Position = tilePositions[i], _Rotation = quaternion.Euler(math.radians(90),0,0), _Scale = floorTiles[i]});
            }

            //CalculateFloorElementPositions
            NativeArray<Vector3> floorElementPositions = floorConfigAspect.CalculateFloorElementPositions(tilePositions);

            //Creating New FloorTilesEvent
            Entity floorTilesEvent = spawnECB.CreateEntity();
            spawnECB.AddComponent(floorTilesEvent, new NewFloorTiles(){Positions = floorElementPositions});
            
            //Destroying the Event Entity
            EndSimulationEntityCommandBufferSystem.Singleton endECBSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            EntityCommandBuffer ecb = endECBSingleton.CreateCommandBuffer(state.WorldUnmanaged);
            NativeArray<Entity> eventEntities = _query.ToEntityArray(Allocator.Temp);

            foreach (Entity entity in eventEntities)
            {
                ecb.DestroyEntity(entity);
            }
        }
    }
}