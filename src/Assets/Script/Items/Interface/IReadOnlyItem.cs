using UnityEngine;

namespace GrowTopia.Items
{
    /// <summary>
    /// An interface to access the prototype of items. It describes inherent property of a item.
    /// The instance was created by <see cref="ItemLoaderManager"/>.
    /// </summary>
    public interface IReadOnlyItem
    {
        /// <summary>
        /// The id of the item.
        /// </summary>
        /// <value></value>
        public string Id { get; }

        /// <summary>
        /// The name of the item.
        /// </summary>
        /// <value></value>
        public string Name { get; }

        /// <summary>
        /// The description of the item.
        /// </summary>
        /// <value></value>
        public string Description { get; }

        /// <summary>
        /// The maximum stack that this item can held by a inventory grid.
        /// </summary>
        /// <value></value>
        public int MaxStack { get; }

        /// <summary>
        /// The sprite to show the item. 
        /// </summary>
        /// <value></value>
        public Sprite Sprite { get; }
    }
}