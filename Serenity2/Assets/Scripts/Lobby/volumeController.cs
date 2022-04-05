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
        if (PlayerPrefs.HasKey("volume")){
            slider.value = PlayerPrefs.GetFloat("volume");
            AudioListener.volume = PlayerPrefs.GetFloat("volume");
            volumeValue.text = Mathf.Round(PlayerPrefs.GetFloat("volume") * 100).ToString() + "%";
        }
        else {
            slider.value = 0.5f;
            AudioListener.volume = 0.5f;
            volumeValue.text = Mathf.Round(0.5f * 100).ToString() + "%";
        }
    }

    void OnSliderValueChanged(float value)
    {
        // globally change the percentage volume of all audio sources
        PlayerPrefs.SetFloat("volume", value);
        AudioListener.volume = value;
        volumeValue.text = Mathf.Round(value * 100).ToString() + "%";
    }
}