using System.Collections;
using System.Collections.Generic;

namespace GrowTopia.Player
{

    public class InventoryChangedContext: IEnumerable<InventoryChangedContext.Entry>
    {
        public List<Entry> Entries = new List<Entry>();
        public int Count => Entries.Count;

        public void AddEntry(InventoryGridInfo oldGrid, InventoryGridInfo newGrid)
        {
            Entries.Add(new Entry() { Old = oldGrid, New = newGrid });
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
            public InventoryGridInfo Old;
            public InventoryGridInfo New;
        }
    }
}