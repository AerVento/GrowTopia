using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GrowTopia.Items;
using Newtonsoft.Json;
using UnityEngine;

namespace GrowTopia.Player
{
    [System.Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class Inventory : IEnumerable<IReadOnlyInventoryGrid>
    {
        public const int SHORTCUT_SIZE = 9;
        public const int INVENTORY_SIZE = SHORTCUT_SIZE * 3;

        private InventoryGridInfo[] _shortcut;
        private InventoryGridInfo[] _inventory;

        public Inventory()
        {
            _shortcut = new InventoryGridInfo[SHORTCUT_SIZE];
            _inventory = new InventoryGridInfo[INVENTORY_SIZE];
            for(int i = 0; i < _shortcut.Length; i++)
                _shortcut[i] = InventoryGridInfo.Empty;
            for(int i = 0; i < _inventory.Length; i++)
                _inventory[i] = InventoryGridInfo.Empty;
        }

        private IEnumerable<InventoryGridInfo> _inventoryGrids
        {
            get
            {
                foreach (var grid in _shortcut)
                    yield return grid;
                foreach (var grid in _inventory)
                    yield return grid;

            }
        }

        /// <summary>
        /// Get the item info of the shortcut grid.
        /// </summary>
        public IReadOnlyInventoryGrid GetShortcutGrid(int index)
        {
            if (index >= 0 && index < SHORTCUT_SIZE)
                return _shortcut[index];
            else
                return null;
        }

        /// <summary>
        /// Get the item info of the inventory grid.
        /// </summary>
        public IReadOnlyInventoryGrid GetInventoryGrid(int index)
        {
            if (index >= 0 && index < INVENTORY_SIZE)
                return _inventory[index];
            else
                return null;
        }

        /// <summary>
        /// Add item to inventory.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        public void AddItem(string itemId, int count)
        {
            IReadOnlyItem item = ItemLoaderManager.GetItem(itemId);
            // first, find grid in inventory with same item id, and try put it in
            var sameItemGrids = from grid in _inventoryGrids where !InventoryGridInfo.IsEmpty(grid) && grid.ItemId == itemId select grid;
            int countLeft = count;
            foreach (var grid in sameItemGrids)
            {
                int emptyPlace = grid.Item.MaxStack - grid.Count;
                if (emptyPlace > 0) // only put item when current grid has empty place 
                {
                    if (emptyPlace < countLeft) // this grid has empty places but not enough
                    {
                        grid.Count = item.MaxStack; // set grid item count to max
                        countLeft -= emptyPlace;
                    }
                    else // this grid have enough empty places to contain the item
                    {
                        grid.Count += countLeft; // add exactly equal amount of item
                        countLeft = 0;
                        break; // end the loop because all items are distributed
                    }
                }
            }

            if (countLeft != 0) // second, failed on distributing, use new empty grid to contain
            {
                var emptyGrids = from grid in _inventoryGrids where InventoryGridInfo.IsEmpty(grid) select grid;
                foreach (var grid in emptyGrids)
                {
                    grid.ItemId = item.Id;
                    if (item.MaxStack < countLeft) // this grid has empty places but not enough
                    {
                        grid.Count = item.MaxStack; // set grid item count to max
                        countLeft -= item.MaxStack;
                    }
                    else // this grid have enough empty places to contain the item
                    {
                        grid.Count += countLeft; // add exactly equal amount of item
                        countLeft = 0;
                        break; // end the loop because all items are distributed
                    }
                }

                if (countLeft != 0)
                {
                    //TODO:将这里修改成 背包满了之后物品作为掉落物出现
                    Debug.LogWarning($"Inventory full! Item Id:{itemId}, Count:{count} have dropped.");
                }
            }
        }

        /// <summary>
        /// Remove item from inventory.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        public void RemoveItem(string itemId, int count)
        {

        }

        public IEnumerator<IReadOnlyInventoryGrid> GetEnumerator()
        {
            return _inventoryGrids.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
