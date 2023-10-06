using GrowTopia.Items;

namespace GrowTopia.Player
{
    public interface IReadOnlyInventoryGrid
    {
        public string ItemId { get; }
        public int Count { get; }
        public InventoryGridType Type { get; }
        public IReadOnlyItem Item { get; }

        /// <summary>
        /// Check if is a empty grid.
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty();

        /// <summary>
        /// Create a new grid with the exactly same values of the source grid.
        /// </summary>
        /// <param name="source"></param>
        public InventoryGridInfo Clone();
    }
}
