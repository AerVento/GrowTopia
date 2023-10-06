using UnityEngine;

namespace GrowTopia.Map.Context
{
    /// <summary>
    /// Context being passed as a parameter when a life cycle function of a base block was called.
    /// </summary>
    public struct BlockEventContext{
        public MapData Map;
        public Vector2Int CurrentPosition;
    }
}