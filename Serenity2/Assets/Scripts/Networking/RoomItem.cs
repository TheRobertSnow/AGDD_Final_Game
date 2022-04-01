using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RoomItem : MonoBehaviour
{
    public TextMeshProUGUI roomName;
    public TextMeshProUGUI playerCount;
    public TextMeshProUGUI roomStatus;
    CreateAndJoinRooms manager;

    private void Start() 
    {
        manager = FindObjectOfType<CreateAndJoinRooms>();
    }
    
    public void SetRoomName(string _roomName)
    {
        roomName.text = _roomName;
    }
    public void SetPlayerCount(string _playerCount)
    {
        playerCount.text = _playerCount;
    }
    public void SetRoomStatus(string _roomStatus)
    {
        roomStatus.text = _roomStatus;
    }

    public void OnClickItem()
    {
        manager.JoinRoom(roomName.text);
    }

}
