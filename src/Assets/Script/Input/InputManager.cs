using System.Collections;
using System.Collections.Generic;
using Framework.Singleton;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GrowTopia.Input
{
    public class InputManager : MonoSingleton<InputManager>
    {
        // List of input asset
        [SerializeField]
        private List<InputActionAsset> _assetList;

        /// <summary>
        /// Get input action from a path. The path style is InputActionAssetName/InputActionMapName/InputActionName .
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public InputAction GetAction(string path)
        {
            string[] names = path.Split('/');
            if (names.Length != 3)
            {
                Debug.LogError("Invalid input path:" + path + " .");
                return null;
            }

            string assetName = names[0];
            InputActionAsset asset = _assetList.Find(asset => asset.name == assetName);
            if (asset == null)
            {
                Debug.LogError("Unknown input action asset name:" + assetName + " .");
                return null;
            }

            string mapName = names[1];
            InputActionMap map = asset.FindActionMap(mapName);
            if (map == null)
            {
                Debug.LogError("Unknown input action map name:" + mapName + " .");
                return null;
            }

            string actionName = names[2];
            InputAction action = map.FindAction(actionName);
            if (map == null)
            {
                Debug.LogError("Unknown input action name:" + actionName + " .");
                return null;
            }
            else
                return action;
        }
    }
}

