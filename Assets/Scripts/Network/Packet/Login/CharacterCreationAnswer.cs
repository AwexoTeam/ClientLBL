using System.Collections;
using System.Collections.Generic;
using System.IO;
using Telepathy;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum CharacterErrorCode
{
    Nothing,
    NameTaken,
    NameTooLong,
    ProfaneName,
    Other
}

public class CharacterCreationAnswer : Packet
{
    public CharacterErrorCode errorCode;

    public override void Deserialize(BinaryReader reader)
    {
        errorCode = (CharacterErrorCode)reader.ReadInt32();
    }

    public override void OnRecieve(Message msg)
    {
        if(errorCode == CharacterErrorCode.Nothing)
        {
            SceneManager.LoadScene(2);
        }
        else
        {
            //TODO: handle errors
        }
    }
}
