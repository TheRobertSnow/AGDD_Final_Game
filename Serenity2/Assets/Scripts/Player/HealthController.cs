using System.Collections;
using System.Collections.Generic;
using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    private Slider blueTeamSlider;
    private Slider redTeamSlider;
    public int maxHealth = 100;
    private int blueTeamHealth;
    private int redTeamHealth;

    private PhotonView _photonView;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
        blueTeamSlider = GameObject.Find("HealthBarBlue").GetComponent<Slider>();
        redTeamSlider = GameObject.Find("HealthBarRed").GetComponent<Slider>();
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
