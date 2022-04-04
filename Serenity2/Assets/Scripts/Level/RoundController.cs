using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class RoundController : MonoBehaviour
{
    public GameObject roundFinished;
    private GameObject blueTeamHealthBar;
    private GameObject redTeamHealthBar;
    private GameObject playerAmmo;
    private GameObject playerCrosshair;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI blueScore;
    public TextMeshProUGUI redScore;

    public TextMeshProUGUI countDownText;
    public int screenTime;
    public int roundsToWin = 2;
    private int blueWins = 0;
    private int redWins = 0;
    private string round_text = "";

    Gun gunController;

    private PhotonView _photonView;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }
    private void Start()
    {
        gunController = FindObjectOfType<Gun>();
        blueTeamHealthBar = GameObject.Find("HealthBarBlue");
        redTeamHealthBar = GameObject.Find("HealthBarRed");
        playerAmmo = GameObject.Find("Ammo");
        playerCrosshair = GameObject.Find("Crosshair");
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
            round_text = "Game Winner: Blue";
            PlayerPrefs.SetInt("gameWinner", 0);
            StartCoroutine(WinnerCoroutine(10));
        }
        else if (redWins == roundsToWin) {
            round_text = "Game Winner: Red";
            PlayerPrefs.SetInt("gameWinner", 1);
            StartCoroutine(WinnerCoroutine(10));
        }
        else {
            StartCoroutine(WaitForSecs(screenTime));
        }
    }
    IEnumerator WaitForSecs(int secs)
    {
        Pause();
        Cursor.lockState = CursorLockMode.None;
        while (secs > 0) {
            countDownText.text = "Time till next round: " + secs.ToString();
            yield return new WaitForSecondsRealtime(1);
            secs -= 1;
        }
        Cursor.lockState = CursorLockMode.Locked;
        Resume();
    }

    IEnumerator WinnerCoroutine(int secs)
    {
        Pause();
        Cursor.lockState = CursorLockMode.None;
        while (secs > 0) {
            countDownText.text = "";
            yield return new WaitForSecondsRealtime(1);
            secs -= 1;
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Resume();
        PhotonNetwork.LoadLevel("Lobby");
    }
    void Pause()
    {
        roundText.text = round_text;
        blueScore.text = blueWins.ToString();
        redScore.text = redWins.ToString();
        countDownText.text = "";
        blueTeamHealthBar.SetActive(false);
        redTeamHealthBar.SetActive(false);
        playerAmmo.SetActive(false);
        playerCrosshair.SetActive(false);
        gunController.ReloadInstantly();
        roundFinished.SetActive(true);
        Time.timeScale = 0f;
    }

    void Resume()
    {
        roundFinished.SetActive(false);
        blueTeamHealthBar.SetActive(true);
        redTeamHealthBar.SetActive(true);
        playerAmmo.SetActive(true);
        playerCrosshair.SetActive(true);
        Time.timeScale = 1f;
    }
}
