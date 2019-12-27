using System.Collections;
using System.Collections.Generic;
using UMA.CharacterSystem;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCreation : MonoBehaviour
{
    public Camera cam;
    public DynamicCharacterAvatar avatar;
    public static CharacterCreation instance;
    public InputField characterName;
    public Dictionary<UmaSliderType, float> characterValues;
    public ToggleGroup toggleGroup;

    public string[] races;
    private int raceIndex;
    
    [Range(0.01f,1f)]
    public float characterUpdateFreq;

    void Awake()
    {
        instance = this;
        characterValues = new Dictionary<UmaSliderType, float>();

        InvokeRepeating("OnCharacterTick", 1, characterUpdateFreq);
    }

    public void OnCharacterTick()
    {
        foreach (UmaSliderType type in characterValues.Keys)
        {
            float typeValue = characterValues[type];
            avatar.GetDNA()[type.ToString()].Set(typeValue);
        }

        avatar.BuildCharacter();
    }
    
    public void OnCreate()
    {

        CharacterCreationRequest characterPacket = new CharacterCreationRequest
        {
            name = characterName.text,
            genativPronoun = "his",
            referalPronoun = "him",
            bodyType = raceIndex
        };
        
        characterPacket.height = characterValues[UmaSliderType.height];
        characterPacket.weight = characterValues[UmaSliderType.belly];
        characterPacket.Serialize();

        characterPacket.Send();
    }

    public void ChangeBodyType(int index)
    {
        raceIndex = index;
        avatar.ChangeRace(races[index]);
    }
}
