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

    public List<Texture2D> playerModels;

    public ExitGames.Client.Photon.Hashtable roomProperties = new ExitGames.Client.Photon.Hashtable
    {
        { "blueTeamCount", 0 },
        { "redTeamCount", 0 },
        { "status", "waiting" },
        { "allPlayersReady", false}
    };

    private PhotonView _photonView;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start() 
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (PhotonNetwork.CurrentLobby == null) {
            PhotonNetwork.JoinLobby();
        }
        if (PlayerPrefs.GetInt("gameWinner") != -1)
        {
            lobbyPanel.SetActive(false);
            roomPanel.SetActive(true);
            roomName.text = "Room name: " + PhotonNetwork.CurrentRoom.Name;
            JoinSpectator();
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

    public void UpdateRoomList(List<RoomInfo> list)
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
            newRoom.SetRoomStatus("waiting");
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
                ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
                playerProperties.Add("team", 2);
                playerProperties.Add("ready", false);
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
                ExitGames.Client.Photon.Hashtable playerProperties = player.Value.CustomProperties;
                playerProperties["team"] = 1;
                playerProperties["spawnPoint"] = PhotonNetwork.CurrentRoom.CustomProperties["redTeamCount"];
                player.Value.SetCustomProperties(playerProperties);
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
                ExitGames.Client.Photon.Hashtable playerProperties = player.Value.CustomProperties;
                playerProperties["team"] = 0;
                playerProperties["spawnPoint"] = PhotonNetwork.CurrentRoom.CustomProperties["blueTeamCount"];
                player.Value.SetCustomProperties(playerProperties);
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
        // Hard reset everything on update
        spectatorList.Clear();
        blueTeamList.Clear();
        redTeamList.Clear();
        ExitGames.Client.Photon.Hashtable currentRoomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
        currentRoomProperties["redTeamCount"] = 0;
        currentRoomProperties["blueTeamCount"] = 0;

        // Iterate through every player in the room
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            // Spectator
            if ((int)player.Value.CustomProperties["team"] == 2)
            {
                SpectatorItem spectator = spectatorPrefab;
                spectator.SetPlayerName(player.Value);
                spectatorList.Add(spectator);
                Instantiate(spectator, spectatorPanel);
            }
            // Red team
            else if ((int)player.Value.CustomProperties["team"] == 1)
            {
                currentRoomProperties["redTeamCount"] = (int)currentRoomProperties["redTeamCount"] + 1;

                PlayerItem redTeamPlayer = playerItemPrefab;
                redTeamPlayer.SetPlayerInfo(player.Value);
                redTeamList.Add(redTeamPlayer);
                Instantiate(redTeamPlayer, redTeamPanel);

            }
            // Blue team
            else if ((int)player.Value.CustomProperties["team"] == 0)
            {
                currentRoomProperties["blueTeamCount"] = (int)currentRoomProperties["blueTeamCount"] + 1;
                
                PlayerItem blueTeamPlayer = playerItemPrefab;
                blueTeamPlayer.SetPlayerInfo(player.Value);
                blueTeamList.Add(blueTeamPlayer);
                Instantiate(blueTeamPlayer, blueTeamPanel);
            }
        }
        PhotonNetwork.CurrentRoom.SetCustomProperties(currentRoomProperties);
    }
    
    public void OnClickPressReady()
    {
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
            {
                ExitGames.Client.Photon.Hashtable playerProperties = player.Value.CustomProperties;
                playerProperties["ready"] = !(bool)playerProperties["ready"];
                readyButton.GetComponentInChildren<TextMeshProUGUI>().text = !(bool)playerProperties["ready"] ? "Ready" : "Unready";
                readyButton.GetComponent<Image>().color = !(bool)playerProperties["ready"] ? Color.green : Color.red;
                player.Value.SetCustomProperties(playerProperties);
            }
        }
    }

    bool CheckAllPlayersReady()
    {
        foreach (KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players)
        {
            if (player.Value.CustomProperties.ContainsKey("team") && (int)player.Value.CustomProperties["team"] != 2 && !(bool)player.Value.CustomProperties["ready"]) return false;
        }
        return true;
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
        if (PhotonNetwork.IsMasterClient && 
            PhotonNetwork.CurrentRoom.PlayerCount >= 1 && 
            CheckAllPlayersReady() && 
            ((int)PhotonNetwork.CurrentRoom.CustomProperties["blueTeamCount"] >= 1 ||
            (int)PhotonNetwork.CurrentRoom.CustomProperties["redTeamCount"] >= 1))
        {
            playButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            playButton.GetComponent<Button>().interactable = false;
        }

        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("team") && (int)PhotonNetwork.LocalPlayer.CustomProperties["team"] == 2)
        {
            readyButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            readyButton.GetComponent<Button>().interactable = true;
        }

        if ((PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("team") && (int)PhotonNetwork.LocalPlayer.CustomProperties["team"] == 1) ||
            (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("team") && (bool)PhotonNetwork.LocalPlayer.CustomProperties["ready"]))
        {
            joinRedButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            joinRedButton.GetComponent<Button>().interactable = true;
        }

        if  ((PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("team") && (int)PhotonNetwork.LocalPlayer.CustomProperties["team"] == 0) ||
            (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("team") && (bool)PhotonNetwork.LocalPlayer.CustomProperties["ready"]))
        {
            joinBlueButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            joinBlueButton.GetComponent<Button>().interactable = true;
        }
    }

    public void OnClickPlayButton()
    {
        ExitGames.Client.Photon.Hashtable currentRoomProperties = PhotonNetwork.CurrentRoom.CustomProperties;
        currentRoomProperties["status"] = "started";
        PhotonNetwork.CurrentRoom.SetCustomProperties(currentRoomProperties);
        PhotonNetwork.LoadLevel("PlayfieldTestDinkus");
    }
}
