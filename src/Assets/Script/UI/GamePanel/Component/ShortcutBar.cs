using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GrowTopia.Input;
using GrowTopia.Items;
using GrowTopia.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace GrowTopia.UI
{
    public class ShortcutBar : MonoBehaviour
    {
        const int GRID_COUNT = 9;

        [SerializeField]
        private RectTransform _selection;

        [SerializeField]
        private Transform _content;

        [SerializeField]
        private GameObject _gridPrefab;

        private List<ShortcutBarGrid> _showingGrids = new();

        // Input
        private InputAction _mouseScroll;

        // Selection
        private int _current;

        /// <summary>
        /// The inventory to show.
        /// </summary>
        public Inventory TargetInventory { get; set; } = null;

        /// <summary>
        /// Current selected index.
        /// </summary>
        public int SelectedIndex => _current;

        /// <summary>
        /// Called when selected index changed.
        /// </summary>
        public event Action<int> OnSelectedChanged;

        // Start is called before the first frame update
        void Start()
        {
            _mouseScroll = InputManager.Instance.GetAction("PlayerControl/Move/Select");
            _mouseScroll.Enable();

            InitializeGrids();

            if (TargetInventory == null)
                TargetInventory = IPlayer.Local.Inventory;
        }

        private void InitializeGrids()
        {

            foreach (var grid in _showingGrids)
            {
                Destroy(grid.gameObject);
            }
            _showingGrids.Clear();

            for (int i = 0; i < GRID_COUNT; i++)
            {
                GameObject instance = Instantiate(_gridPrefab, _content);
                _showingGrids.Add(instance.GetComponent<ShortcutBarGrid>());
            }
        }

        // Update is called once per frame
        void Update()
        {
            CheckMouseScroll();
            RefreshInfo();
        }

        private void CheckMouseScroll()
        {
            Vector2 value = _mouseScroll.ReadValue<Vector2>();
            if (value.y < 0 && _current < _showingGrids.Count - 1)
                _current++;
            else if (value.y > 0 && _current > 0)
                _current--;

            _selection.anchoredPosition = Vector2.zero;
            _selection.SetParent(_showingGrids[_current].transform);

            if (value.y != 0)
                OnSelectedChanged?.Invoke(_current);
        }

        private void RefreshInfo()
        {
            for (int i = 0; i < _showingGrids.Count; i++)
            {
                ShortcutBarGrid grid = _showingGrids[i];
                IReadOnlyInventoryGrid data = TargetInventory?.GetShortcutGrid(i);

                if (data == null || data.IsEmpty())
                    grid.SetEmpty();
                else
                {
                    grid.Sprite = data.Item.Sprite;
                    grid.Count = data.Count;
                }
            }
        }

    }
}
