using UnityEngine;
using Photon.Realtime;
using TMPro;

public class SpectatorItem : MonoBehaviour
{
    public TextMeshProUGUI playerNameText;
    Player player;

    public void SetPlayerName(Player _player)
    {
        playerNameText.text = _player.NickName;
        player = _player;
    }
}
