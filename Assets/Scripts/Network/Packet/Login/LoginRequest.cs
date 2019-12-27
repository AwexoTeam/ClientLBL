using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoginRequest : Packet
{
    public string username;
    public string password;

    public LoginRequest(string username, string password)
    {
        type = PacketType.LoginRequest;
        this.username = username;
        this.password = password;
    }

    public override void Serialize()
    {
        BeginWrite();
        writer.Write(username);
        writer.Write(password);
        EndWrite();
    }
}
