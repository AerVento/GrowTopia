using GrowTopia.Items;
using GrowTopia.Map.Context;
using GrowTopia.Player;
using UnityEngine.Tilemaps;

namespace GrowTopia.Map
{
    /// <summary>
    /// Abstract behaviour of a block being placed on map.
    /// Implements: 
    /// <see cref="BaseBlock"/>, dynamic block with status, it can interact with map objects.
    /// <see cref="Block"/>, prototype of a block, can be shown in map in simple way. 
    /// </summary>
    public interface IMapBlock
    {
        /// <summary>
        /// The block id of current block.
        /// </summary>
        /// <value></value>
        public string BlockId { get; }

        /// <summary>
        /// The tile used to show on the tilemap. 
        /// </summary>
        /// <value></value>
        public TileBase Tile { get; }

        /// <summary>
        /// The block prototype of current block.
        /// </summary>
        /// <value></value>
        public IReadOnlyBlock Block { get; }

        #region Serialization
        
        /// <summary>
        /// Serialize the current status of the block and turn it to a string.
        /// </summary>
        /// <returns>The status content.</returns>
        public string Serialize();

        /// <summary>
        /// Deserialize the status content and transform the block to the right status.
        /// </summary>
        /// <param name="statusContent"></param>
        public void Deserialize(string statusContent);
        
        #endregion

        #region Life Cycle Function
        
        /// <summary>
        /// Called when the block is being placed.
        /// </summary>
        /// <param name="context"></param>
        public void Start();

        /// <summary>
        /// Called every game logic update cycle. 
        /// </summary>
        /// <param name="context"></param>
        public void Update();

        /// <summary>
        /// Called when the block is being destroyed.
        /// </summary>
        /// <param name="context"></param>
        public void OnDestroy();

        #endregion
        



        /// <summary>
        /// If the two map block has the same block.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool CompareBlock(IMapBlock left, IMapBlock right)
        {
            if (left == null || right == null)
                return left.Equals(right);

            return left.BlockId == right.BlockId;
        }
    }
}