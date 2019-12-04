using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Telepathy;
using UnityEngine;

public class MessageHandler : MonoBehaviour
{
    public void HandleMessage(PacketType type, string guid, BinaryReader reader, Message msg)
    {
        switch (type)
        {
            case PacketType.LoginAnswer:
                HandleLoginAnswer(reader);
                break;
            case PacketType.MovementRequest:
                HandleMovementUpdate(reader);
                break;
            case PacketType.PlayerDisconnected:
                break;
            case PacketType.PlayerSyncRequest:
                HandlePlayerSyncRequest(reader);
                break;
            default:
                break;
        }
    }

    private void HandlePlayerSyncRequest(BinaryReader reader)
    {
        int length = reader.ReadInt32();
        Debug.Log("Length of: " + length);

        for (int i = 0; i < length; i++)
        {
            string guid = reader.ReadString();

            float cPosX = reader.ReadSingle();
            float cPosY = reader.ReadSingle();
            float cPosZ = reader.ReadSingle();

            float tPosX = reader.ReadSingle();
            float tPosY = reader.ReadSingle();
            float tPosZ = reader.ReadSingle();

            Vector3 currPos = new Vector3(cPosX, cPosY, cPosZ);
            Vector3 targetPos = new Vector3(tPosX, tPosY, tPosZ);

            if (PlayerManager.instance.DoesPlayerExist(guid))
            {
                Debug.Log("Player is not initiated");
                PlayerManager.instance.UpdateMovement(guid, targetPos);
            }
            else
            {
                Debug.Log("Player is not initiated");
                PlayerManager.instance.RegisterPlayer(guid, currPos, targetPos);
            }
        }
    }

    private void HandleMovementUpdate(BinaryReader reader)
    {
        string guid = reader.ReadString();
        float x = reader.ReadSingle();
        float y = reader.ReadSingle();
        float z = reader.ReadSingle();

        Vector3 pos = new Vector3(x, y, z);

        PlayerManager.instance.UpdateMovement(guid, pos);
    }
    
    public void HandleLoginAnswer(BinaryReader reader)
    {
        string guid = "null";

        if (reader.ReadBoolean())
        {
            guid = reader.ReadString();
            NetworkManager.instance.GUID = guid;
        }
        else { Debug.Log("Login was not successful."); }
    }
}
