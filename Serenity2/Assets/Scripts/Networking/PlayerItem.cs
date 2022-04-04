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
    Player player;

    private void Start()
    {
        if (PhotonNetwork.LocalPlayer.NickName == playerName.text)
        {
            playerName.fontStyle = FontStyles.Bold;  
        }
    }

    public void SetPlayerInfo(Player _player)
    {
        playerName.text = _player.NickName;
        player = _player;

    }

}

/*
 * 
    public void SwitchTeams()
    {
        playerProperties["team"] = (int)playerProperties["team"] == 0 ? 1 : 0;
        playerProperties["new"] = false;
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
        playerProperties["team"] = (int)player.CustomProperties["team"];
        playerProperties["new"] = (bool)player.CustomProperties["new"];
    }
 * 
 * */