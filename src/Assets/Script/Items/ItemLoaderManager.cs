using System.Collections;
using System.Collections.Generic;
using System.IO;
using Framework.Singleton;
using Newtonsoft.Json;
using UnityEngine;

namespace GrowTopia.Items{
    /// <summary>
    /// A manager to load current available items and apply it to the game.
    /// </summary>
    public class ItemLoaderManager : Singleton<ItemLoaderManager>
    {
        private Dictionary<string,Item> _data = new ();
        
        protected ItemLoaderManager(){
            LoadData();
        }

        private void LoadData(){
            // For test, load the data from disk.
            string content = File.ReadAllText("ItemData.json");
            JsonSerializerSettings settings = new JsonSerializerSettings(){
                 TypeNameHandling = TypeNameHandling.Auto
            };
            ItemData data = JsonConvert.DeserializeObject<ItemData>(content,settings);

            foreach(var item in data.AvailableItemList){
                _data.Add(item.Id, item);
            }
        }

        public Item GetItem(string itemId){
            if(_data.TryGetValue(itemId, out var item)){
                Item itemCopy = new Item();

                itemCopy.Id = item.Id;
                itemCopy.Name = item.Name;
                itemCopy.Description = item.Description;

                return itemCopy;
            }
            return null;
        }
    }
}

