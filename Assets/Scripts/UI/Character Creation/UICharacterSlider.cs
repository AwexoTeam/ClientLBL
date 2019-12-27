using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterSlider : MonoBehaviour
{
    public UmaSliderType type;
    public float max = 1;
    public float min = 0;
    public float currValue { get { return slider.value; } }
    public Slider slider;

    private void Start()
    {
        slider.onValueChanged.AddListener(delegate { OnValueChanged(); });
        CharacterCreation.instance.characterValues.Add(type, 0);

        slider.maxValue = max;
        slider.minValue = min;
        slider.value = min+((max-min)/2);
    }

    private void OnValueChanged()
    {
        CharacterCreation.instance.characterValues[type] = currValue;
    }
}