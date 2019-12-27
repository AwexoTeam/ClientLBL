using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterCreationRequest : Packet
{
    public string name;
    public string genativPronoun;
    public string referalPronoun;

    public int bodyType;

    public float height;
    public float weight;

    public CharacterCreationRequest()
    {
        type = PacketType.CharacterCreationRequest;
    }

    public override void Serialize()
    {
        BeginWrite();
        writer.Write(name);
        writer.Write(genativPronoun);
        writer.Write(referalPronoun);
        
        writer.Write(bodyType);
        
        writer.Write(height);
        writer.Write(weight);
        EndWrite();
    }
}
