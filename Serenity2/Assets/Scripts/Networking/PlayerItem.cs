using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class PlayerItem : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI playerName;

    public GameObject leftArrowButton;
    public GameObject rightArrowButton;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    public TextMeshProUGUI currentTeam;
    public string[] teams;

    Player player;
    int blueTeamCount = 0;
    int redTeamCount = 0;

    public void setPlayerInfo(Player _player)
    {
        playerName.text = _player.NickName;
        player = _player;
        player.CustomProperties["team"] = 0;
        player.CustomProperties["spawnPoint"] = 0;
        UpdatePlayerItem(player);
    }

    public void onClickLeftArrow()
    {
        Debug.Log("OnleftClick");
        if ((int)playerProperties["team"] == 0)
        {
            playerProperties["team"] = teams.Length - 1;
        }
        else {
            playerProperties["team"] = (int)playerProperties["team"] - 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }
    public void onClickRightArrow()
    {
        if ((int)playerProperties["team"] == teams.Length - 1)
        {
            playerProperties["team"] = 0;
        }
        else {
            playerProperties["team"] = (int)playerProperties["team"] + 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable playerProperties)
    {
        Debug.Log("player props updated");
        if (player == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
        }
    }

    void UpdatePlayerItem(Player player)
    {
        if (player.CustomProperties.ContainsKey("team"))
        {
            currentTeam.text = teams[(int)player.CustomProperties["team"]];
            playerProperties["team"] = (int)player.CustomProperties["team"];
        }
        else
        {
            playerProperties["team"] = 0;
        }
        if(player.CustomProperties.ContainsKey("spawnPoint"))
        {
            for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
            {
                var team = (int)PhotonNetwork.PlayerList[i].CustomProperties["team"];
                if (team == 0) blueTeamCount++;
                else redTeamCount++;
            }
            if ((int)player.CustomProperties["team"] == 0)
            {
                playerProperties["spawnPoint"] = blueTeamCount;
                Debug.Log("Blue team spawn point:" + blueTeamCount);
            }
            else
            {
                playerProperties["spawnPoint"] = redTeamCount;
                Debug.Log("Red team spawn point:" + redTeamCount);
            }
            blueTeamCount = 0;
            redTeamCount = 0;
        }
    }
}
