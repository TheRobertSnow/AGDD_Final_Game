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
    public Material blueTeamMaterial;
    public Material redTeamMaterial;
    public List<Material> playerSkins;
    public MeshRenderer playerModel;

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
        Material[] materials = new Material[2];
        System.Array.Copy(playerModel.sharedMaterials, materials, playerModel.sharedMaterials.Length);
        materials[0] = (int)_player.CustomProperties["team"] == 0 ? blueTeamMaterial : redTeamMaterial;
        materials[1] = playerSkins[(int)_player.CustomProperties["skin"]];
        playerModel.sharedMaterials = materials;
        readyText.text = (bool)_player.CustomProperties["ready"] ? "Ready" : "Not ready";
        readyText.color = (bool)_player.CustomProperties["ready"] ? Color.green : Color.red;
        player = _player;
    }
}