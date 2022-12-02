using Components;
using Unity.Entities;
using UnityEngine;

namespace Authoring
{
    public class FloorTileAuthoring : MonoBehaviour
    {
        
    }
    
    internal class FloorTileBaker : Baker<FloorTileAuthoring>
    {
        public override void Bake(FloorTileAuthoring authoring)
        {
            AddComponent<FloorTile>();
        }
    }
}