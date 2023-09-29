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
            DrawMapBoarder();

            CreateBlock(new Vector2Int(3, 8), ItemLoaderManager.GetBlock("stone"));
        }

        // get the right layer for the block
        private TilemapLayer GetLayerOfBlock(IReadOnlyBlock block)
        {
            TilemapLayer affectedLayer;
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

            return affectedLayer;
        }

        /// <summary>
        /// Apply the replacement to map data, and set tile on tilemap layers.
        /// </summary>
        /// <param name="position">The replacement position.</param> 
        /// <param name="grid">The new grid to replace. Null to set the position empty.</param>
        private void ReplaceBlockInternal(Vector2Int position, IReadOnlyMapGrid grid)
        {
            IReadOnlyBlock block = grid.Block;

            // set the map data 
            CurrentMap.SetGrid(position, grid);

            // Distribute the grid info to the right map layer
            TilemapLayer layer = GetLayerOfBlock(block);

            // set the tile
            _tilemapSet.GetMapLayer(layer)?.SetTile(
                position: new Vector3Int(position.x, position.y),
                tile: grid.Block.Tile
                );
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
            if (_currentMapHandler.Target.TryGetGrid(position, out var grid))
            {
                TilemapLayer layer = GetLayerOfBlock(grid.Block);
                _tilemapSet.GetMapLayer(layer).SetTile(
                    new Vector3Int(position.x, position.y), tile:null
                );

                _currentMapHandler.Target.SetGrid(position, null);
            }
        }

        /// <summary>
        /// Transform a world position to grid position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector2Int WorldToGrid(Vector3 position)
        {
            Vector3Int pos = _grid.WorldToCell(position);
            return new Vector2Int(pos.x, pos.y);
        }

        /// <summary>
        /// Load the map data and set tile for each entry.
        /// </summary>
        public void Load()
        {
            foreach (var (position, grid) in _currentMapHandler.Target)
            {
                IReadOnlyBlock block = grid.Block;
                _tilemapSet.GetMapLayer(
                    layer:GetLayerOfBlock(block)
                    ).SetTile(new Vector3Int(position.x, position.y), block.Tile);
            }
        }

        private void DrawMapBoarder()
        {
            Vector3 leftBottom = _grid.CellToWorld(Vector3Int.zero);
            Vector3 leftTop = _grid.CellToWorld(Vector3Int.zero + Vector3Int.up * CurrentMap.Height);
            Vector3 rightBottom = _grid.CellToWorld(Vector3Int.zero + Vector3Int.right * CurrentMap.Width);
            Vector3 rightTop = _grid.CellToWorld(Vector3Int.zero + Vector3Int.up * CurrentMap.Height + Vector3Int.right * CurrentMap.Width);

            Debug.DrawLine(leftBottom, leftTop, Color.red, 120f);
            Debug.DrawLine(leftTop, rightTop, Color.red, 120f);
            Debug.DrawLine(rightTop, rightBottom, Color.red, 120f);
            Debug.DrawLine(rightBottom, leftBottom, Color.red, 120f);
        }
    }
}

