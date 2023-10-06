using UnityEngine;
using GrowTopia.Map;
using UnityEngine.Tilemaps;

namespace GrowTopia.Items
{
    public class Container : DynamicBlock, IMapBlock
    {
        public string BlockId => Id;

        public TileBase Tile => StaticBlock.GetSprite(BlockId)?.Tile;

        public IReadOnlyBlock Block => this;

        public string Serialize()
        {
            return "I'm a container that contains 99 diamonds!";
        }
        public void Deserialize(string statusContent)
        {
            Debug.Log("Deserialize:" + statusContent);
        }

        public void Start()
        {

        }

        public void Update()
        {

        }
        
        public void OnDestroy()
        {

        }

        public override IMapBlock CreateInstance()
        {
            return this;
        }
    }
}