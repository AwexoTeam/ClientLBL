using System.Collections;
using System.Collections.Generic;
using System.IO;
using Telepathy;
using UnityEngine;

public class PlayerSyncAnswer : Packet
{
    public string name;
    public string genativPronoun;
    public string referalPronoun;

    public int bodyType;

    public float height;
    public float weight;

    public float targetX;
    public float targetY;
    public float targetZ;

    public int characterID;
    
    public PlayerSyncAnswer()
    {
        type = PacketType.PlayerSyncAnswer;
    }

    public override void Deserialize(BinaryReader reader)
    {
        name = reader.ReadString();
        genativPronoun = reader.ReadString();
        referalPronoun = reader.ReadString();

        bodyType = reader.ReadInt32();

        height = reader.ReadSingle();
        weight = reader.ReadSingle();

        targetX = reader.ReadSingle();
        targetY = reader.ReadSingle();
        targetZ = reader.ReadSingle();

        characterID = reader.ReadInt32();
    }

    public override void OnRecieve(Message msg)
    {
        this.Serialize();
        PlayerManager.instance.DoPlayerSync(this);
    }
}
