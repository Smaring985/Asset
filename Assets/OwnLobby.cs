using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OwnLobby : MonoBehaviour
{
    public static OwnLobby instance;
    public InputField NickName;
    public Text Nick;

    public GameObject ClientPrefab;


    public void Awake()
    {
        instance = this;
    }



    public void SetNickName()
    {
        OwnClient.localClient.NickName = NickName.text;
        Nick.text = NickName.text;
    }
    public void Begin()
    {
        OwnClient.localClient.BeginMatch();
    }   
    public void Search()
    {
        OwnClient.localClient.SearchMatch();
    }
    public void Disconnect()
    {
        OwnClient.localClient.DisconnectFromMatch();
    }
    public void WriteMatches()
    {
        OwnMatchMaker.instance.WriteMatches();
    }

    public void OnJoinedIntoServer()
    {
        OwnClient client = new OwnClient();
        InstantiateClient(client);
    }

    public void InstantiateClient(OwnClient client)
    {
        GameObject go = Instantiate(ClientPrefab);
        OwnClient goClient = go.GetComponent<OwnClient>();
        goClient = client;
    }
}
