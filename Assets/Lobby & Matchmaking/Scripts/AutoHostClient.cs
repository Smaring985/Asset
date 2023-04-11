using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace MirrorBasics {
    public class AutoHostClient : MonoBehaviour {

        [SerializeField] NetworkManager networkManager;

        void Start () {
            if (!Application.isBatchMode) { //Headless build
                Debug.Log ($"=== Client Build ===");
                networkManager.StartClient ();
            } else {
                Debug.Log ($"=== Server Build ===");
                networkManager.StartServer();
            }
        }

        public void JoinLocal () {
            Message.instance.AddMessage("AutoHostClient.JoinLocal(). run");
            networkManager.networkAddress = "176.57.68.230";//"176.57.68.230"localhost;
            networkManager.StartClient ();
        }

    }
}