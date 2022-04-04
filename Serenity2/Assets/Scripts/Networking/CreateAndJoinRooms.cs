using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public TextMeshProUGUI roomName;

    public RoomItem roomItemPrefab;
    List<RoomItem> roomItemsList = new List<RoomItem>();
    public Transform contentObject;

    public float timeBetweenUpdates = 1.5f;
    float nextUpdateTime;

    public List<PlayerItem> playerItemsList = new List<PlayerItem>();
    public PlayerItem playerItemPrefab;
    public PlayerItem otherPlayerItemPrefab;
    public Transform blueTeamList;
    public Transform redTeamList;

    public GameObject playButton;

    

    private void Start() 
    {
        PhotonNetwork.JoinLobby();
    }

    public void OnClickCreate()
    {
        if (createInput.text.Length > 0)
        {
            ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable
            {
                { "blueTeamCount", 0 },
                { "redTeamCount", 0 },
            };
            PhotonNetwork.CreateRoom(createInput.text, new RoomOptions(){ MaxPlayers = 10, BroadcastPropsChangeToAll = true, CustomRoomProperties = roomProperties });
        }
    }

    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = "Room name: " + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenUpdates;
        }
    }

    void UpdateRoomList(List<RoomInfo> list)
    {
        foreach (RoomItem item in roomItemsList)
        {   
            Destroy(item.gameObject);
        }
        roomItemsList.Clear();

        foreach(RoomInfo room in list)
        {
            if (room.RemovedFromList)
            {
                return;
            }
            RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
            newRoom.SetRoomName(room.Name);
            newRoom.SetPlayerCount(room.PlayerCount.ToString() + "/" + room.MaxPlayers.ToString());
            roomItemsList.Add(newRoom);
        }
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);   
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        roomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        UpdatePlayerList();
    }

    void UpdatePlayerList()
    {
        foreach (PlayerItem item in playerItemsList)
        {
            Destroy(item.gameObject);
        }
        playerItemsList.Clear();

        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if(player.Value.NickName == PhotonNetwork.NickName)
            {
                if (player.Value.CustomProperties.ContainsKey("team"))
                {
                    PlayerItem newPlayerItem = Instantiate(playerItemPrefab, (int)player.Value.CustomProperties["team"] == 0 ? blueTeamList : redTeamList);
                    newPlayerItem.SetPlayerInfo(player.Value);
                    playerItemsList.Add(newPlayerItem);
                }
                else
                {
                    PlayerItem newPlayerItem = Instantiate(playerItemPrefab, blueTeamList);
                    newPlayerItem.SetPlayerInfo(player.Value);
                    playerItemsList.Add(newPlayerItem);
                }
            }
            else
            {
                if (player.Value.CustomProperties.ContainsKey("team"))
                {
                    PlayerItem newPlayerItem = Instantiate(otherPlayerItemPrefab, (int)player.Value.CustomProperties["team"] == 0 ? blueTeamList : redTeamList);
                    newPlayerItem.SetPlayerInfo(player.Value);
                    playerItemsList.Add(newPlayerItem);
                }
                else
                {
                    PlayerItem newPlayerItem = Instantiate(otherPlayerItemPrefab, blueTeamList);
                    newPlayerItem.SetPlayerInfo(player.Value);
                    playerItemsList.Add(newPlayerItem);
                }
            }
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
    }

    private void Update()
    {
        //if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 1)
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 1)
        {
            playButton.SetActive(true);
        }
        else
        {
            playButton.SetActive(false);
        }
    }

    public void OnClickPlayButton()
    {
        PhotonNetwork.LoadLevel("PlayfieldTestEikus");
    }
}
