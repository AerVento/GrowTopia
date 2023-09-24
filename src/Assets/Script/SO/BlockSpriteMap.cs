using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GrowTopia.Data{
    
    [CreateAssetMenu( fileName = "New Block Sprite Map", menuName = "SO/Sprite/Block Sprite Map")]
    public class BlockSpriteMap : ScriptableObject
    {
        [SerializeField]
        private SerializedDictionary<string,BlockSpritePack> _spriteMap = new SerializedDictionary<string, BlockSpritePack>();

        public IReadOnlyDictionary<string,BlockSpritePack> SpriteMap => _spriteMap;
    }

    [System.Serializable]
    public class BlockSpritePack{
        public string ItemId;
        public TileBase Tile;
    }
}

