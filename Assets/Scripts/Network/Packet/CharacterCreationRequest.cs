using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterCreationRequest : Packet
{
    public string name;
    public string pronouns;
    public int bodyType;

    public float height;
    public float weight;
    
    public CharacterCreationRequest(string _name, string _pronouns, int _bodyType)
    {
        type = PacketType.CharacterCreationRequest;
        name = _name;
        pronouns = _pronouns;
        bodyType = _bodyType;
    }

    public override void Serialize()
    {
        BeginWrite();
        writer.Write(name);
        writer.Write(pronouns.ToLower());
        writer.Write(bodyType);
        writer.Write(height);
        writer.Write(weight);
        EndWrite();
    }


    public override void Deserialize(BinaryReader reader) { }
}
