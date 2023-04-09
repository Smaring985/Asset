using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour
{
    public static Message instance;

    public string _message;

    public void Awake()
    {
        instance = this;
    }
    public void AddMessage(string message)
    {
        _message += message + "\n";
    }
    public void DebugMessage()
    {
        Debug.Log(_message);
    }
    public void ClearMessage()
    {
        _message = "";
    }
}
