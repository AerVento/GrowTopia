using UnityEngine;
using UnityEngine.Tilemaps;

namespace GrowTopia.Map
{
    public enum TilemapLayer
    {
        Background,
        Solid,
        Liquid,
        Platforms,
        Foreground,
    }
    public class TilemapSet
    {
        public Tilemap Background;
        public Tilemap Solid;
        public Tilemap Liquid;
        public Tilemap Platforms;
        public Tilemap Foreground;

        public bool Valid => Background && Solid && Liquid && Platforms && Foreground;

        /// <summary>
        /// Load layer from a given grid component.
        /// As default, all tilemap are staged under the grid component.
        /// </summary>
        /// <param name="grid"></param>
        public void LoadLayer(Grid grid)
        {
            Background = grid.transform.Find("Background")?.GetComponent<Tilemap>();
            Solid = grid.transform.Find("Solid")?.GetComponent<Tilemap>();
            Liquid = grid.transform.Find("Liquid")?.GetComponent<Tilemap>();
            Platforms = grid.transform.Find("Platforms")?.GetComponent<Tilemap>();
            Foreground = grid.transform.Find("Foreground")?.GetComponent<Tilemap>();

            if (!Valid)
            {
                Debug.Log("Missing one of the tilemaps layer.");
            }
        }

        public Tilemap GetMapLayer(TilemapLayer layer)
        {
            var map = layer switch
            {
                TilemapLayer.Background => Background,
                TilemapLayer.Solid => Solid,
                TilemapLayer.Liquid => Liquid,
                TilemapLayer.Platforms => Platforms,
                TilemapLayer.Foreground => Foreground,
                _ => null
            };

            if(map == null){
                Debug.LogError($"Access an unknown tilemap layer: {layer}. Make sure it was supported and was added to the scene." );
            }

            return map;
        }
    }
}

