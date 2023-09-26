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
        /// <summary>
        /// The delegate that gives a index and returns a inventory grid.
        /// Shortcut Bar uses this to update item sprite.
        /// </summary>
        /// <param name="index">The item index.</param>
        /// <returns>The inventory grid to show.</returns>
        public delegate IReadOnlyInventoryGrid ShowSignal(int index);
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
        /// The binder that input the item sprite.
        /// </summary>
        public ShowSignal DataBinder { get; set; }

        public int SelectedIndex => _current;

        // Start is called before the first frame update
        void Start()
        {
            _mouseScroll = InputManager.Instance.GetAction("PlayerControl/Move/Select");
            _mouseScroll.Enable();

            InitializeGrids();
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
        }

        private void RefreshInfo()
        {
            for (int i = 0; i < _showingGrids.Count; i++)
            {
                ShortcutBarGrid grid = _showingGrids[i];
                IReadOnlyInventoryGrid data = DataBinder?.Invoke(i);

                if (!InventoryGridInfo.IsEmpty(data))
                {
                    grid.Sprite = data.Item.Sprite;
                    grid.Count = data.Count;
                }
            }
        }

    }
}
