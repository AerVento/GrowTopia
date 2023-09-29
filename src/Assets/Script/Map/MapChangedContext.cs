using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrowTopia.Map
{
    public class MapChangedContext : IEnumerable<MapChangedContext.Entry>
    {
        public List<Entry> Entries = new List<Entry>();

        public int Count => Entries.Count;

        public void AddEntry(Vector2Int pos, IReadOnlyMapGrid oldGrid, IReadOnlyMapGrid newGrid)
        {
            Entries.Add(new Entry()
            {
                Position = pos,
                OldGrid = oldGrid,
                NewGrid = newGrid
            });
        }

        public IEnumerator<Entry> GetEnumerator()
        {
            return ((IEnumerable<Entry>)Entries).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Entries).GetEnumerator();
        }

        public class Entry
        {
            public Vector2Int Position;
            public IReadOnlyMapGrid OldGrid;
            public IReadOnlyMapGrid NewGrid;
        }
    }
}