using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class RoundController : MonoBehaviour
{
    public GameObject roundFinished;
    public GameObject gameFinished;
    private GameObject blueTeamHealthBar;
    private GameObject redTeamHealthBar;
    private GameObject playerAmmo;
    private GameObject playerCrosshair;
    private GameObject playerEnergy;
    private GameObject playerSmokeContainer;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI blueScore;
    public TextMeshProUGUI redScore;
    public TextMeshProUGUI countDownText;
    public TextMeshProUGUI roundText_2;
    public TextMeshProUGUI blueScore_2;
    public TextMeshProUGUI redScore_2;
    public TextMeshProUGUI countDownText_2;
    public int screenTime = 5;
    public int endScreenTime = 15;
    public int roundsToWin = 2;
    private int blueWins = 0;
    private int redWins = 0;
    private string round_text = "";

    Gun gunController;
    public AudioSource whatMemeAudio;
    public GameObject[] playerGameObjects;
    private void Start()
    {
        blueWins = 0;
        redWins = 0;
        gunController = FindObjectOfType<Gun>();
        blueTeamHealthBar = GameObject.Find("HealthBarBlue");
        redTeamHealthBar = GameObject.Find("HealthBarRed");
        playerAmmo = GameObject.Find("Ammo");
        playerCrosshair = GameObject.Find("Crosshair");
        playerEnergy = GameObject.Find("Energy");
        playerSmokeContainer = GameObject.Find("SmokeContainer");
    }
    public void InitNewRound()
    {
        if (PlayerPrefs.GetInt("roundWinner") == 0) {
            round_text = "Round Winner: Blue";
            blueWins += 1;
        }
        else if (PlayerPrefs.GetInt("roundWinner") == 1) {
            round_text = "Round Winner: Red";
            redWins += 1;
        }
        if (blueWins == roundsToWin) {
            round_text = "BLUE TEAM";
            PlayerPrefs.SetInt("gameWinner", 0);
            StartCoroutine(WinnerCoroutine(endScreenTime));
        }
        else if (redWins == roundsToWin) {
            round_text = "RED TEAM";
            PlayerPrefs.SetInt("gameWinner", 1);
            StartCoroutine(WinnerCoroutine(endScreenTime));
        }
        else {
            StartCoroutine(WaitForSecs(screenTime));
        }
    }
    IEnumerator WaitForSecs(int secs)
    {
        roundText.text = round_text;
        blueScore.text = blueWins.ToString();
        redScore.text = redWins.ToString();
        Pause();
        roundFinished.SetActive(true);
        while (secs > 0) {
            countDownText.text = "Time till next round: " + secs.ToString();
            yield return new WaitForSecondsRealtime(1);
            secs -= 1;
        }
        roundFinished.SetActive(false);
        Resume();
    }

    IEnumerator WinnerCoroutine(int secs)
    {
        whatMemeAudio.Play();
        roundText_2.text = round_text;
        blueScore_2.text = blueWins.ToString();
        redScore_2.text = redWins.ToString();
        Pause();
        gameFinished.SetActive(true);
        while (secs > 0) {
            countDownText_2.text = "";
            yield return new WaitForSecondsRealtime(1);
            secs -= 1;
        }
        Resume();
        gameFinished.SetActive(false);
        whatMemeAudio.Stop();
        if (PhotonNetwork.IsMasterClient) {
            PhotonNetwork.LoadLevel("Lobby");
        }
    }
    void Pause()
    {
        blueTeamHealthBar.SetActive(false);
        redTeamHealthBar.SetActive(false);
        playerAmmo.SetActive(false);
        playerCrosshair.SetActive(false);
        playerEnergy.SetActive(false);
        playerSmokeContainer.SetActive(false);
        gunController.ReloadInstantly();
        Time.timeScale = 0f;
    }

    void Resume()
    {
        blueTeamHealthBar.SetActive(true);
        redTeamHealthBar.SetActive(true);
        playerAmmo.SetActive(true);
        playerCrosshair.SetActive(true);
        playerEnergy.SetActive(true);
        playerSmokeContainer.SetActive(true);
        Time.timeScale = 1f;
    }
}
