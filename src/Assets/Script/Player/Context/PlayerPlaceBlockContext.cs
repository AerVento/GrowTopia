using UnityEngine;

namespace GrowTopia.Player.Context
{
    public struct PlayerPlaceBlockContext
    {
        /// <summary>
        /// The player that triggered this event.
        /// </summary>
        /// <value></value>
        public IPlayer Player { get; private set; }
        
        /// <summary>
        /// The position of the block which player affected.
        /// </summary>
        public Vector2Int BlockPosition { get; private set; }

        public PlayerPlaceBlockContext(IPlayer player, Vector2Int blockPos)
        {
            Player = player;
            BlockPosition = blockPos;
        }
    }
}