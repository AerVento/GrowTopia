using GrowTopia.Data;
using GrowTopia.Events;
using GrowTopia.Player.Context;
using GrowTopia.SO;
using UnityEngine;

namespace GrowTopia.Map
{
    /// <summary>
    /// A static class to control special graphic on the map, such as animation of breaking a block.
    /// </summary>
    public static partial class MapGraphics
    {
        public readonly static GameObject Graphics = new GameObject("Graphics");
        /// <summary>
        /// Start showing graphic.
        /// </summary>
        public static void StartGraphic()
        {
            BreakingBlockGraphic.Start();
        }

        /// <summary>
        /// Stop showing graphic.
        /// </summary>
        public static void StopGraphic()
        {
            BreakingBlockGraphic.Stop();
        }

    }
}