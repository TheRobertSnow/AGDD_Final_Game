using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class RoundController : MonoBehaviour
{
    public GameObject roundFinished;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI blueScore;
    public TextMeshProUGUI redScore;

    public TextMeshProUGUI countDownText;
    public int screenTime;
    public int roundsToWin = 2;
    private int blueWins = 0;
    private int redWins = 0;

    Gun gunController;

    private void Start()
    {
        gunController = FindObjectOfType<Gun>();
    }
    public void InitNewRound()
    {
        if (PlayerPrefs.GetInt("roundWinner") == 0) {
            roundText.text = "Round Winner: Blue";
            blueWins += 1;
        }
        else if (PlayerPrefs.GetInt("roundWinner") == 1) {
            roundText.text = "Round Winner: Red";
            redWins += 1;
        }
        blueScore.text = blueWins.ToString();
        redScore.text = redWins.ToString();
        if (redWins == roundsToWin) {
            PlayerPrefs.SetInt("gameWinner", 0);
            PhotonNetwork.LoadLevel("GameOver");
        }
        if (blueWins == roundsToWin) {
            PlayerPrefs.SetInt("gameWinner", 1);
            PhotonNetwork.LoadLevel("GameOver");
        }
        StartCoroutine(WaitForSecs(screenTime));
    }
    IEnumerator WaitForSecs(int secs)
    {
        Pause();
        gunController.ReloadInstantly();
        roundFinished.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        while (secs > 0) {
            countDownText.text = "Time till next round: " + secs.ToString();
            yield return new WaitForSecondsRealtime(1);
            secs -= 1;
        }
        Cursor.lockState = CursorLockMode.Locked;
        roundFinished.SetActive(false);
        Resume();
    }
    void Pause()
    {
        Time.timeScale = 0f;
    }

    void Resume()
    {
        Time.timeScale = 1f;
    }
}
