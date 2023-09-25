using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace GrowTopia.Serialization{
    public class ItemSerializationBinder : ISerializationBinder
    {
        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = serializedType.Assembly.FullName;
            typeName = serializedType.Name;
        }

        public Type BindToType(string assemblyName, string typeName)
        {
            return Assembly.GetExecutingAssembly().GetType(typeName);
        }
    }

    public class ItemSerializationSettings : JsonSerializerSettings {
        public ItemSerializationSettings(){
            TypeNameHandling = TypeNameHandling.Auto;
            SerializationBinder = new ItemSerializationBinder();
        }
    }
}

