using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GrowTopia.Map.Context
{
    public class MapChangedContext : IEnumerable<MapChangedContext.Entry>
    {
        private List<Entry> _entries;
        public IReadOnlyList<Entry> Entries => _entries;
        
        public MapChangedContext()
        {
            _entries = new List<Entry>();
        }

        public MapChangedContext(IEnumerable<Entry> entries)
        {
            _entries = entries.ToList();
        }


        public int Count => _entries.Count;

        public void AddEntry(Vector2Int pos, MapGridInfo? oldGrid, MapGridInfo? newGrid)
        {
            _entries.Add(new Entry()
            {
                Position = pos,
                OldGrid = oldGrid,
                NewGrid = newGrid
            });
        }

        public IEnumerator<Entry> GetEnumerator()
        {
            return ((IEnumerable<Entry>)_entries).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_entries).GetEnumerator();
        }

        public class Entry
        {
            public Vector2Int Position;
            public MapGridInfo? OldGrid;
            public MapGridInfo? NewGrid;

            public bool IsCreating => !OldGrid.HasValue && NewGrid.HasValue;
            public bool IsChanging => OldGrid.HasValue && NewGrid.HasValue;
            public bool IsDestroying => OldGrid.HasValue && !NewGrid.HasValue;
        }
    }
}