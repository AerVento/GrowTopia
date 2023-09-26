using System.Collections.Generic;
using System.Threading;
using GrowTopia.Items;
using Newtonsoft.Json;
using UnityEngine;

namespace GrowTopia.Player
{
    public interface IReadOnlyInventoryGrid
    {
        public string ItemId { get; }
        public int Count { get; }
        public IReadOnlyItem Item { get; }
    }

    [System.Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class InventoryGridInfo : IReadOnlyInventoryGrid
    {
        public static InventoryGridInfo Empty => new InventoryGridInfo()
        {
            ItemId = "",
            Count = 0
        };

        public string ItemId;
        public int Count;

        #region Interface Implements
        string IReadOnlyInventoryGrid.ItemId => ItemId;
        int IReadOnlyInventoryGrid.Count => Count;
        #endregion

        public IReadOnlyItem Item => ItemLoaderManager.GetItem(ItemId);

        public static bool IsEmpty(IReadOnlyInventoryGrid value)
        {
            return value.ItemId == "" && value.Count == 0;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
