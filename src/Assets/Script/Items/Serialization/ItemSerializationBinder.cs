using System;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace GrowTopia.Serialization
{
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
}

