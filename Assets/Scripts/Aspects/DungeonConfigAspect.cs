using Config;
using Unity.Collections;
using Unity.Entities;

namespace Aspects
{
    internal readonly partial struct DungeonConfigAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<RandomNumbers> _random;
        private readonly RefRO<DungeonConfig> _dungeonConfig;
        
        public TileStats GetRandomTile()
        {
            if (!_random.ValueRO.HasMaxTileWeight)
            {
                _random.ValueRW.MaxTileWeight = 0;
                _random.ValueRW.TileWeights = new NativeArray<int>(_dungeonConfig.ValueRO.TileStats.Length, Allocator.Persistent);
                
                for (int i = 0; i < _dungeonConfig.ValueRO.TileStats.Length; i++)
                {
                    _random.ValueRW.MaxTileWeight += _dungeonConfig.ValueRO.TileStats[i].Value.Weight;
                    _random.ValueRW.TileWeights[i] = _dungeonConfig.ValueRO.TileStats[i].Value.Weight;
                }

                _random.ValueRW.HasMaxTileWeight = true;
            }

            int checkValue = _random.ValueRW.Value.NextInt(0, _random.ValueRO.MaxTileWeight);
            int index = 0;
            for (int i = 0; i < _random.ValueRO.TileWeights.Length; i++)
            {
                if (checkValue <= _random.ValueRO.TileWeights[i])
                {
                    index = i;
                    break;
                }
                else
                {
                    checkValue -= _random.ValueRO.TileWeights[i];
                }
            }
            
            return _dungeonConfig.ValueRO.TileStats[index].Value;
        }
    }
}