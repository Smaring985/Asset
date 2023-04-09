using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OwnUIClient : MonoBehaviour
{
    [SerializeField] Text text;
    public OwnClient client;

    public void SetClient(OwnClient client)
    {
        this.client = client;
        text.text = client.NickName;
    }
}
