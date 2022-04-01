using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform[] blueTeamSpawnPoints;
    public Transform[] redTeamSpawnPoints;
    public Transform noTeamSpawnPoint;
    int randomNumber;
    Transform spawnPoint;
    void Start()
    {
        int playerTeam = (int)PhotonNetwork.LocalPlayer.CustomProperties["team"];
        
        if (playerTeam == 0)
        {
            randomNumber = Random.Range(0, blueTeamSpawnPoints.Length);
            spawnPoint = blueTeamSpawnPoints[randomNumber];
        }
        else if (playerTeam == 1)
        {
            randomNumber = Random.Range(0, redTeamSpawnPoints.Length);
            spawnPoint = redTeamSpawnPoints[randomNumber];
        }
        else {
            spawnPoint = noTeamSpawnPoint;
        }
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity);
    }
}

