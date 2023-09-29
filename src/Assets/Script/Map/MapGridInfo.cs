using GrowTopia.Items;
using Newtonsoft.Json;
using UnityEngine;

namespace GrowTopia.Map
{
    public interface IReadOnlyMapGrid
    {
        public Vector2Int Position { get; }
        public string BlockId { get; }
        public string ExtraContent { get; }

        public IReadOnlyBlock Block { get; }

        /// <summary>
        /// Clone this grid.
        /// </summary>
        /// <returns></returns>
        public MapGridInfo Clone();
    }

    [System.Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public struct MapGridInfo : IReadOnlyMapGrid
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

        #region Interface Implements
        Vector2Int IReadOnlyMapGrid.Position => Position;
        string IReadOnlyMapGrid.BlockId => BlockId;
        string IReadOnlyMapGrid.ExtraContent => ExtraContent;
        #endregion

        public MapGridInfo(Vector2Int pos, string blockId, string extraContent = "")
        {
            Position = pos;
            BlockId = blockId;
            ExtraContent = extraContent;
        }

        /// <summary>
        /// The current block of this grid.
        /// </summary>
        public IReadOnlyBlock Block
        {
            get => ItemLoaderManager.GetBlock(BlockId);
            set
            {
                BlockId = value.Id;
                // TODO: 现在在修改方块时只是修改了当前格子方块ID，以后ExtraContent相关完善后需要将这部分内容告知给新的方块。
            }
        }

        public MapGridInfo Clone() => Clone(this);

        public static MapGridInfo Clone(IReadOnlyMapGrid source)
        {
            var grid = new MapGridInfo(source.Position, source.BlockId, source.ExtraContent);

            return grid;
        }
    }
}

