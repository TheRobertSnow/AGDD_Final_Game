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
    public TextMeshProUGUI readyText;
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
        readyText.text = (bool)_player.CustomProperties["ready"] ? "Ready" : "Not ready";
        readyText.color = (bool)_player.CustomProperties["ready"] ? Color.green : Color.red;
        player = _player;
    }
}