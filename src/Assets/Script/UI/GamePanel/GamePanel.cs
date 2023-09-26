using System.Collections;
using System.Collections.Generic;
using Framework.UI;
using UnityEngine;


namespace GrowTopia.UI
{
    public class GamePanel : MonoBehaviour, IPanel
    {
        [SerializeField]
        private ShortcutBar _shortcutBar;

        public ShortcutBar ShortcutBar => _shortcutBar;
        public bool IsShowing { get; private set; } = false;
        
        public void Hide()
        {
            IsShowing = false;
            gameObject.SetActive(false);
        }

        public void Show()
        {
            IsShowing = true;
            gameObject.SetActive(true);
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

