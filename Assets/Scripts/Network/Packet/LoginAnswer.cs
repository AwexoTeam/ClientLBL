using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoginAnswer : Packet
{
    public int characterID;
    public bool canLogin;
    public bool hasCharacter;
    public int errorCode;

    public override void Serialize() { }

    public override void Deserialize(BinaryReader reader)
    {
        canLogin = reader.ReadBoolean();
        hasCharacter = reader.ReadBoolean();
        errorCode = reader.ReadInt32();
        characterID = reader.ReadInt32();
    }
}
