using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace GrowTopia.Serialization
{
    public class ItemSerializationSettings : JsonSerializerSettings {
        public ItemSerializationSettings(){
            TypeNameHandling = TypeNameHandling.Auto;
            SerializationBinder = new ItemSerializationBinder();
        }
    }
}

