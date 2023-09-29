using System.Collections.Generic;
using System.IO;
using Framework.Serialization;
using Newtonsoft.Json;
using UnityEngine;

namespace GrowTopia.Map
{
    public class MapDataHandler
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            Converters = new List<JsonConverter>() 
            { 
                new Vector2IntConverter(),
                new DictionarySerializer<Vector2Int, MapGridInfo>()
            }
        };

        private static System.Text.Encoding Encoding => System.Text.Encoding.UTF8;

        private MapData _target;
        public MapData Target
        {
            get
            {
                if (_target == null)
                    Load();
                return _target;
            }
        }

        public bool Load()
        {
            Debug.Log("Loading map...");
            try
            {
                // For test, read inventory from disk.
                // TODO: Get map from server.
                string content = File.ReadAllText("Assets/Script/Map/MapData.json");
                _target = JsonConvert.DeserializeObject<MapData>(content, Settings);
                _target.OnMapChanged += context => Save();
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                _target = new MapData();
                return false;
            }
        }

        public bool Save()
        {
            Debug.Log("Saving map...");
            try
            {
                // For test, save inventory to disk.
                // TODO: Upload map to server.
                string content = JsonConvert.SerializeObject(_target, Settings);
                File.WriteAllText("Assets/Script/Map/MapData.json", content, Encoding);
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }
    }
}