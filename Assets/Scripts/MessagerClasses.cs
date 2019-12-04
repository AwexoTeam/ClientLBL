using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ZeroFormatter;

public enum PacketType
{
    Unknown,
    LoginRequest,
    LoginAnswer,
    MovementRequest,
    PositionUpdate,
    PlayerSyncRequest,
    PlayerDisconnected,
}

public class Packet
{
    public string guid;
    public PacketType type;
    public byte[] buffer;
    public BinaryWriter writer;

    public Packet(string _guid, PacketType _type)
    {
        guid = _guid;
        type = _type;
    }

    public Packet(PacketType _type)
    {
        guid = NetworkManager.instance.GUID;
        type = _type;
    }

    private MemoryStream stream;

    public void BeginWrite()
    {
        stream = new MemoryStream();
        writer = new BinaryWriter(stream);

        writer.Write((int)type);
        writer.Write(guid);
    }
    public void EndWrite()
    {
        buffer = stream.ToArray();
        stream.Close();
    }

}
