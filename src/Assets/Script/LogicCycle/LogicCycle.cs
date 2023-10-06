using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Framework.Singleton;
using UnityEngine;
using UnityEngine.Events;

namespace GrowTopia.LogicCycle
{
    /// <summary>
    /// To control the game logic update cycle. 
    /// Like Update(), but its function will be executed in a longer game update circle.
    /// </summary>
    public class LogicCycle : MonoSingleton<LogicCycle>
    {
        [Tooltip("The time gap (seconds) between two game logic update cycle.")]
        [Range(0, 1)]
        [SerializeField]
        private float _cycleLength = 0.1f;

        private UnityAction _update;

        public event UnityAction OnUpdate
        {
            add => _update += value;
            remove => _update -= value;
        }

        private async UniTask UpdateCoroutine()
        {
            while(true){
                await UniTask.WaitForSeconds(_cycleLength);
                _update?.Invoke();
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            UniTask.Create(UpdateCoroutine);
        }
    }
}

