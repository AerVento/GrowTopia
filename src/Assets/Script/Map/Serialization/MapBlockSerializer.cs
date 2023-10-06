using System;
using System.Collections.Generic;
using System.Reflection;
using GrowTopia.Items;
using GrowTopia.Map;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace GrowTopia.Serialization
{
    public class MapBlockSerializer : JsonConverter<IMapBlock>
    {
        public override IMapBlock ReadJson(JsonReader reader, Type objectType, IMapBlock existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject obj = JObject.Load(reader);
            
            string id = obj.GetValue("id").Value<string>();
            IReadOnlyBlock blockPrototype = ItemLoaderManager.GetBlock(id);
            

            IMapBlock block = blockPrototype.CreateInstance();
            string content = obj.GetValue("content").Value<string>();
            block.Deserialize(content);
            
            return block;
        }

        public override void WriteJson(JsonWriter writer, IMapBlock value, JsonSerializer serializer)
        {
            JObject obj = new JObject()
            {
                { "id", value.BlockId },
                { "content", value.Serialize() }
            };

            obj.WriteTo(writer);
        }
    }
}