using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Telepathy;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum NetworkState
{
    NotLoggedIn,
    StartingUp,
    Handshake,
    ProcessingLogin,
    CharacterScreen,
    LoggedIn,
}

public enum ConsoleLevel
{
    Minimal,
    Default,
    Verbose,
    Debug,
}

public class NetworkManager : MonoBehaviour
{
    [Header("Base Server Info")]
    public static NetworkManager instance;
    //TODO: static local player variable.
    public Client client;
    public int localPlayerID;
    public ConsoleLevel level;
    public NetworkState state;
    public string ip;
    public GameObject playerPrefab;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }

    private void FixedUpdate()
    {
        if(state == NetworkState.StartingUp)
        {            
            client = new Client();
            client.Connect("127.0.0.1", 1337);
            if (client.Connected)
            {
                state = NetworkState.Handshake;
            }
        }
        if((int)state > 1) { MessageLoop(); } 
    }

    private void MessageLoop()
    {
        if (client.Connected)
        {
            Message msg;
            while (client.GetNextMessage(out msg))
            {
                if (msg.eventType == Telepathy.EventType.Data)
                {
                    MemoryStream stream;
                    BinaryReader reader;

                    stream = new MemoryStream(msg.data);
                    reader = new BinaryReader(stream);

                    PacketType type = (PacketType)reader.ReadInt32();
                    int id = reader.ReadInt32();

                    Packet packet = GetPacketByType(type);
                    if(level >= ConsoleLevel.Debug)
                        Debug.Log("R: " + type);

                    packet.Deserialize(reader);
                    packet.OnRecieve(msg);
                    
                }
            }
        }
    }

    private Packet GetPacketByType(PacketType type)
    {
        switch (type)
        {
            case PacketType.LoginAnswer:
                return new LoginAnswer();

            case PacketType.CharacterCreationAnswer:
                return new CharacterCreationAnswer();

            case PacketType.PlayerSyncAnswer:
                return new PlayerSyncAnswer();

            case PacketType.PlayerMovementUpdate:
                return new PlayerMovementUpdate();

            default:
                return null;
        }
    }

    public void RequestLogin(string username, string password)
    {
        LoginRequest request = new LoginRequest(username, password);
        request.Serialize();
        request.Send();

        state = NetworkState.ProcessingLogin;
    }
    
    public void Send(Packet packet)
    {
        if (level >= ConsoleLevel.Debug)
            Debug.Log("S: " + packet.type);

        client.Send(packet.buffer);
    }

    private void OnLevelWasLoaded(int level)
    {
        if(level == 2)
        {
            PlayerSyncRequest request = new PlayerSyncRequest();
            request.Serialize();
            Send(request);
        }
    }
}
