using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorBasics {

    public class UIPlayer : MonoBehaviour {

        [SerializeField] Text text;
        public Player player;

        public void SetPlayer (Player player) {
            Message.instance.AddMessage("UIPlayer.SetPlayer(). run");
            this.player = player;
            //text.text = "Player " + player.playerIndex.ToString ();
            text.text = player.NickName;
        }

    }
}