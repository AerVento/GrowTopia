using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace GrowTopia.Player
{
    /// <summary>
    /// This is an handler for inventory, and provided some methods to load the latest inventory and save it.
    /// </summary>
    public class InventoryHandler
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented
        };

        private static System.Text.Encoding Encoding => System.Text.Encoding.UTF8;

        private Inventory _target;

        public Inventory Target
        {
            get
            {
                if (_target == null)
                    Load();
                return _target;
            }
        }

        /// <summary>
        /// Load the inventory from a specific source.
        /// </summary>
        /// <returns>If the method succeed.</returns>
        public bool Load()
        {
            Debug.Log("Loading inventory...");
            try
            {
                // For test, read inventory from disk.
                // TODO: Get inventory from server.
                string content = File.ReadAllText("Assets/Script/Player/Inventory.json");
                _target = JsonConvert.DeserializeObject<Inventory>(content, Settings);
                _target.OnInventoryChanged += context => Save();
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                _target = new Inventory();
                return false;
            }
        }

        /// <summary>
        /// Save the inventory to a specific source.
        /// </summary>
        /// <returns>If the method succeed.</returns>
        public bool Save()
        {
            Debug.Log("Saving inventory...");
            try
            {
                // For test, save inventory to disk.
                // TODO: Upload inventory to server.
                string content = JsonConvert.SerializeObject(_target, Settings);
                File.WriteAllText("Assets/Script/Player/Inventory.json", content, Encoding);
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
                return false;
            }
        }
    }
}