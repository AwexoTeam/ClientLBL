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
            case PacketType.MovementUpdate:
                HandleMovementUpdate(reader);
                break;
            case PacketType.PlayerConnected:
                HandlePlayerConnected(reader);
                break;
            case PacketType.PlayerDisconnected:
                break;
            default:
                break;
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

    private void HandlePlayerConnected(BinaryReader reader)
    {
        string guid = reader.ReadString();
        if (guid != NetworkManager.instance.GUID)
        {
            GameObject gObj = Instantiate(NetworkManager.instance.playerprefab);
            gObj.GetComponent<MeshRenderer>().material = NetworkManager.instance.notPlayerMat;
            PlayerEntity entity = gObj.GetComponent<PlayerEntity>();
            entity.GUID = guid;

            PlayerManager.instance.RegisterPlayer(entity, guid);
        }
    }

    public void HandleLoginAnswer(BinaryReader reader)
    {
        string guid = "null";

        if (reader.ReadBoolean())
        {
            guid = reader.ReadString();
            GameObject gObj = Instantiate(NetworkManager.instance.playerprefab);
            gObj.GetComponent<MeshRenderer>().material = NetworkManager.instance.playerMat;
            PlayerEntity entity = gObj.GetComponent<PlayerEntity>();
            entity.GUID = guid;

            PlayerManager.instance.RegisterPlayer(entity, guid);
            gObj.AddComponent<Player>().entityData = gObj.GetComponent<PlayerEntity>();
        }
        else { Debug.Log("Login was not successful."); }
        NetworkManager.instance.GUID = guid;
    }
}
