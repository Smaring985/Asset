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

    [SerializeField] GameObject playerLobbyUI;
    NetworkMatch networkMatch;
    Guid netIDGuid;
    public static OwnClient localClient;

    public override void OnStartServer()
    {
        Message.instance.AddMessage("Player.OnStartServer(). run");
        netIDGuid = MatchExtensions.ToGuid(netId.ToString());
        networkMatch.matchId = netIDGuid;
    }
    public override void OnStartClient()
    {
        if (isLocalPlayer)
        {
            localClient = this;
        }
        else
        {
            Debug.Log($"Spawning other player UI Prefab (NickName {NickName})");
            playerLobbyUI = OwnLobby.instance.SpawnPlayerUIPrefab(this);
        }
    }
    public override void OnStopClient()
    {
        Message.instance.AddMessage($"Client Stopped");
        //ClientDisconnect();
    }

    public override void OnStopServer()
    {
        Message.instance.AddMessage($"Client Stopped on Server");
        //ServerDisconnect();
    }

    public void DisconnectGame()
    {
        CmdDisconnectGame();
    }

    [Command]
    void CmdDisconnectGame()
    {
        ServerDisconnect();
    }

    void ServerDisconnect()
    {
        OwnMatchMaker.instance.ClientDisconnect(this);
        RpcDisconnectGame();
        networkMatch.matchId = netIDGuid;
    }

    [ClientRpc]
    void RpcDisconnectGame()
    {
        Message.instance.AddMessage("Player.RpcDisconnectGame(). run");
        ClientDisconnect();
    }

    void ClientDisconnect()
    {
        if (playerLobbyUI != null)
        {
            if (!isServer)
            {
                Destroy(playerLobbyUI);
            }
            else
            {
                playerLobbyUI.SetActive(false);
            }
        }
    }

    public void JoinIntoMatch(string address)
    {
        if ((address == "" || address== "Empty")&& address.Length!=5)
        {
            OwnMatchMaker.instance.SearchAndJoinIntoMatch(this);
        }
        else
        {
            OwnMatchMaker.instance.FindMatchWithAddress(this,address);
        }
    }
}
