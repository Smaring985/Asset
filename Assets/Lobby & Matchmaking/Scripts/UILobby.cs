using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorBasics {

    public class UILobby : MonoBehaviour {

        public static UILobby instance;

        [Header ("Host Join")]
        [SerializeField] InputField joinMatchInput;
        [SerializeField] List<Selectable> lobbySelectables = new List<Selectable> ();
        [SerializeField] Canvas lobbyCanvas;
        [SerializeField] Canvas searchCanvas;
        bool searching = false;

        [Header ("Lobby")]
        [SerializeField] Transform UIPlayerParent;
        [SerializeField] GameObject UIPlayerPrefab;
        [SerializeField] Text matchIDText;
        [SerializeField] GameObject beginGameButton;

        GameObject localPlayerLobbyUI;

        [Header("Kirill change")]
        public float TimeForAutoHost = 7f;
        public InputField NickNameField;

        void Start () {
            Debug.Log("UILobby.Start(). run");
            instance = this;
        }

        public void SetStartButtonActive (bool active) {
            beginGameButton.SetActive (active);
        }

        public void HostPublic () {
            Message.instance.AddMessage("UILobby.HostPublic(). run");
            lobbySelectables.ForEach (x => x.interactable = false);

            Player.localPlayer.HostGame (true);
        }

        public void HostPrivate () {
            Message.instance.AddMessage("UILobby.HostPrivate(). run");
            lobbySelectables.ForEach (x => x.interactable = false);

            Player.localPlayer.HostGame (false);
        }

        public void HostSuccess (bool success, string matchID) {
            Message.instance.AddMessage($"UILobby.HostSuccess(success {success}, matchID {matchID}). run");
            if (success) {
                lobbyCanvas.enabled = true;

                if (localPlayerLobbyUI != null) Destroy (localPlayerLobbyUI);
                localPlayerLobbyUI = SpawnPlayerUIPrefab (Player.localPlayer);
                matchIDText.text = matchID;
            } else {
                lobbySelectables.ForEach (x => x.interactable = true);
            }
        }

        public void Join()
        {
            Message.instance.AddMessage("UILobby.Join(). run");
            lobbySelectables.ForEach(x => x.interactable = false);
            Player.localPlayer.JoinGame(joinMatchInput.text.ToUpper());
        }

        public void JoinSuccess (bool success, string matchID) {
            Debug.Log($"UILobby.JoinSuccess(success {success}, matchID {matchID}). run");
            if (success) {
                lobbyCanvas.enabled = true;

                if (localPlayerLobbyUI != null) Destroy (localPlayerLobbyUI);
                localPlayerLobbyUI = SpawnPlayerUIPrefab (Player.localPlayer);
                matchIDText.text = matchID;
            } else {
                lobbySelectables.ForEach (x => x.interactable = true);
            }
        }

        public void DisconnectGame () {
            Message.instance.AddMessage("UILobby.DisconnectGame(). run");
            if (localPlayerLobbyUI != null) Destroy (localPlayerLobbyUI);
            Player.localPlayer.DisconnectGame ();

            lobbyCanvas.enabled = false;
            lobbySelectables.ForEach (x => x.interactable = true);
        }

        public GameObject SpawnPlayerUIPrefab (Player player) {
            Message.instance.AddMessage($"UILobby.SpawnPlayerUIPrefab(player.NickName {player.NickName}). run");
            GameObject newUIPlayer = Instantiate (UIPlayerPrefab, UIPlayerParent);
            newUIPlayer.GetComponent<UIPlayer> ().SetPlayer (player);
            newUIPlayer.transform.SetSiblingIndex (player.playerIndex - 1);

            return newUIPlayer;
        }

        public void BeginGame () {
            Message.instance.AddMessage("UILobby.BeginGame(). run");
            Player.localPlayer.BeginGame ();
        }

        public void SearchGame () {
            Message.instance.AddMessage("UILobby.SearchGame(). run");
            StartCoroutine (Searching ());
        }

        public void CancelSearchGame () {
            Message.instance.AddMessage("UILobby.CancelSearchGame(). run");
            searching = false;
        }

        public void SearchGameSuccess (bool success, string matchID) {
            Message.instance.AddMessage($"UILobby.SearchGameSuccess(success {success}, matchID {matchID}). run");
            if (success) {
                searchCanvas.enabled = false;
                searching = false;
                JoinSuccess (success, matchID);
            }
        }

        IEnumerator Searching () {
            Message.instance.AddMessage("UILobby.Searching(). run");
            searchCanvas.enabled = true;
            searching = true;

            float searchInterval = 1;
            float currentTime = 1;
            float timeToAutoHost = TimeForAutoHost;

            while (searching) {
                if (currentTime > 0) {
                    currentTime -= Time.deltaTime;
                } else {
                    currentTime = searchInterval;
                    Player.localPlayer.SearchGame ();
                }
                if (timeToAutoHost > 0)
                {
                    timeToAutoHost -= Time.deltaTime;
                }
                else
                {
                    HostPublic();
                }
                yield return null;
            }
            searchCanvas.enabled = false;
        }

        public void SetNickname()
        {
            Player.localPlayer.NickName = NickNameField.text;
        }

    }
}