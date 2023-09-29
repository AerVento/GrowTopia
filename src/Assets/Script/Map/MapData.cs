using System;
using System.Collections;
using System.Collections.Generic;
using GrowTopia.Items;
using GrowTopia.Serialization;
using Newtonsoft.Json;
using UnityEngine;

namespace GrowTopia.Map
{
    [System.Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class MapData : IEnumerable<KeyValuePair<Vector2Int, IReadOnlyMapGrid>>
    {
        private Dictionary<Vector2Int, MapGridInfo> _grids = new Dictionary<Vector2Int, MapGridInfo>();

        [JsonIgnore]
        private Action<MapChangedContext> _onMapChanged;

        /// <summary>
        /// Width of map. (blocks)
        /// </summary>
        public int Width = 20;

        /// <summary>
        /// Height of map. (blocks)
        /// </summary>
        public int Height = 60;

        /// <summary>
        /// The spawn point of the map.
        /// </summary>
        public Vector2Int SpawnPoint;

        /// <summary>
        /// Called when any of map grid changed.
        /// </summary>
        public event Action<MapChangedContext> OnMapChanged
        {
            add => _onMapChanged += value;
            remove => _onMapChanged -= value;
        }


        /// <summary>
        /// If the given position is in map.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool IsInMap(Vector2Int position)
        {
            // as default, we set [0,0] to [MaxWidth,MaxHeight] as the map content.
            return (0 <= position.x && position.x < Width) &&
                (0 <= position.y && position.y < Height);
        }

        /// <summary>
        /// If the grid was empty on given position. Position outside the map is always empty.
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool IsEmpty(Vector2Int position) => !IsInMap(position) || _grids.ContainsKey(position);

        /// <summary>
        /// Get the read only grid info at given position. 
        /// Returns null if the position is outside map or the grid is empty.
        /// </summary>
        /// <param name="position"></param>
        /// <returns>The grid info, or null if the position is empty.</returns>
        public IReadOnlyMapGrid GetGrid(Vector2Int position)
        {
            if (IsInMap(position))
            {
                if(_grids.TryGetValue(position, out MapGridInfo gridInfo))
                    return gridInfo;
            }
            return null;
        }

        /// <summary>
        /// Try get the grid info at given position. 
        /// If the given position is outside map or the grid was empty on the position, returns false.
        /// Otherwise, returns true.
        /// </summary>
        /// <param name="position">Target position.</param>
        /// <param name="grid">Output grid info.</param>
        /// <returns>True if the position has a block.</returns>
        public bool TryGetGrid(Vector2Int position, out IReadOnlyMapGrid grid)
        {
            grid = GetGrid(position);
            return grid != null;
        }

        /// <summary>
        /// Get an array of the grid info at given positions. 
        /// Similar as <see cref="GetGrid()"/> but get multi grids at one time.
        /// </summary>
        /// <param name="positions">Target positions.</param>
        /// <returns>A enumerable grid info result.</returns>
        public IEnumerable<KeyValuePair<Vector2Int, IReadOnlyMapGrid>> GetGrids(IEnumerable<Vector2Int> positions)
        {
            foreach (var pos in positions)
            {
                yield return new KeyValuePair<Vector2Int, IReadOnlyMapGrid>(pos, GetGrid(pos));
            }
        }

        /// <summary>
        /// Check if the grid of given position is having the same block as specified.
        /// </summary>
        /// <param name="position">The target position.</param>
        /// <param name="block">The block to check. Null to check if empty.</param>
        /// <returns></returns>
        public bool CheckBlock(Vector2Int position, IReadOnlyBlock block)
        {
            // if the param was null then just check if empty
            if(block == null)
                return IsEmpty(position);
            
            // otherwise, get the grid of target position to have a check. 
            if (TryGetGrid(position, out IReadOnlyMapGrid grid))
            {
                // uses the block id to check if they were the same.
                return grid.BlockId == block.Id;
            }
            else // the target position is empty but the given block is not.
                return false;
        }

        /// <summary>
        /// Set the grid internally. By use the context it can track the changes of the map.
        /// Real operations on dictionary.
        /// Changes one grid at one time.
        /// </summary>
        /// <param name="position">The target position.</param>
        /// <param name="grid">The new grid content. Null to set empty.</param>
        /// <param name="context">The context used to collect the changes. Null if not needed.</param>
        /// <returns></returns>
        private MapChangedContext SetGridWithContext(Vector2Int position, IReadOnlyMapGrid grid, MapChangedContext context = null)
        {
            // if the context was enabled and find different on comparing the current grid to the new one
            if (context != null && !CheckBlock(position, grid.Block))
            {
                context.AddEntry(position, GetGrid(position), grid);
            }

            // special path for null: set grid empty by removing the dictionary key
            if (grid == null)
            {
                if (_grids.ContainsKey(position))
                    _grids.Remove(position);
            }
            else
            {
                // only data clones will be stored 
                // because it is reference type and we don't want to let the extern to keep the access directly to the grid in dictionary.
                if (_grids.ContainsKey(position))
                    _grids[position] = grid.Clone();
                else
                    _grids.Add(position, grid.Clone());
            }

            return context;
        }

        /// <summary>
        /// Set the grid info.
        /// </summary>
        /// <param name="position">Target position.</param>
        /// <param name="grid">The grid info. Set null to set the position empty.</param>
        public void SetGrid(Vector2Int position, IReadOnlyMapGrid grid)
        {
            if (IsInMap(position))
            {
                var context = SetGridWithContext(position, grid, new MapChangedContext());

                if (context.Count > 0)
                    _onMapChanged?.Invoke(context);
            }
        }

        public void SetGrids(IEnumerable<KeyValuePair<Vector2Int, IReadOnlyMapGrid>> changes)
        {
            var context = new MapChangedContext();

            foreach (var pair in changes)
            {
                if (IsInMap(pair.Key))
                    context = SetGridWithContext(pair.Key, pair.Value, context);
            }

            if (context.Count > 0)
                _onMapChanged?.Invoke(context);
        }

        public IEnumerator<KeyValuePair<Vector2Int, IReadOnlyMapGrid>> GetEnumerator()
        {
            foreach (var pair in _grids)
                yield return new KeyValuePair<Vector2Int, IReadOnlyMapGrid>(pair.Key, pair.Value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

