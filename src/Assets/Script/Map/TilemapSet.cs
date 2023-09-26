using UnityEngine.Tilemaps;

namespace GrowTopia.Map
{
    class TilemapSet
    {
        public Tilemap Background;
        public Tilemap Solid;
        public Tilemap Liquid;
        public Tilemap Platforms;
        public Tilemap Foreground;

        public bool Valid => Background && Solid && Liquid && Platforms && Foreground;
    }
}

