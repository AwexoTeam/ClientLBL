using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    public List<Player> players;

    public Player localPlayer
    {
        get
        {
            return players.Find(x => x.characterID == NetworkManager.instance.localPlayerID);
        }
    }

    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        players = new List<Player>();
    }

    private Player SpawnPlayer(PlayerSyncAnswer answer)
    {
        GameObject prefab = NetworkManager.instance.playerPrefab;

        GameObject gObjPlayer = Instantiate(prefab);
        gObjPlayer.name = "Player: " + "";
        Player player = gObjPlayer.GetComponent<Player>();
        
        //TODO: set base location for new players.

        player.referalPronoun = answer.referalPronoun;
        player.displayName = answer.name;
        player.genativPrnoun = answer.genativPronoun;
        player.bodyType = answer.bodyType;
        player.characterID = answer.characterID;
        player.height = answer.height;
        player.weight = answer.weight;
        player.needUpdate = true;
        player.targetPosition = gObjPlayer.transform.position;

        players.Add(player);
        return player;
    }

    public void DoPlayerSync(PlayerSyncAnswer answer)
    {
        if(!players.Exists(p => p.characterID == answer.characterID))
        {
            Player newPlayer = new Player()
            {
                characterID = answer.characterID
            };

            SpawnPlayer(answer);
        }

        int index = players.FindIndex(p => p.characterID == answer.characterID);
        Player currPlayer = players[index];
        
        currPlayer.name = answer.name;
        currPlayer.genativPrnoun = answer.genativPronoun;
        currPlayer.referalPronoun = answer.referalPronoun;

        currPlayer.bodyType = answer.bodyType;
        currPlayer.height = answer.height;
        currPlayer.weight = answer.weight;

        currPlayer.targetPosition = new Vector3(answer.targetX, answer.targetY, answer.targetZ);
        currPlayer.needUpdate = true;

        players[index] = currPlayer;
    }

    public int GetCharacterIndex(Player player) { return GetCharacterIndexByID(player.characterID); }
    public int GetCharacterIndexByID(int id)
    {
        return players.FindIndex(p => p.characterID == id);
    }

    public bool HavePlayer(Player player) { return HavePlayer(player.characterID); }
    public bool HavePlayer(int id)
    {
        return players.Exists(p => p.characterID == id);
    }
}
