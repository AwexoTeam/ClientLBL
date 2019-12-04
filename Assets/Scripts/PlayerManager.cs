using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [Range(0, 1)]
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

    public void RegisterPlayer(PlayerEntity playerObject, string guid)
    {
        otherPlayers.Add(guid, playerObject);
    }

    public void UpdateMovement(string guid, Vector3 pos)
    {
        if (otherPlayers.ContainsKey(guid))
        {
            otherPlayers[guid].agent.SetDestination(pos);
        }
    }
}
