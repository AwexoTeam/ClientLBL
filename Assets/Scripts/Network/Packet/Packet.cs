using System.Collections;
using System.Collections.Generic;
using System.IO;
using Telepathy;
using UnityEngine;

public enum PacketType
{
    Unknown,
    LoginRequest,
    LoginAnswer,
    CharacterCreationRequest,
    CharacterCreationAnswer,
    MovementRequest,
    PositionUpdate,
    PlayerSyncRequest,
    PlayerDisconnected,
    UmaCharacterPacket,
}

public abstract class Packet
{
    public int id { get { return NetworkManager.instance.localPlayerID; } }
    public PacketType type;
    public byte[] buffer;
    public BinaryWriter writer;
    
    private MemoryStream stream;

    public void BeginWrite()
    {
        stream = new MemoryStream();
        writer = new BinaryWriter(stream);

        writer.Write((int)type);
        writer.Write(id);
    }
    public void EndWrite()
    {
        buffer = stream.ToArray();
        stream.Close();
    }

    public abstract void Serialize();
    public abstract void Deserialize(BinaryReader reader);

    public void Send() { NetworkManager.instance.Send(this); }
}

