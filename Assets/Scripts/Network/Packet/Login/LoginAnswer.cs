using System.Collections;
using System.Collections.Generic;
using System.IO;
using Telepathy;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginAnswer : Packet
{
    public bool canLogin;
    public bool hasCharacter;
    public int errorCode;
    public int characterID;

    public override void Deserialize(BinaryReader reader)
    {
        type = PacketType.LoginAnswer;
        canLogin = reader.ReadBoolean();
        hasCharacter = reader.ReadBoolean();
        errorCode = reader.ReadInt32();
        characterID = reader.ReadInt32();
    }

    public override void OnRecieve(Message msg)
    {
        int sceneID = 0;

        NetworkManager.instance.localPlayerID = characterID;

        if (canLogin)
        {
            if (hasCharacter) { sceneID = 2; }
            else { sceneID = 1; }

            SceneManager.LoadScene(sceneID);
        }
        else
        {
            NetworkManager.instance.client.Disconnect();
            NetworkManager.instance.state = NetworkState.NotLoggedIn;
        }
    }
}
