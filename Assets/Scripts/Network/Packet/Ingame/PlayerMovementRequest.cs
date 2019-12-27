using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementRequest : Packet
{
    public float x;
    public float y;
    public float z;

    public PlayerMovementRequest(float x, float y, float z)
    {
        type = PacketType.PlayerMovementRequest;

        this.x = x;
        this.y = y;
        this.z = z;
    }

    public override void Serialize()
    {
        BeginWrite();
        writer.Write(x);
        writer.Write(y);
        writer.Write(z);
        EndWrite();
    }
}
