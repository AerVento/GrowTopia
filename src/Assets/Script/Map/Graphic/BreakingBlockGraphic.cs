using System.Collections.Generic;
using GrowTopia.Data;
using GrowTopia.Events;
using GrowTopia.Map.Context;
using GrowTopia.Player.Context;
using GrowTopia.SO;
using UnityEngine;
using UnityEngine.Pool;

namespace GrowTopia.Map
{
    public static partial class MapGraphics
    {
        static class BreakingBlockGraphic
        {
            private static ObjectPool<Animator> _pool = new ObjectPool<Animator>(
                createFunc: () => GameObject.Instantiate(_animationPrefab, Graphics.transform).GetComponent<Animator>(),
                actionOnGet: (anim) => anim.gameObject.SetActive(true), 
                actionOnRelease: (anim) => anim.gameObject.SetActive(false), 
                actionOnDestroy: (anim) => GameObject.Destroy(anim.gameObject)
                );

            private static GameObject _animationPrefab;
            private static Dictionary<Vector2Int, Animator> _graphics = new ();

            private static void OnPlayerStartDestroy(PlayerDestroyBlockContext context)
            {
                Vector2Int pos = context.BlockPosition;

                Animator instance = _pool.Get();
                instance.speed = 1 / context.TotalTime;
                if (_graphics.ContainsKey(pos))
                {
                    _pool.Release(_graphics[pos]);
                    _graphics[pos] = instance;
                }
                else
                    _graphics.Add(pos, instance);

                instance.transform.position = MapManager.Instance.GridToWorld(pos);
            }


            private static void OnPlayerEndDestroyBlock((PlayerDestroyBlockContext context, bool success) arg)
            {
                if (_graphics.Remove(arg.context.BlockPosition, out Animator instance))
                {
                    _pool.Release(instance);
                }
            }

            private static void SetListenerActive(bool active)
            {
                if (active)
                {
                    EventCenter.OnPlayerStartDestroyBlock += OnPlayerStartDestroy;
                    EventCenter.OnPlayerEndDestroyBlock += OnPlayerEndDestroyBlock;
                }
                else
                {
                    EventCenter.OnPlayerStartDestroyBlock -= OnPlayerStartDestroy;
                    EventCenter.OnPlayerEndDestroyBlock -= OnPlayerEndDestroyBlock;
                }
            }

            public static void Start()
            {
                if (_animationPrefab == null)
                {
                    PrefabMap map = SingletonSOManager.Instance.GetSOFile<PrefabMap>("GraphicPrefabMap");
                    if (map == null || !map.Prefabs.TryGetValue("breakingBlocks", out var prefabPack))
                        return;
                    _animationPrefab = prefabPack.Prefab;
                }

                SetListenerActive(true);
            }

            public static void Stop()
            {
                _graphics.Clear();
                _pool.Clear();
                SetListenerActive(false);
            }
        }
    }
}