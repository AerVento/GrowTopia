using GrowTopia.Map;
using UnityEngine.Tilemaps;

namespace GrowTopia.Items
{
    /// <summary>
    /// An interface to access the prototype of blocks. It describes inherent property of a block.
    /// The instance was created by <see cref="ItemLoaderManager"/>.
    /// </summary>
    public interface IReadOnlyBlock : IReadOnlyItem
    {
        /// <summary>
        /// The block property of the block.
        /// </summary>
        /// <value></value>
        public BlockProperty Property { get; }

        /// <summary>
        /// The block attribute of the block. Normally, default.
        /// </summary>
        /// <value></value>
        public BlockAttribute Attribute { get; }

        /// <summary>
        /// To define the needed time to break the block.
        /// </summary>
        /// <value>Milliseconds needed to break the block.</value>
        public int Hardness { get; }

        /// <summary>
        /// Use this prototype to create a new instance of a map block.
        /// The map loader uses it to create instance and show it in the map.
        /// It controls it's behaviour of transforming into <see cref="IMapBlock"/>.
        /// </summary>
        /// <returns></returns>
        public IMapBlock CreateInstance();
    }
}