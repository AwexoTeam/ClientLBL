using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [Range(0.01f, 1)]
    public float updateFrequency;

    private Dictionary<string, PlayerEntity> otherPlayers;

    private void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            return;

        otherPlayers = new Dictionary<string, PlayerEntity>();
    }

    public bool DoesPlayerExist(string guid) { return otherPlayers.ContainsKey(guid); }

    public void RegisterPlayer(string guid, Vector3 currPos, Vector3 targetPos)
    {
        PlayerEntity entity = CreatePlayerPrefab(guid, currPos);
        if (entity != null)
        {
            otherPlayers.Add(guid, entity);
        }
    }

    public void UpdateMovement(string guid, Vector3 pos)
    {
        if (otherPlayers.ContainsKey(guid))
        {
            otherPlayers[guid].targetPosition = pos;
            otherPlayers[guid].UpdateDestination();
        }
    }

    public void UpdatePosition(string guid, Vector3 pos)
    {
        if (otherPlayers.ContainsKey(guid))
        {
            otherPlayers[guid].currPosition = pos;
        }
    }

    public void SyncPosition(Vector3 pos)
    {
        Packet packet = new Packet(PacketType.PositionUpdate);
        packet.BeginWrite();
        packet.writer.Write(pos.x);
        packet.writer.Write(pos.y);
        packet.writer.Write(pos.z);
        packet.EndWrite();

        NetworkManager.instance.Send(packet);
    }

    private PlayerEntity CreatePlayerPrefab(string guid, Vector3 currPos)
    {
        GameObject gObj = Instantiate(NetworkManager.instance.playerprefab);

        PlayerEntity entity = gObj.GetComponent<PlayerEntity>();
        entity.currPosition = currPos;
        entity.targetPosition = currPos;
        entity.GUID = guid;

        if(guid == NetworkManager.instance.GUID)
        {
            gObj.AddComponent<Player>().entityData = entity;
        }
        gObj.GetComponent<Renderer>().material = guid == NetworkManager.instance.GUID ? NetworkManager.instance.playerMat : NetworkManager.instance.notPlayerMat;
        

        return entity;
    }
}
