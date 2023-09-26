using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Framework.UI;
using GrowTopia.Player;
using GrowTopia.UI;
using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Inventory inventory = new Inventory();
        inventory.AddItem("lava",2);
        inventory.AddItem("stone",1);

        var panel = UIManager.Instance.CreatePanel<GamePanel>("GamePanel");
        panel.ShortcutBar.DataBinder = index => {
            return inventory.GetShortcutGrid(index);
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
