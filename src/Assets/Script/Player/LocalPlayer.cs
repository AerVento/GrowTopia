using System.Collections;
using System.Collections.Generic;
using System.IO;
using Framework.UI;
using GrowTopia.UI;
using Newtonsoft.Json;
using UnityEngine;

namespace GrowTopia.Player
{
    public class LocalPlayer : MonoBehaviour, IPlayer
    {
        private static LocalPlayer _localPlayer;

        public static LocalPlayer Current
        {
            get
            {
                if (_localPlayer == null)
                    throw new System.NullReferenceException("No local player found in the scene.");
                return _localPlayer;
            }
        }

        private InventoryHandler _inventoryHandler = new InventoryHandler();

        /// <summary>
        /// Player inventory.
        /// </summary>
        public Inventory Inventory => _inventoryHandler.Target;

        /// <summary>
        /// Current selected inventory item.
        /// </summary>
        public IReadOnlyInventoryGrid Selected
        {
            get
            {
                if (GamePanel.Instance != null) // the game panel exists.
                {
                    ShortcutBar bar = GamePanel.Instance.ShortcutBar;
                    return Inventory.GetShortcutGrid(bar.SelectedIndex);
                }
                else
                {
                    return InventoryGridInfo.Empty;
                }
            }
        }

        #region Interface Implementation
        GameObject IPlayer.Target => gameObject;
        bool IPlayer.IsLocal => true;
        #endregion

        void Awake()
        {
            if (_localPlayer != null)
            {
                Debug.LogWarning($"Multi-LocalPlayer Detected. The new one which name is {name} will be destroyed.");
                Destroy(gameObject);
                return;
            }

            _localPlayer = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            UIManager.Instance.CreatePanel<GamePanel>("GamePanel");
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void OnDestroy()
        {
            _localPlayer = null;
        }

    }

}
