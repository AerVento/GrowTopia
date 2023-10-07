using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Framework.Singleton;
using GrowTopia.Events;
using GrowTopia.Items;
using GrowTopia.Map.Context;
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

            // load map data from extern and apply these map data to the tilemap
            Load();
            DrawMapBoarder();

            // start the graphic
            MapGraphics.StartGraphic();
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
        private void ReplaceBlockInternal(Vector2Int position, MapGridInfo grid)
        {
            IReadOnlyBlock block = grid.MapBlock.Block;

            // set the map data 
            CurrentMap.SetGrid(position, grid);

            // Distribute the grid info to the right map layer
            TilemapLayer layer = GetLayerOfBlock(block);

            // set the tile
            _tilemapSet.GetMapLayer(layer)?.SetTile(
                position: new Vector3Int(position.x, position.y),
                tile: grid.MapBlock.Tile
                );
        }


        /// <summary>
        /// Create a block on specified location. If the location already have a block, it will be replaced.
        /// Use block id to create a map grid, and the block of the grid was created from the block prototype.
        /// </summary>
        /// <param name="position">The target position.</param>
        /// <param name="blockId">The block id of the prototype. </param>
        public void CreateBlock(Vector2Int position, string blockId)
        {
            if (blockId == null)
                DestroyBlock(position);
            else
                ReplaceBlockInternal(position, new MapGridInfo(position, blockId));
        }

        /// <summary>
        /// Create a block on specified location. If the location already have a block, it will be replaced.
        /// Use IMapBlock instance to create a map grid, and the block of the grid is the given instance.
        /// </summary>
        /// <param name="position">The target position.</param>
        /// <param name="block">The map block instance to create. </param>
        public void CreateBlock(Vector2Int position, IMapBlock block)
        {
            if (block == null)
                DestroyBlock(position);
            else
                ReplaceBlockInternal(position, new MapGridInfo(position, block));
        }

        /// <summary>
        /// Destroy a block on specified location. If the location is empty, it will do nothing.
        /// </summary>
        /// <param name="position"></param>
        public void DestroyBlock(Vector2Int position)
        {
            if (_currentMapHandler.Target.TryGetGrid(position, out var grid))
            {
                TilemapLayer layer = GetLayerOfBlock(grid.MapBlock.Block);
                _tilemapSet.GetMapLayer(layer).SetTile(
                    new Vector3Int(position.x, position.y), tile: null
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
        /// Transform a grid position to world position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Vector3 GridToWorld(Vector2Int position)
        {
            Vector3 pos = _grid.CellToWorld(new Vector3Int(position.x, position.y));
            return pos;
        }

        /// <summary>
        /// Load the map data and set tile for each entry.
        /// </summary>
        public void Load()
        {
            _currentMapHandler.Load();
            _currentMapHandler.Target.OnMapChanged += OnMapChangedNoticeExtern;

            foreach (var (position, grid) in _currentMapHandler.Target)
            {
                IMapBlock block = grid.MapBlock;
                _tilemapSet.GetMapLayer(
                    layer: GetLayerOfBlock(grid.MapBlock.Block)
                    ).SetTile(new Vector3Int(position.x, position.y), block.Tile);
            }
        }

        /// <summary>
        /// When the map changed, distribute the change message to: creating/changing/destroying and notice event center.
        /// </summary>
        /// <param name="context"></param>
        private void OnMapChangedNoticeExtern(MapChangedContext context)
        {
            IEnumerable<MapChangedContext.Entry> changes = context.Entries;
            MapChangedContext created = new MapChangedContext(from change in changes where change.IsCreating select change);
            MapChangedContext changed = new MapChangedContext(from change in changes where change.IsChanging select change);
            MapChangedContext destroyed = new MapChangedContext(from change in changes where change.IsDestroying select change);

            EventCenter.OnBlockCreated.Trigger(created);
            EventCenter.OnBlockChanged.Trigger(changed);
            EventCenter.OnBlockDestroyed.Trigger(destroyed);
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

