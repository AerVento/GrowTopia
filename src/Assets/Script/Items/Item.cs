using System.Collections;
using System.Collections.Generic;
using GrowTopia.SO;
using GrowTopia.Data;
using UnityEngine;
using Newtonsoft.Json;

namespace GrowTopia.Items
{



    /// <summary>
    /// Prototype of items. It describes inherent property of an item.
    /// </summary>
    [System.Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class Item : IReadOnlyItem
    {
        public string Id;
        public string Name;
        public string Description;

        public int MaxStack;

        public Sprite Sprite => GetSprite(Id).InventorySprite;

        #region Interface Implementation
        string IReadOnlyItem.Id => Id;
        string IReadOnlyItem.Name => Name;
        string IReadOnlyItem.Description => Description;
        int IReadOnlyItem.MaxStack => MaxStack;
        #endregion

        public static ItemSpritePack GetSprite(string itemId)
        {

            ItemSpriteMap map = SingletonSOManager.Instance.GetSOFile<ItemSpriteMap>("ItemSpriteMap");

            if (map == null)
            {
                Debug.LogError("Cannot find Item Sprite Map.");
                return null;
            }

            return map.SpriteMap.GetValueOrDefault(itemId);
        }
    }
}

