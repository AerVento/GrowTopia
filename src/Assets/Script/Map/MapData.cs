using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrowTopia.Map{
    [System.Serializable]
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

        public List<GridInfo> Grids;


        public static string Serialize(MapData data){
            return JsonUtility.ToJson(data);
        }

        public static MapData Deserialize(string content){
            return JsonUtility.FromJson<MapData>(content);
        }
    }

    [System.Serializable]
    public class GridInfo{
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
    }
}

