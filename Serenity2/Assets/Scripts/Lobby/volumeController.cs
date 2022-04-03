using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class volumeController : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI volumeValue;

    private void Start()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        slider.value = 1f;
    }

    void OnSliderValueChanged(float value)
    {
        // globally change the percentage volume of all audio sources
        AudioListener.volume = value;
        volumeValue.text = Mathf.Round(value * 100).ToString() + "%";
    }
}
