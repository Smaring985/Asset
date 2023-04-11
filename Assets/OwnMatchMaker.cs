using Mirror;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

[Serializable]
public class OwnMatch
{
    public string Address="1XYJ7"; // .ToUpper();
    public bool isFull=false;
    public bool isOpen=true;
    public int MaxClientsAmount = 4;
    public List<OwnClient> Clients = new List<OwnClient>();      // Clients.Count();

    public OwnMatch() { }

    public OwnMatch(string address, OwnClient client, int maxClientsAmount=4)
    {
        Address = address;
        isFull = false;
        isOpen = true;
        MaxClientsAmount = maxClientsAmount;
        Clients.Add(client);
    }
}
public class OwnMatchMaker : NetworkBehaviour
{                 
    public int maxMatchPlayers = 12;
    public static OwnMatchMaker instance=new OwnMatchMaker();
    public SyncList<OwnMatch> Matches=new SyncList<OwnMatch>();
      
    public void Start()
    {
        instance = this;
    }


    public void OnConnectedToServer()
    {
        Debug.Log("OnConnectedToServer");
        // client join into server
        OwnLobby.instance.OnJoinedIntoServer();
    }

    public bool SearchAndJoinIntoMatch(OwnClient client)
    {      
        Debug.Log("SearchAndJoinIntoMatch");
        if (!client.isInMatch)
        {
            foreach (OwnMatch match in Matches)
            {
                if (match.isOpen && !match.isFull)
                {
                    if (JoinIntoMatch(client, match))
                    {
                        return true;
                    }
                }
            }
            return HostMatch(client);
        }
        return false;
    }
    public bool JoinIntoMatch(OwnClient joinedClient,OwnMatch match)
    {
        Debug.Log("JoinIntoMatch");
        if (Matches.Contains(match) && !match.Clients.Contains(joinedClient))
        {
            match.Clients.Add(joinedClient);
            joinedClient.Match = match;
            joinedClient.MatchID = match.Address;

            if (match.Clients.Count == match.MaxClientsAmount)
            {
                match.isFull = true;
            }

            Debug.Log($"<color=green> I been join into match </color> and match.isFull={match.isFull}");
            foreach (OwnClient client in match.Clients)
            {
                if (client != joinedClient)
                {
                    client.OnClientJoinIntoMatch();
                }
            }
            return true;
        }
        return false;    
    }
    public bool HostMatch(OwnClient client)
    {
        Debug.Log("HostMatch");
        OwnMatch match = new OwnMatch(GetRandomMatchID(), client);
        client.Match = match;
        client.MatchID = match.Address;
        Matches.Add(match);
        return Matches.Contains(match);
    }
    public void ClientDisconnect(OwnClient client)
    {
        Debug.Log("ClientDisconnect");
        if (client.MatchID != "Empty" || client.MatchID != "")
        {
            foreach (OwnMatch match in Matches)
            {
                if (match.Address==client.MatchID)
                {
                    if (match.Clients.Contains(client))
                    {
                        client.Match = null;
                        client.MatchID = "Empty";
                        match.Clients.Remove(client);
                        return;
                    }
                }
            }
        }
    }
    public void BeginMatch(OwnMatch match)
    {
        Debug.Log("BeginMatch");
        match.isOpen = false;
        foreach (OwnClient client in match.Clients)
        {
            if (!client.isMatchHost)
            {
                client.OnBeginGameIntoMatch();
            }
        }
    }

    [ContextMenu("Write Matches")]
    public void WriteMatches()
    {
        Debug.Log("WriteMatches");
        string message = "";
        foreach (OwnMatch match in Matches)
        {
            string clients = "";
            foreach(OwnClient client in match.Clients)
            {
                clients += client.NickName+" ";
            }
            message += $"Address {match.Address}, isFull {match.isFull}, isOpen {match.isOpen}, MaxClientsAmount {match.MaxClientsAmount}, clients {clients}\n";
        }
        GUIUtility.systemCopyBuffer = message;
        Debug.Log(message);
    }



    public static string GetRandomMatchID()
    {
        string _id = string.Empty;
        for (int i = 0; i < 5; i++)
        {
            int random = UnityEngine.Random.Range(0, 36);
            if (random < 26)
            {
                _id += (char)(random + 65);
            }
            else
            {
                _id += (random - 26).ToString();
            }
        }
        Debug.Log($"Random Match ID: {_id}");
        return _id;
    }

}