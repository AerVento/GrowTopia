using System.Collections;
using System.Collections.Generic;
using GrowTopia.SO;
using GrowTopia.Data;
using UnityEngine;
using UnityEngine.Tilemaps;
using Newtonsoft.Json;

namespace GrowTopia.Items{

    public interface IReadOnlyBlock : IReadOnlyItem {
        public BlockProperty Property {get;}
        public BlockAttribute Attribute {get;}
    }


    [System.Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class Block : Item, IReadOnlyBlock
    {
        public BlockProperty Property;
        public BlockAttribute Attribute;

        public TileBase Tile => GetSprite(Id).Tile;

        #region Interface Implementation
        BlockProperty IReadOnlyBlock.Property => Property;
        BlockAttribute IReadOnlyBlock.Attribute => Attribute;
        #endregion

        public new static BlockSpritePack GetSprite(string itemId){
            
            BlockSpriteMap map = SingletonSOManager.Instance.GetSOFile<BlockSpriteMap>("BlockSpriteMap");

            if(map == null)
            {
                Debug.LogError("Cannot find Block Sprite Map.");
                return null;
            }

            return map.SpriteMap.GetValueOrDefault(itemId);
        }
    }
}
