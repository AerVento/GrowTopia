using System.Collections.Generic;
using System.Threading;
using GrowTopia.Items;
using Newtonsoft.Json;
using UnityEngine;

namespace GrowTopia.Player
{
    [System.Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class InventoryGridInfo : IReadOnlyInventoryGrid
    {
        /// <summary>
        /// A read only grid presents empty grid.
        /// </summary>
        public static InventoryGridInfo Empty => new InventoryGridInfo()
        {
            ItemId = "",
            Count = 0
        };

        public string ItemId;
        public int Count;
        public InventoryGridType Type = InventoryGridType.None;


        #region Interface Implements
        string IReadOnlyInventoryGrid.ItemId => ItemId;

        int IReadOnlyInventoryGrid.Count => Count;

        InventoryGridType IReadOnlyInventoryGrid.Type => Type;

        public IReadOnlyItem Item
        {
            get
            {
                if (IsEmpty())
                    throw new System.InvalidOperationException("Trying access the item Property with a empty inventory grid.");
                else
                    return ItemLoaderManager.GetItem(ItemId);
            }
        }

        public bool IsEmpty() => IsEmpty(this);

        public InventoryGridInfo Clone() => Clone(this);

        #endregion


        /// <summary>
        /// Read the values of the source grid and apply it to this one.
        /// </summary>
        /// <param name="source"></param>
        public void Copy(InventoryGridInfo source) => Copy(source, this);

        /// <summary>
        /// Set this grid to empty.
        /// </summary>
        public void Clear()
        {
            var empty = Empty;
            ItemId = empty.ItemId;
            Count = empty.Count;
            // we don't want the grid lose its type while Clear();
        }

        public static bool IsEmpty(IReadOnlyInventoryGrid value)
        {
            return value != null && value.ItemId == "" && value.Count == 0;
        }

        public static void Copy(IReadOnlyInventoryGrid source, InventoryGridInfo destination)
        {
            destination.ItemId = source.ItemId;
            destination.Count = source.Count;
            destination.Type = source.Type;
        }

        public static InventoryGridInfo Clone(IReadOnlyInventoryGrid source)
        {
            var grid = new InventoryGridInfo();
            Copy(source, grid);
            return grid;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

    }
}
