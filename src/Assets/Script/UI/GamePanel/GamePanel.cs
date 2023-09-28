using System.Collections;
using System.Collections.Generic;
using Framework.UI;
using UnityEngine;


namespace GrowTopia.UI
{
    public class GamePanel : SingletonPanel<GamePanel>
    {
        [SerializeField]
        private ShortcutBar _shortcutBar;

        public ShortcutBar ShortcutBar => _shortcutBar;

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

