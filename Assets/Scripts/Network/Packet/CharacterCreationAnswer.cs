using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterCreationAnswer : Packet
{
    public bool canCreate;
    public int errorCode;
    
    public override void Serialize() { }
    public override void Deserialize(BinaryReader reader)
    {
        canCreate = reader.ReadBoolean();
        errorCode = reader.ReadInt32();
    }
}
