using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoginRequest : Packet
{
    public string username;
    public string password;

    public LoginRequest(string _username, string _password)
    {
        type = PacketType.LoginRequest;
        username = _username;
        password = _password;
    }

    public override void Serialize()
    {
        BeginWrite();
        writer.Write(username);
        writer.Write(password);
        EndWrite();
    }

    public override void Deserialize(BinaryReader reader) { }
}
