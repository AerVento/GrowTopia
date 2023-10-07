using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GrowTopia.Events;
using GrowTopia.Input;
using GrowTopia.Items;
using GrowTopia.Map;
using GrowTopia.Player.Context;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GrowTopia.Player
{
    /// <summary>
    /// Behaviour class used to present the build behaviour of player. Such as placing blocks and destroying blocks.
    /// </summary>
    [RequireComponent(typeof(IPlayer))]
    public class PlayerBuild : MonoBehaviour
    {
        private bool _isHolding;

        private IPlayer _player;

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
            _player = GetComponent<IPlayer>();
            
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
            if (!(grid.Item is IReadOnlyBlock block))
                return;

            if (manager.CurrentMap.IsInMap(_gridPosInput) && manager.CurrentMap.IsEmpty(_gridPosInput))
            {
                manager.CreateBlock(_gridPosInput, block.Id);

                
                EventCenter.OnPlayerPlaceBlock.Trigger(new PlayerPlaceBlockContext(_player, _gridPosInput));
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
                IReadOnlyBlock block = grid.MapBlock.Block;
                float breakTime = block.Hardness / 1000f;
                float holdTime = 0;

                _isHolding = true;
                
                PlayerDestroyBlockContext context = new PlayerDestroyBlockContext(_player, position, breakTime);

                EventCenter.OnPlayerStartDestroyBlock.Trigger(context);

                while (holdTime < breakTime)
                {
                    if (!_breakBtn.IsPressed()) // the player released button
                        break;
                    if (position != _gridPosInput) // the player moved the pointer to other grid
                        break;

                    // otherwise, adding holding time each frame
                    holdTime += Time.deltaTime;
                    await UniTask.NextFrame();
                    EventCenter.OnPlayerContinueDestroyingBlock.Trigger((context, holdTime / breakTime));
                }

                bool success = holdTime >= breakTime;

                if (success)
                    MapManager.Instance.DestroyBlock(position);
                
                EventCenter.OnPlayerEndDestroyBlock.Trigger((context, success));
                _isHolding = false;
            });
        }
    }
}

