using System.Collections.Generic;
using Framework.Serialization;
using GrowTopia.Map;
using Newtonsoft.Json;
using UnityEngine;

namespace GrowTopia.Serialization
{
    public class MapSerializationSettings : JsonSerializerSettings
    {
        public MapSerializationSettings()
        {
            Formatting = Formatting.Indented;
            Converters = new List<JsonConverter>()
            {
                new Vector2IntConverter(),
                new DictionarySerializer<Vector2Int, MapGridInfo>(),
                new MapBlockSerializer()
            };
        }
    }
}