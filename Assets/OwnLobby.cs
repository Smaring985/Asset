using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OwnLobby : MonoBehaviour
{
    public static OwnLobby instance;
    public InputField MatchAddress;

    [SerializeField] Transform UIClientParent;
    [SerializeField] GameObject UIClientPrefab;

    public void Awake()
    {
        instance = this;
    }
    public void Join()    // Join button
    {
        OwnClient.localClient.JoinIntoMatch(MatchAddress.text);
    }
    public GameObject SpawnPlayerUIPrefab(OwnClient client)
    {
        GameObject newUIPlayer = Instantiate(UIClientPrefab, UIClientParent);
        newUIPlayer.GetComponent<OwnUIClient>().SetClient(client);

        return newUIPlayer;
    }
}
