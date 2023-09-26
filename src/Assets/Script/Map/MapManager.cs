using System.Collections;
using System.Collections.Generic;
using Framework.Singleton;
using GrowTopia.Items;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace GrowTopia.Map
{
    public class MapManager : MonoSingleton<MapManager>
    {
        // Components
        [SerializeField]
        private Grid _grid; // The grid component where all tilemaps attached to.

        private MapData _current; // Current map data.

        private TilemapSet _tilemapSet = new TilemapSet();

        void Start()
        {
            _tilemapSet.Background = _grid.transform.Find("Background")?.GetComponent<Tilemap>();
            _tilemapSet.Solid = _grid.transform.Find("Solid")?.GetComponent<Tilemap>();
            _tilemapSet.Liquid = _grid.transform.Find("Liquid")?.GetComponent<Tilemap>();
            _tilemapSet.Platforms = _grid.transform.Find("Platforms")?.GetComponent<Tilemap>();
            _tilemapSet.Foreground = _grid.transform.Find("Foreground")?.GetComponent<Tilemap>();

            if (!_tilemapSet.Valid)
            {
                Debug.Log("Missing one of the tilemaps layer.");
            }
        }

        private void CreatePlatform(GridInfo grid)
        {
            if (!_current.Grids.TryAdd(grid.Position, grid)) // if the block on position already exist 
            {
                _current.Grids[grid.Position] = grid;
            }
            _tilemapSet.Platforms.SetTile(
                position: new Vector3Int(grid.Position.x, grid.Position.y),
                tile: grid.Block.Tile
                );
        }

        private void CreateSolid(GridInfo grid)
        {
            if (!_current.Grids.TryAdd(grid.Position, grid)) // if the block on position already exist 
            {
                _current.Grids[grid.Position] = grid;
            }
            _tilemapSet.Solid.SetTile(
                position: new Vector3Int(grid.Position.x, grid.Position.y),
                tile: grid.Block.Tile
                );
        }

        private void CreateLiquid(GridInfo grid)
        {
            if (!_current.Grids.TryAdd(grid.Position, grid)) // if the block on position already exist 
            {
                _current.Grids[grid.Position] = grid;
            }
            _tilemapSet.Solid.SetTile(
                position: new Vector3Int(grid.Position.x, grid.Position.y),
                tile: grid.Block.Tile
                );
        }

        public void Load(MapData map)
        {
            foreach (var grid in map.Grids.Values)
            {
                // Distribute the grid info to the right function.
                IReadOnlyBlock block = grid.Block;
                if (block.Attribute == BlockAttribute.Platform)
                {
                    CreatePlatform(grid);
                }
                else
                {
                    switch (block.Property)
                    {
                        case BlockProperty.Solid:
                            CreateSolid(grid); break;
                        case BlockProperty.Liquid:
                            CreateLiquid(grid); break;
                    }
                }
            }

            _current = map;
        }
    }
}

