using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GrowTopia.Input;
using GrowTopia.Items;
using GrowTopia.Map;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GrowTopia.Player
{
    /// <summary>
    /// Behaviour class used to present the build behaviour of player. Such as placing blocks and destroying blocks.
    /// </summary>
    public class PlayerBuild : MonoBehaviour
    {
        /// <summary>
        /// If the player want to break the block, it needs time to hold the input key and wait it to be broken.
        /// </summary>
        private float _holdTime = 0;
        private bool _isHolding;


        private Vector2Int _gridPosInput;

        /// <summary>
        /// The key used to place block.
        /// </summary>
        private InputAction _placeBtn;

        /// <summary>
        /// The key used to break the block.
        /// </summary>
        private InputAction _breakBtn;

        /// <summary>
        /// The position of pointer.
        /// </summary>
        private InputAction _pointerPosInput;

        // Start is called before the first frame update
        void Start()
        {
            _placeBtn = InputManager.Instance.GetAction("PlayerControl/Build/Place");
            _placeBtn.started += PlaceBtnPressedDown;
            _placeBtn.Enable();

            _breakBtn = InputManager.Instance.GetAction("PlayerControl/Build/Break");
            _breakBtn.Enable();

            _pointerPosInput = InputManager.Instance.GetAction("PlayerControl/Build/Position");
            _pointerPosInput.Enable();
        }

        // Update is called once per frame
        void Update()
        {
            CheckPointerInput();
            
            if(_breakBtn.IsPressed())
                BreakBtnPressed();
        }

        private void CheckPointerInput()
        {
            Vector2 screenPos = _pointerPosInput.ReadValue<Vector2>();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);

            Vector2Int gridPos = MapManager.Instance.WorldToGrid(worldPos);

            _gridPosInput = gridPos;
        }

        private void PlaceBtnPressedDown(InputAction.CallbackContext context)
        {
            MapManager manager = MapManager.Instance;
            IReadOnlyInventoryGrid grid = LocalPlayer.Current.Selected;
            if(grid.IsEmpty())
                return;
            if (!(grid.Item is Block block))
                return;

            if (manager.CurrentMap.IsInMap(_gridPosInput) && manager.CurrentMap.IsEmpty(_gridPosInput))
            {
                manager.CreateBlock(_gridPosInput, block);
            }
        }

        private void BreakBtnPressed()
        {
            if (_isHolding)
                return;
            if (!MapManager.Instance.CurrentMap.TryGetGrid(_gridPosInput, out var grid))
                return;
            // starts a coroutine to check if the break btn was pressed enough time
            UniTask.Create(async () =>
            {
                Vector2Int position = _gridPosInput;
                IReadOnlyBlock block = grid.Block;
                float breakTime = block.Hardness / 1000f;
                _isHolding = true;
                
                while (_holdTime < breakTime)
                {
                    if (!_breakBtn.IsPressed()) // the player released button
                        break;
                    if (position != _gridPosInput) // the player moved the pointer to other grid
                        break;

                    // otherwise, adding holding time each frame
                    _holdTime += Time.deltaTime;
                    await UniTask.NextFrame();
                    Debug.Log(_holdTime + "/" + breakTime);
                }

                if (_holdTime >= breakTime)
                    MapManager.Instance.DestroyBlock(position);

                _holdTime = 0;
                _isHolding = false;
            });
        }
    }
}

