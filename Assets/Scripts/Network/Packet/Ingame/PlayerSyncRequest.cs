using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSyncRequest : Packet
{
    public PlayerSyncRequest() { type = PacketType.PlayerSyncRequest; }

    public override void Serialize()
    {
        BeginWrite();
        EndWrite();
    }
}
