using GrowTopia.Items;
using Newtonsoft.Json;
using UnityEngine;

namespace GrowTopia.Map
{

    [System.Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public struct MapGridInfo
    {
        /// <summary>
        /// The position of the grid.
        /// </summary>
        public Vector2Int Position;

        public IMapBlock MapBlock;


        /// <summary>
        /// Gives a block id to create the grid info. 
        /// It will use the the block prototype <see cref="StaticBlock"/> as map block.
        /// </summary>
        /// <param name="pos">The position of grid.</param>
        /// <param name="blockId">The block id of block.</param>
        public MapGridInfo(Vector2Int pos, string blockId)
        {
            Position = pos;
            MapBlock = ItemLoaderManager.GetBlock(blockId).CreateInstance();
        }

        /// <summary>
        /// Gives a map block instance to create the grid info.
        /// </summary>
        /// <param name="pos">The position of grid.</param>
        /// <param name="block">The map block instance.</param>
        public MapGridInfo(Vector2Int pos, IMapBlock block){
            Position = pos;
            MapBlock = block;
        }
    }
}

