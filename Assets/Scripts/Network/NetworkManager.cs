using System.Collections;
using System.Collections.Generic;
using System.IO;
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

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    //TODO: static local player variable.
    public Client client;
    public int localPlayerID;

    public NetworkState state;
    public string ip;

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

                    switch (type)
                    {
                        case PacketType.Unknown:
                            break;
                        case PacketType.LoginAnswer:
                            ProcessLoginAnswer(reader);
                            break;
                        case PacketType.CharacterCreationAnswer:
                            ProcessCharacterAnswer(reader);
                            break;
                        case PacketType.PositionUpdate:
                            break;
                        case PacketType.PlayerDisconnected:
                            break;
                        case PacketType.UmaCharacterPacket:
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public void RequestLogin(string username, string password)
    {
        LoginRequest request = new LoginRequest(username, password);
        request.Serialize();
        request.Send();

        state = NetworkState.ProcessingLogin;
    }

    public void ProcessLoginAnswer(BinaryReader reader)
    {
        LoginAnswer answer = new LoginAnswer();
        answer.Deserialize(reader);
        
        if (answer.canLogin)
        {
            localPlayerID = answer.characterID;

            if (answer.hasCharacter)
            {
                state = NetworkState.LoggedIn;
                SceneManager.LoadScene(2);
            }
            else
            {
                state = NetworkState.CharacterScreen;
                SceneManager.LoadScene(1);
            }
        }
        else
        {
            state = NetworkState.NotLoggedIn;
            client.Disconnect();
            Debug.Log("Couldnt login");
        }
    }

    public void ProcessCharacterAnswer(BinaryReader reader)
    {
        CharacterCreationAnswer answer = new CharacterCreationAnswer();
        answer.Deserialize(reader);

        if (answer.canCreate)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            Debug.LogWarning("Name already taken or other error idfk demo build");
        }
    }

    public void Send(Packet packet)
    {
        client.Send(packet.buffer);
    }
}
