using System.Collections;
using System.Collections.Generic;
using GrowTopia.Items;
using GrowTopia.Serialization;
using Newtonsoft.Json;
using UnityEngine;

namespace GrowTopia.Map
{
    [System.Serializable]
    [JsonObject(MemberSerialization.Fields)]

    public class MapData
    {
        /// <summary>
        /// Width of map. (blocks)
        /// </summary>
        public int Width;

        /// <summary>
        /// Height of map. (blocks)
        /// </summary>
        public int Height;

        /// <summary>
        /// The spawn point of the map.
        /// </summary>
        public Vector2Int SpawnPoint;

        public Dictionary<Vector2Int,GridInfo> Grids;


        public static string Serialize(MapData data)
        {
            return JsonConvert.SerializeObject(data, new MapSerializationSettings());
        }

        public static MapData Deserialize(string content)
        {
            return JsonConvert.DeserializeObject<MapData>(content, new MapSerializationSettings());
        }
    }

    [System.Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class GridInfo
    {
        /// <summary>
        /// The position of the grid.
        /// </summary>
        public Vector2Int Position;

        /// <summary>
        /// The id of the block.
        /// </summary>
        public string BlockId;

        /// <summary>
        /// For variant block types, there may have some status which was saved on Server. 
        /// Client needs these content to recover to the correct status.
        /// </summary>
        public string ExtraContent;


        public IReadOnlyBlock Block
        {
            get
            {
                IReadOnlyItem item = ItemLoaderManager.Instance.Items.GetValueOrDefault(BlockId);
                if (item == null)
                {
                    Debug.LogError("Unknown block id:" + BlockId);
                    return null;
                }
                IReadOnlyBlock block = item as IReadOnlyBlock;
                if (block == null)
                {
                    Debug.LogError($"Id:{BlockId} is not a block but an item.");
                    return null;
                }
                return block;
            }
        }
    }
}

