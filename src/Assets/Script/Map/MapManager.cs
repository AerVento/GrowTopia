using System.Collections;
using System.Collections.Generic;
using Framework.Singleton;
using GrowTopia.Items;
using Newtonsoft.Json;
using UnityEngine;

namespace GrowTopia.Map{
    public class MapManager : MonoSingleton<MapManager>
    {
        void Start(){
            ItemData data = new ItemData();
            data.AvailableItemList.Add(new Item(){ Id = "dev", Name = "Dev_Name", Description = "Test Item"});
            data.AvailableItemList.Add(new Block(){ Id = "dirt", Name = "Dirt", Description = "This is dirt." });
            data.AvailableItemList.Add(new Block(){ Id = "lava", Name = "Lava", Description = "This is lava." });
            data.AvailableItemList.Add(new Block(){ Id = "stone", Name = "Stone", Description = "This is stone." });
            JsonSerializerSettings settings = new JsonSerializerSettings(){ TypeNameHandling = TypeNameHandling.Auto};
            Debug.Log(JsonConvert.SerializeObject(data,settings));
        }
        public void Load(MapData map){

        }
    }
}

