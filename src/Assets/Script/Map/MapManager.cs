using System.Collections;
using System.Collections.Generic;
using Framework.Singleton;
using GrowTopia.Items;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GrowTopia.Map
{
    /// <summary>
    /// The controller of map.
    /// </summary>
    public class MapManager : MonoSingleton<MapManager>
    {
        // Components
        [SerializeField]
        private Grid _grid; // The grid component where all tilemaps attached to.

        private MapDataHandler _currentMapHandler = new MapDataHandler();

        private TilemapSet _tilemapSet = new TilemapSet();

        /// <summary>
        /// Current map data.
        /// </summary>
        public MapData CurrentMap => _currentMapHandler.Target;

        void Start()
        {
            // get each tilemap layer
            _tilemapSet.LoadLayer(_grid);

            // load map data from extern
            _currentMapHandler.Load();

            // apply these map data to the tilemap
            Load();

            CreateBlock(new Vector2Int(3,8), ItemLoaderManager.GetBlock("stone"));
        }

        // set the tilemap tile
        private void SetTile(Vector2Int position, IReadOnlyBlock block)
        {
            TilemapLayer affectedLayer;
            // Distribute the grid info to the right map layer
            if (block.Attribute == BlockAttribute.Platform)
                affectedLayer = TilemapLayer.Platforms;
            else
            {
                affectedLayer = block.Property switch
                {
                    BlockProperty.Solid => TilemapLayer.Solid,
                    BlockProperty.Liquid => TilemapLayer.Liquid,
                    _ => default
                };
            }
            // set tilemap tile
            _tilemapSet.GetMapLayer(affectedLayer)?.SetTile(
                position: new Vector3Int(position.x, position.y),
                tile: block == null ? null : block.Tile
                );
        }

        /// <summary>
        /// Apply the replacement to map data, and set tile on tilemap layers.
        /// </summary>
        /// <param name="position">The replacement position.</param> 
        /// <param name="grid">The new grid to replace. Null to set the position empty.</param>
        private void ReplaceBlockInternal(Vector2Int position, IReadOnlyMapGrid grid)
        {
            // set the map data 
            CurrentMap.SetGrid(grid.Position, grid);

            // set the tile
            SetTile(position, grid == null ? null : grid.Block);
        }


        /// <summary>
        /// Create a block on specified location. If the location already have a block, it will be replaced.
        /// </summary>
        /// <param name="position">The target position.</param>
        /// <param name="blockType">The block to create. </param>
        public void CreateBlock(Vector2Int position, IReadOnlyBlock blockType)
        {
            if (blockType == null)
                DestroyBlock(position);
            else
                ReplaceBlockInternal(position, new MapGridInfo(position, blockType.Id));
        }

        /// <summary>
        /// Destroy a block on specified location. If the location is empty, it will do nothing.
        /// </summary>
        /// <param name="position"></param>
        public void DestroyBlock(Vector2Int position)
        {
            ReplaceBlockInternal(position, null);
        }

        /// <summary>
        /// Load the map data and set tile for each entry.
        /// </summary>
        public void Load()
        {
            foreach (var (position, grid) in _currentMapHandler.Target)
            {
                IReadOnlyBlock block = grid.Block;
                SetTile(position, block);
            }
        }
    }
}

