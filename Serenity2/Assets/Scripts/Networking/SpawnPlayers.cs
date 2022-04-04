using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform[] blueTeamSpawnPoints;
    public Transform[] redTeamSpawnPoints;
    public Transform noTeamSpawnPoint;
    Transform spawnPoint;
    int redTeamCount;
    int blueTeamCount;
    void Start()
    {
        int playerTeam = (int)PhotonNetwork.LocalPlayer.CustomProperties["team"];
        if (playerTeam == 0)
        {
            spawnPoint = blueTeamSpawnPoints[0];
        }
        else if (playerTeam == 1)
        {
            spawnPoint = redTeamSpawnPoints[0];
        }
        else
        {
            spawnPoint = noTeamSpawnPoint;
        }
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity);
    }
}

