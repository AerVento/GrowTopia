using System.Collections;
using System.Collections.Generic;
using GrowTopia.SO;
using GrowTopia.Data;
using UnityEngine;
using UnityEngine.Tilemaps;
using Newtonsoft.Json;
using GrowTopia.Map;

namespace GrowTopia.Items
{
    /// <summary>
    /// A implement class of <see cref="IReadOnlyBlock"/> , and the prototype of static block.
    /// It is a template class for all static blocks, and it is made to easily adding static blocks.
    /// "Static" means no interaction with players, no status changing, just showing textures.
    /// So this type of prototype can be used directly as <see cref="IMapBlock"/>.
    /// </summary>
    [System.Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public sealed class StaticBlock : Item, IReadOnlyBlock, IMapBlock
    {
        public BlockProperty Property;
        public BlockAttribute Attribute;
        public int Hardness;


        #region Interface Implementation
        // IReadOnlyBlock
        BlockProperty IReadOnlyBlock.Property => Property;
        BlockAttribute IReadOnlyBlock.Attribute => Attribute;
        int IReadOnlyBlock.Hardness => Hardness;

        // IMapBlock
        string IMapBlock.BlockId => Id;
        TileBase IMapBlock.Tile => GetSprite(Id)?.Tile;
        IReadOnlyBlock IMapBlock.Block => this;
        string IMapBlock.Serialize() => string.Empty;
        void IMapBlock.Deserialize(string statusContent) { }
        void IMapBlock.Start() { }
        void IMapBlock.Update() { }
        void IMapBlock.OnDestroy() { }
        IMapBlock IReadOnlyBlock.CreateInstance() => this;
        #endregion

        public new static BlockSpritePack GetSprite(string itemId)
        {

            BlockSpriteMap map = SingletonSOManager.Instance.GetSOFile<BlockSpriteMap>("BlockSpriteMap");

            if (map == null)
            {
                Debug.LogError("Cannot find Block Sprite Map.");
                return null;
            }

            return map.SpriteMap.GetValueOrDefault(itemId);
        }
    }
}

