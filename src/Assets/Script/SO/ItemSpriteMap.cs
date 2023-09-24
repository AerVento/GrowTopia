using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace GrowTopia.Data{

    [CreateAssetMenu( fileName = "New Item Sprite Map", menuName = "SO/Sprite/Item Sprite Map")]    
    public class ItemSpriteMap : ScriptableObject
    {
        [SerializeField]
        private SerializedDictionary<string,ItemSpritePack> _spriteMap = new SerializedDictionary<string, ItemSpritePack>();

        public IReadOnlyDictionary<string,ItemSpritePack> SpriteMap => _spriteMap;
    }

    [System.Serializable]
    public class ItemSpritePack{
        public string ItemId;
        public Sprite InventorySprite;
    }
}
