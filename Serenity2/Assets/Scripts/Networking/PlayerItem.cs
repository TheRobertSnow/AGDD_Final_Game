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

    public ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    public GameObject switchTeamButton;

    private PhotonView _view;
    Player player;

    private void Awake()
    {
        _view = GetComponent<PhotonView>();
    }

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
        UpdatePlayerItem(player);
    }

    public void OnClickSwitchTeams()
    {
        playerProperties["team"] = (int)playerProperties["team"] == 0 ? 1 : 0;
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable playerProperties)
    {
        if (player == targetPlayer)
        {
            UpdatePlayerItem(targetPlayer);
        }
    }

    void UpdatePlayerItem(Player player)
    {
        if (player.CustomProperties.ContainsKey("team"))
        {
            playerProperties["team"] = (int)player.CustomProperties["team"];
        }
        else
        {
            playerProperties["team"] = 0;
        }
    }
}
