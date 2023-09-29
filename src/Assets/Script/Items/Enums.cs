namespace GrowTopia.Items{
    
    /// <summary>
    /// The property of Block, defined whether the block can be passed through by player.
    /// </summary>
    public enum BlockProperty{
        /// <summary>
        /// The block is Solid, and the player CANNOT go through it.
        /// </summary>
        Solid,
        /// <summary>
        /// The block is Liquid, and the player CAN go through it.
        /// The <see cref="IReadOnlyBlock.Hardness"/> will be ignored when setting a block to liquid.
        /// </summary>
        Liquid,
    }
    
    /// <summary>
    /// The attribute of block, defined the special behaviour of the block.
    /// </summary>
    public enum BlockAttribute{
        
        /// <summary>
        /// Default Block, and the player won't interact with it.
        /// </summary>
        Default,

        /// <summary>
        /// Platform Block, means the player can jump up to the platform and won't fall from the platform.
        /// NOTICE: Setting a block to Platform will ignore <see cref="IReadOnlyBlock.Property"/> and treat it as Solid.
        /// </summary>
        Platform,
    }

}