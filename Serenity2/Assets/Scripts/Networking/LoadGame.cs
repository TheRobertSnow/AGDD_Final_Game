using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LoadGame : MonoBehaviourPunCallbacks
{
    private PhotonView view;
    private int playerCount;
    public int roomSize = 2;
    public bool readyToStart;
    public bool startingGame;
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        WaitingForMorePlayers();
    }

    public void PlayerCountUpdate()
    {
        playerCount = PhotonNetwork.PlayerList.Length;
        roomSize = PhotonNetwork.CurrentRoom.MaxPlayers;
        if (roomSize == null)
        {
            roomSize = 2;
        }

        if (playerCount == roomSize)
        {
            readyToStart = true;
        }
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        PlayerCountUpdate();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PlayerCountUpdate();
    }

    public void WaitingForMorePlayers()
    {
        if (readyToStart)
        {
            if (startingGame)
                return;
            StartGame();
        }
    }

    public void StartGame()
    {
        startingGame = true;
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel("Game-Eiki");
    }
}
