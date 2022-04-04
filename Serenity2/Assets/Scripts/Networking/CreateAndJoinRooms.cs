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

    public List<PlayerItem> blueTeamList = new List<PlayerItem>();
    public List<PlayerItem> redTeamList = new List<PlayerItem>();
    public List<SpectatorItem> spectatorList = new List<SpectatorItem>();

    public SpectatorItem spectatorPrefab;
    public PlayerItem playerItemPrefab;
    public PlayerItem otherPlayerItemPrefab;

    public Transform blueTeamPanel;
    public Transform redTeamPanel;
    public Transform spectatorPanel;

    public GameObject playButton;
    public GameObject readyButton;
    public GameObject joinBlueButton;
    public GameObject joinRedButton;

    public ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable
            {
                { "blueTeamCount", 0 },
                { "redTeamCount", 0 },
            };


    private void Start() 
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (PhotonNetwork.CurrentLobby == null) {
            PhotonNetwork.JoinLobby();
        }
        if (PlayerPrefs.GetInt("gameWinner") != -1)
        {
            PlayerPrefs.SetInt("gameWinner", -1);
            lobbyPanel.SetActive(false);
            roomPanel.SetActive(true);
            roomName.text = "Room name: " + PhotonNetwork.CurrentRoom.Name;
            UpdatePlayerList();
        }
    }

    public void OnClickCreate()
    {
        if (createInput.text.Length > 0)
        {
            PhotonNetwork.CreateRoom(createInput.text, new RoomOptions(){ MaxPlayers = 10, CustomRoomProperties = roomProperties });
        }
    }

    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = "Room name: " + PhotonNetwork.CurrentRoom.Name;
        /* Set player automatically to spectator list */
        JoinSpectator();
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

    void JoinSpectator()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if(player.Value.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable
                {
                    ["team"] = 2
                };
                player.Value.SetCustomProperties(playerProperties);
                return;
            }
        }
    }

    public void JoinRedTeam()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable
                {
                    ["team"] = 1
                };
                player.Value.SetCustomProperties(playerProperties);
                roomProperties["redTeamCount"] = (int)roomProperties["redTeamCount"] + 1;
                PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
                return;
            }
        }
    }
    public void JoinBlueTeam()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable
                {
                    ["team"] = 0
                };
                player.Value.SetCustomProperties(playerProperties);
                roomProperties["blueTeamCount"] = (int)roomProperties["blueTeamCount"] + 1;
                PhotonNetwork.CurrentRoom.SetCustomProperties(roomProperties);
                return;
            }
        }
    }
    
    void UpdatePlayerList()
    {
        if (PhotonNetwork.CurrentRoom == null)
        {
            return;
        }

        foreach (Transform item in spectatorPanel)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in blueTeamPanel)
        {
            Destroy(item.gameObject);
        }
        foreach (Transform item in redTeamPanel)
        {
            Destroy(item.gameObject);
        }
        spectatorList.Clear();
        blueTeamList.Clear();
        redTeamList.Clear();

        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log(player.Value.NickName + player.Value.CustomProperties["team"]);
            Debug.Log(roomProperties["redTeamCount"]);
            Debug.Log(roomProperties["blueTeamCount"]);

            if ((int)player.Value.CustomProperties["team"] == 2)
            {
                SpectatorItem spectator = spectatorPrefab;
                spectator.SetPlayerName(player.Value);
                spectatorList.Add(spectator);
                Instantiate(spectator, spectatorPanel);
            }
            else if ((int)player.Value.CustomProperties["team"] == 1)
            {
                PlayerItem redTeamPlayer = playerItemPrefab;
                redTeamPlayer.SetPlayerInfo(player.Value);
                redTeamList.Add(redTeamPlayer);
                Instantiate(redTeamPlayer, redTeamPanel);
            }
            else if ((int)player.Value.CustomProperties["team"] == 0)
            {
                PlayerItem blueTeamPlayer = playerItemPrefab;
                blueTeamPlayer.SetPlayerInfo(player.Value);
                blueTeamList.Add(blueTeamPlayer);
                Instantiate(blueTeamPlayer, blueTeamPanel);
            }
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable playerProperties)
    {
        UpdatePlayerList();
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
        PhotonNetwork.LoadLevel("PlayfieldTestDinkus");
    }
}
