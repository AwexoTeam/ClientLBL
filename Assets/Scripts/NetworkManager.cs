using System.Collections;
using System.Collections.Generic;
using System.IO;
using Telepathy;
using UnityEngine;
using ZeroFormatter;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;
    public MessageHandler messageHandler;
    public Camera mainCamera;

    public Material playerMat;
    public Material notPlayerMat;
    public GameObject playerprefab;

    public Client client = new Client();
    bool hasNotSent = true;

    public string GUID;

    private MemoryStream stream;
    private BinaryReader reader;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            return;

        Application.runInBackground = true;
        stream = new MemoryStream();
        reader = new BinaryReader(stream);

        // use Debug.Log functions for Telepathy so we can see it in the console
        Telepathy.Logger.Log = Debug.Log;
        Telepathy.Logger.LogWarning = Debug.LogWarning;
        Telepathy.Logger.LogError = Debug.LogError;
        
    }

    private void FixedUpdate()
    {
        if (client.Connected)
        {
            Message msg;
            while (client.GetNextMessage(out msg))
            {
                switch (msg.eventType)
                {
                    case Telepathy.EventType.Connected:

                        break;
                    case Telepathy.EventType.Data:
                        stream = new MemoryStream(msg.data);
                        reader = new BinaryReader(stream);

                        PacketType type = (PacketType)reader.ReadInt32();
                        string gID = reader.ReadString();

                        messageHandler.HandleMessage(type, gID, reader, msg);
                        break;
                    case Telepathy.EventType.Disconnected:

                        break;
                }
            }
        }
    }

    private void OnGUI()
    {
        if (!client.Connected)
        {
            if (GUILayout.Button("Join"))
            {
                client.Connect("localhost", 1337);
            }
        }
        else
        {
            if (hasNotSent)
            {
                hasNotSent = false;

                Packet packet = new Packet("Client", PacketType.LoginRequest);
                packet.BeginWrite();
                packet.EndWrite();

                client.Send(packet.buffer);
            }
        }
    }

    public void Send(Packet packet)
    {
        client.Send(packet.buffer);
    }

    void OnApplicationQuit()
    {
        GUID = "null";
        client.Disconnect();
    }
}
