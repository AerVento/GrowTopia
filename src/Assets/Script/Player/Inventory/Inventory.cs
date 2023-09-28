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

        [JsonIgnore] // ignore the serialization of delegate
        private Action<InventoryChangedContext> _onInventoryChanged;

        public Inventory()
        {
            _shortcut = new InventoryGridInfo[SHORTCUT_SIZE];
            _inventory = new InventoryGridInfo[INVENTORY_SIZE];
            for (int i = 0; i < _shortcut.Length; i++)
            {
                _shortcut[i] = InventoryGridInfo.Empty;
                _shortcut[i].Type = InventoryGridType.Shortcut;
            }
            for (int i = 0; i < _inventory.Length; i++)
            {
                _inventory[i] = InventoryGridInfo.Empty;
                _inventory[i].Type = InventoryGridType.Inventory;
            }
        }

        /// <summary>
        /// All inventory grids, including shortcut and real inventory.
        /// </summary>
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
        /// Called when any inventory grid changed.
        /// </summary>
        public event Action<InventoryChangedContext> OnInventoryChanged
        {
            add => _onInventoryChanged += value;
            remove => _onInventoryChanged -= value;
        }

        public IEnumerable<InventoryGridInfo> ShortcutGrids => _shortcut;

        public IEnumerable<InventoryGridInfo> InventoryGrids => _inventory;

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
            // get the item
            IReadOnlyItem item = ItemLoaderManager.GetItem(itemId);

            // first, find grid in inventory with same item id, and try put it in
            var sameItemGrids = from grid in _inventoryGrids where !grid.IsEmpty() && grid.ItemId == itemId select grid;

            // create context for potential changes
            InventoryChangedContext context = new InventoryChangedContext();

            int countLeft = count;
            foreach (var grid in sameItemGrids)
            {
                int emptyPlace = grid.Item.MaxStack - grid.Count;
                if (emptyPlace > 0) // only put item when current grid has empty place 
                {
                    InventoryGridInfo oldGrid = grid.Clone();

                    if (emptyPlace < countLeft) // this grid has empty places but not enough
                    {
                        grid.Count = item.MaxStack; // set grid item count to max
                        countLeft -= emptyPlace;
                    }
                    else // this grid have enough empty places to contain the item
                    {
                        grid.Count += countLeft; // add exactly equal amount of item
                        countLeft = 0;
                    }

                    InventoryGridInfo newGrid = grid.Clone();
                    context.AddEntry(oldGrid, newGrid);
                }
                // end the loop because all items are distributed
                if (countLeft == 0)
                    break;
            }

            // second, if failed on distributing, use new empty grid to contain
            if (countLeft != 0)
            {
                var emptyGrids = from grid in _inventoryGrids where grid.IsEmpty() select grid;
                foreach (var grid in emptyGrids)
                {
                    InventoryGridInfo oldGrid = grid.Clone();

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

                    InventoryGridInfo newGrid = grid.Clone();
                    context.AddEntry(oldGrid, newGrid);
                }

                if (countLeft != 0)
                {
                    //TODO:将这里修改成 背包满了之后物品作为掉落物出现
                    Debug.LogWarning($"Inventory full! Item Id:{itemId}, Count:{countLeft} have dropped.");
                }
            }

            // trigger the event
            if (context.Count != 0) { }
            _onInventoryChanged?.Invoke(context);
        }

        private bool CalculateRemoveOperations(string itemId, int count,
            out IEnumerable<(InventoryGridInfo Target, int RemoveCount)> operations)
        {
            // find grid not empty and with same item id 
            var sameItemGrids = from grid in _inventoryGrids where !grid.IsEmpty() && grid.ItemId == itemId select grid;

            // test the remove operations
            var removeOperations = new List<(
                InventoryGridInfo Target, // the grid to remove
                int removeCount // the item amount to remove
            )>();

            int countLeft = count;
            foreach (var grid in sameItemGrids)
            {
                if (grid.Count < countLeft) // this grid is not enough, remove all
                {
                    countLeft -= grid.Count;
                    removeOperations.Add((grid, grid.Count));
                }
                else // this grid is already enough, only remove the needs
                {
                    removeOperations.Add((grid, countLeft));
                    countLeft = 0;
                    break;
                }
            }

            operations = removeOperations;
            return countLeft == 0; // true means the amount is enough and it can be removed
        }

        /// <summary>
        /// Remove item from inventory when the item amount in inventory is enough..
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        /// <returns>True if successfully removed, false if the item amount is not enough.</returns>
        public bool RemoveItem(string itemId, int count)
        {
            if (CalculateRemoveOperations(itemId, count, out var operations))
            {
                InventoryChangedContext context = new InventoryChangedContext();

                // started real removing 
                foreach (var op in operations)
                {
                    InventoryGridInfo oldGrid = op.Target.Clone();

                    if (op.Target.Count == op.RemoveCount)
                        op.Target.Clear(); // set this grid to empty
                    else
                        op.Target.Count -= op.RemoveCount;

                    InventoryGridInfo newGrid = op.Target.Clone();
                    context.AddEntry(oldGrid, newGrid);
                }

                if (context.Count != 0) { }
                _onInventoryChanged?.Invoke(context);

                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// If the inventory contains such amount items.
        /// </summary>
        /// <param name="itemId"></param>
        /// <param name="count"></param>
        /// <returns>True if inventory contains this item and enough amount.</returns>
        public bool Contains(string itemId, int count)
        {
            return CalculateRemoveOperations(itemId, count, out _);
        }


        public IEnumerator<IReadOnlyInventoryGrid> GetEnumerator()
        {
            return _inventoryGrids.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
