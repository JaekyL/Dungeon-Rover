using System.Collections.Generic;
using Helper;
using Sirenix.OdinInspector;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Config
{
    [CreateAssetMenu(menuName = "Game/TileConfig Asset", fileName = "TileConfig")]
    public class TileConfig : SerializedScriptableObject
    {
        public static Dictionary<string, BlobAssetReference<TileStats>> Allocations =
            new Dictionary<string, BlobAssetReference<TileStats>>();

        [SerializeField] private TileType tileType;
        [SerializeField] private Color color;
        [SerializeField] private int health;
        
        public BlobAssetReference<TileStats> ToBlobAssetReference()
        {
            if (Allocations.ContainsKey(this.name))
            {
                return Allocations[this.name];
            }
            else
            {
                BlobAssetReference<TileStats> tileStats;

                {
                    var builder = new BlobBuilder(Allocator.Temp);
                    ref var root = ref builder.ConstructRoot<TileStats>();

                    {
                        root.Type = tileType;
                    }
                    
                    {
                        root.Color = color;
                    }

                    {
                        root.Health = health;
                    }

                    tileStats = builder.CreateBlobAssetReference<TileStats>(Allocator.Persistent);
                    builder.Dispose();
                }
                
                Allocations.Add(this.name, tileStats);

                return tileStats;
            }
        }

    }

    public struct TileStats
    {
        public TileType Type;
        public Color Color;
        public int Health;
    }
}