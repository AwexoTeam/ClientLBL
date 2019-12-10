using System.Collections;
using System.Collections.Generic;
using System.IO;
using Telepathy;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginHandler : MonoBehaviour
{
    public InputField username;
    public InputField password;
    public InputField ip;

    public Button loginButton;
    public Text statusText;

    public NetworkManager manager;

    public bool startLogin { get; set; }

    private void Start()
    {
        Application.runInBackground = true;
        Telepathy.Logger.Log = Debug.Log;
        Telepathy.Logger.LogWarning = Debug.LogWarning;
        Telepathy.Logger.LogError = Debug.LogError;
        manager = NetworkManager.instance;
    }

    private void Update()
    {
        if (manager == null)
            return;

        statusText.text = "Status: " + manager.state;
        loginButton.interactable = manager.state != NetworkState.ProcessingLogin;

        if (startLogin)
        {
            manager.ip = ip.text;
            manager.state = NetworkState.StartingUp;
            startLogin = false;
        }

        if(manager.state == NetworkState.Handshake)
        {
            manager.RequestLogin(username.text, password.text);
            
        }
    }
    
}
