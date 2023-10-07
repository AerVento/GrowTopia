using UnityEngine;

namespace GrowTopia.Player.Context
{
    public struct PlayerDestroyBlockContext
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
        
        /// <summary>
        /// Total time needed to destroy the block.
        /// </summary>
        /// <value></value>
        public float TotalTime {get;private set;}

        public PlayerDestroyBlockContext(IPlayer player, Vector2Int blockPos, float totalTime)
        {
            Player = player;
            BlockPosition = blockPos;
            TotalTime = totalTime;
        }
    }
}