using UnityEngine.Tilemaps;

namespace GrowTopia.Map
{
    class TilemapSet
    {
        public Tilemap Background;
        public Tilemap Solid;
        public Tilemap Liquid;
        public Tilemap Platform;
        public Tilemap Foreground;

        public bool Valid => Background && Solid && Liquid && Platform && Foreground;
    }
}

