using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GrowTopia.Player
{
    public interface IPlayer
    {
        public static LocalPlayer Local => LocalPlayer.Current;

        /// <summary>
        /// The player object.
        /// </summary>
        public GameObject Target { get; }

        /// <summary>
        /// If the player is the local player.
        /// </summary>
        /// <value></value>
        public bool IsLocal { get; }
    }
}

