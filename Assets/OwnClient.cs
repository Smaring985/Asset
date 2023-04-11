using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(NetworkMatch))]
public class OwnClient : NetworkBehaviour
{
    [SyncVar] public string NickName = "None";
    [SyncVar] public bool isInMatch=false;
    [SyncVar] public string MatchID="Empty";
    [SyncVar] public bool isMatchHost=true;

    public static OwnClient localClient;
    [SyncVar] public OwnMatch Match=null;


    public override void OnStartClient()
    {
        Debug.Log("Client.OnStartClient()");
        if (isLocalPlayer)
        {
            localClient = this;
        }
        else
        {
            Debug.Log($"Spawning other player UI Prefab (NickName {NickName})");
            //playerLobbyUI = UILobby.instance.SpawnPlayerUIPrefab(this);
        }
    }
    public void BeginMatch()
    {
        OwnMatchMaker.instance.BeginMatch(Match);
    }
    public void SearchMatch()
    {
        OwnMatchMaker.instance.SearchAndJoinIntoMatch(this);
    }
    public void DisconnectFromMatch()
    {
        OwnMatchMaker.instance.ClientDisconnect(this);
    }

    public void OnClientJoinIntoMatch()
    {
        if (Match.isFull)
        {
            Debug.Log("Game was started");
            // start the game
        }    
    }
    public void OnBeginGameIntoMatch()
    {
        Debug.Log("I been into a game");
    }
}
