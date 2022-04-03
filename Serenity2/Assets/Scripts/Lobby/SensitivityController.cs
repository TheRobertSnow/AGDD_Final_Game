using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SensitivityController : MonoBehaviour
{
    public Slider slider;
    public TMP_InputField sensitivityInput;
    private void Start()
    {
        // Slider properties
        slider.onValueChanged.AddListener(OnSliderValueChanged);
        slider.value = 1f;
        slider.minValue = 0.1f;
        slider.maxValue = 10f;

        // Input properties
        sensitivityInput.onValueChanged.AddListener(delegate { OnInputValueChanged();  });
    }

    void OnInputValueChanged()
    {
        float newVal = Mathf.Clamp(Mathf.Round(float.Parse(sensitivityInput.text) * 100f) / 100f, 0.01f, 10f);
        PlayerPrefs.SetFloat("sensitivity", newVal);
        slider.value = PlayerPrefs.GetFloat("sensitivity");
    }

    void OnSliderValueChanged(float value)
    {
        // globally change the percentage volume of all audio sources
        PlayerPrefs.SetFloat("sensitivity", (Mathf.Round(value * 100f) / 100f));
        sensitivityInput.text = PlayerPrefs.GetFloat("sensitivity").ToString();
    }
}
