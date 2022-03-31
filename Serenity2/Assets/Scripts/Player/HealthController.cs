using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    public Slider blueTeamSlider;
    public Slider redTeamSlider;
    public int maxHealth = 100;
    private int blueTeamHealth;
    private int redTeamHealth;

    private void Awake()
    {
        blueTeamHealth = maxHealth;
        redTeamHealth = maxHealth;
        Debug.Log(blueTeamSlider);
        blueTeamSlider.value = maxHealth;
        redTeamSlider.value = maxHealth;
    }
    public void SetHealthBlueTeam(int health)
    {
        blueTeamHealth = blueTeamHealth - health;
        blueTeamSlider.value = blueTeamHealth;
    }

    public void SetHealthRedTeam(int health)
    {
        redTeamHealth = redTeamHealth - health;
        redTeamSlider.value = redTeamHealth;
    }
}
