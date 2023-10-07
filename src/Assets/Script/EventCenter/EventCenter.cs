using System;
using GrowTopia.Map.Context;
using GrowTopia.Player.Context;

namespace GrowTopia.Events
{
    /// <summary>
    /// Center class for listener of all modules.
    /// </summary>
    public static class EventCenter
    {
        #region Map Module
        /// <summary>
        /// Called when a block was created on map.
        /// </summary>
        /// <returns></returns>
        public static Event<Action<MapChangedContext>> OnBlockCreated = new ();
        /// <summary>
        /// Called when a block on map was changed.
        /// </summary>
        /// <returns></returns>
        public static Event<Action<MapChangedContext>> OnBlockChanged = new ();
        /// <summary>
        /// Called when a block on map was destroyed.
        /// </summary>
        /// <returns></returns>
        public static Event<Action<MapChangedContext>> OnBlockDestroyed = new ();
        #endregion 

        #region Player Input
        /// <summary>
        /// Called when player placed a block, which was after the block created on map.
        /// </summary>
        /// <returns></returns>
        public static Event<Action<PlayerPlaceBlockContext>> OnPlayerPlaceBlock = new ();
        /// <summary>
        /// Called when player start the behaviour of destroying block.
        /// </summary>
        /// <returns></returns>
        public static Event<Action<PlayerDestroyBlockContext>> OnPlayerStartDestroyBlock = new ();
        /// <summary>
        /// Called when player continued to destroy the block.
        /// </summary>
        /// <returns></returns>
        public static Event<Action<(PlayerDestroyBlockContext context, float progress)>> OnPlayerContinueDestroyingBlock = new ();
        /// <summary>
        /// Called when the player ended the destroying behaviour.
        /// </summary>
        /// <returns></returns>
        public static Event<Action<(PlayerDestroyBlockContext context, bool success)>> OnPlayerEndDestroyBlock = new ();
        #endregion
    }
}