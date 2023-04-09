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
    public bool isPublic=true;
    public bool isFull=false;
    public bool isOpen=true;
    public int MaxClientsAmount = 4;
    public List<OwnClient> Clients = new List<OwnClient>();      // Clients.Count();

    public OwnMatch() { }

    public OwnMatch(string address, bool isPublic, OwnClient client, int maxClientsAmount=4)
    {
        Address = address;
        this.isPublic = isPublic;
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

    public bool FindMatchWithAddress(OwnClient client,string address)
    {
        address = address.ToUpper();
        foreach(OwnMatch match in Matches)
        {
            if (match.Address == address)
            {
                return JoinIntoMatch(client, match);
            }
        }              
        return false;
    }
    public bool SearchAndJoinIntoMatch(OwnClient client)
    {
        foreach (OwnMatch match in Matches)
        {
            if (match.isPublic&& match.isOpen&& !match.isFull)
            {
                return JoinIntoMatch(client, match);
            }
        }
        return false;
    }
    public bool JoinIntoMatch(OwnClient client,OwnMatch match)
    {
        Debug.Log($"<color=green> I been join into match </color>");
        match.Clients.Add(client);
        return false;    
    }
    public void ClientDisconnect(OwnClient client)
    {
        if (client.MatchID != "Empty" || client.MatchID != "")
        {
            foreach (OwnMatch match in Matches)
            {
                if (match.Address==client.MatchID)
                {
                    if (match.Clients.Contains(client))
                    {
                        match.Clients.Remove(client);
                    }
                }
            }
        }
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
public static class MatchExtensions
{
    public static Guid ToGuid(this string id)
    {
        MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
        byte[] inputBytes = Encoding.Default.GetBytes(id);
        byte[] hashBytes = provider.ComputeHash(inputBytes);

        return new Guid(hashBytes);
    }
}
