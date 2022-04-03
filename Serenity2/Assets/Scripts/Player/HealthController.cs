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

    public int hitsToWinRound = 1;
    private int blueTeamHealth;
    private int redTeamHealth;
    private PhotonView _photonView;
    RoundController roundController;

    public AudioSource pingAudio;
    public AudioSource damageAudio;

    private void Awake()
    {
        PlayerPrefs.SetInt("roundWinner", -1);
        roundController = FindObjectOfType<RoundController>();
        _photonView = GetComponent<PhotonView>();
        blueTeamSlider = GameObject.Find("SliderBlue").GetComponent<Slider>();
        redTeamSlider = GameObject.Find("SliderRed").GetComponent<Slider>();
        resetHealth();
    }
    public void damageBlueTeam()
    {
        blueTeamHealth = blueTeamHealth - (maxHealth/hitsToWinRound);
        blueTeamSlider.value = blueTeamHealth;
        playSound("blue");
        if (blueTeamHealth == 0) {
            PlayerPrefs.SetInt("roundWinner", 1);
            roundController.InitNewRound();
            resetHealth();
            //PhotonNetwork.LoadLevel("GameOver");
        }
    }

    public void damageRedTeam()
    {
        redTeamHealth = redTeamHealth - (maxHealth/hitsToWinRound);
        redTeamSlider.value = redTeamHealth;
        playSound("red");
        if (redTeamHealth == 0) {
            PlayerPrefs.SetInt("roundWinner", 0);
            roundController.InitNewRound();
            resetHealth();
            //PhotonNetwork.LoadLevel("GameOver");
        }
    }

    void resetHealth() {
        blueTeamHealth = maxHealth;
        redTeamHealth = maxHealth;
        blueTeamSlider.value = maxHealth;
        redTeamSlider.value = maxHealth;
    }


    private void playSound(string teamHit)
    {
        if (_myTeamHitTarget(teamHit))
        {
            pingAudio.Play();
        }
        else
        {
            damageAudio.Play();
        }
    }
    
    private bool _myTeamHitTarget(string teamHit)
    {
        int myTeam = (int) PhotonNetwork.LocalPlayer.CustomProperties["team"];
        if (teamHit == "blue" && myTeam == 1)
        {
            return true;
        } 
        if (teamHit == "red" && myTeam == 0)
        {
            return true;
        }

        return false;
    }
}
