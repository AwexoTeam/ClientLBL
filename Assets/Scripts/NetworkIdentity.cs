using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkIdentity : MonoBehaviour
{
    public string GUID;
    public bool isOwner { get { return GUID == NetworkManager.instance.GUID; } }
    
}
