using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Framework.Singleton;
using GrowTopia.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace GrowTopia.Items
{
    /// <summary>
    /// A manager to load current available items and apply it to the game.
    /// </summary>
    public class ItemLoaderManager : Singleton<ItemLoaderManager>
    {
        private Dictionary<string, IReadOnlyItem> _data;

        /// <summary>
        /// All available items.
        /// </summary>
        public IReadOnlyDictionary<string, IReadOnlyItem> Items
        {
            get
            {
                if (_data == null)
                    LoadData();
                return _data;
            }
        }

        private void LoadData()
        {
            _data = new();
            // For test, load the data from disk.
            string content = File.ReadAllText("Assets/Script/Items/ItemData.json");

            ItemData data = JsonConvert.DeserializeObject<ItemData>(content, new ItemSerializationSettings());

            foreach (var item in data.AvailableItemList)
            {
                _data.Add(item.Id, item);
                Debug.Log(item.Id + "," + item.GetType());
            }
        }

        public static IReadOnlyItem GetItem(string ItemId)
        {
            IReadOnlyItem item = Instance.Items.GetValueOrDefault(ItemId);
            if (item == null)
            {
                Debug.LogError("Unknown item id:" + ItemId);
                return null;
            }
            return item;
        }

        public static IReadOnlyBlock GetBlock(string BlockId)
        {
            IReadOnlyItem item = Instance.Items.GetValueOrDefault(BlockId);
            if (item == null)
            {
                Debug.LogError("Unknown block id:" + BlockId);
                return null;
            }
            IReadOnlyBlock block = item as IReadOnlyBlock;
            if (block == null)
            {
                Debug.LogError($"Id:{BlockId} is not a block but an item.");
                return null;
            }
            return block;
        }
    }
}

