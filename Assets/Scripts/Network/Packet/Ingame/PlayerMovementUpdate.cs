using System.Collections;
using System.Collections.Generic;
using System.IO;
using Telepathy;
using UnityEngine;

public class PlayerMovementUpdate : Packet
{
    public int characterID;
    public float x;
    public float y;
    public float z;

    public PlayerMovementUpdate() { type = PacketType.PlayerMovementUpdate; }

    public override void Deserialize(BinaryReader reader)
    {
        characterID = reader.ReadInt32();
        x = reader.ReadSingle();
        y = reader.ReadSingle();
        z = reader.ReadSingle();
    }

    public override void OnRecieve(Message msg)
    {
        int index = PlayerManager.instance.players.FindIndex(p => p.characterID == characterID);
        
        PlayerManager.instance.players[index].targetPosition = new Vector3(x, y, z);
        
    }
}
