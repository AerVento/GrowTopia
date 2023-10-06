using GrowTopia.Map;
using Newtonsoft.Json;
using UnityEngine.Tilemaps;

namespace GrowTopia.Items
{
    /// <summary>
    /// A implement class of <see cref="IReadOnlyBlock"/>. 
    /// It is the base class of all dynamic blocks.
    /// "Dynamic" means it can interact with players, it can change status, and change the texture.
    /// Inherit this class to make custom game logic of dynamic block.
    /// </summary>
    [System.Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public abstract class DynamicBlock : Item, IReadOnlyBlock
    {
        /// <summary>
        /// The initial property. It was used to make prototype setting.
        /// </summary>
        protected BlockProperty _initProperty = BlockProperty.Solid;
        /// <summary>
        /// The initial attribute. It was used to make prototype setting.
        /// </summary>
        protected BlockAttribute _initAttribute = BlockAttribute.Default;
        /// <summary>
        /// The initial hardness. It was used to make prototype setting.
        /// </summary>
        protected int _initHardness = 1000;

        // IReadOnlyBlock
        public virtual BlockProperty Property => _initProperty;
        public virtual BlockAttribute Attribute => _initAttribute;
        public virtual int Hardness => _initHardness;

        public abstract IMapBlock CreateInstance();
    }
}