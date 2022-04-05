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
    public Transform redTeamRespawnPoint;
    public Transform blueTeamRespawnPoint;

    GameObject _player;
    Transform _spawnPoint;
    int _playerTeam;

    void Start()
    {
        _playerTeam = (int)PhotonNetwork.LocalPlayer.CustomProperties["team"];
        if (_playerTeam == 0)
        {
            _spawnPoint = blueTeamSpawnPoints[(int)PhotonNetwork.LocalPlayer.CustomProperties["spawnPoint"]];
        }
        else if (_playerTeam == 1)
        {
            _spawnPoint = redTeamSpawnPoints[(int)PhotonNetwork.LocalPlayer.CustomProperties["spawnPoint"]];
        }
        else
        {
            _spawnPoint = noTeamSpawnPoint;
        }
        _player = PhotonNetwork.Instantiate(playerPrefab.name, _spawnPoint.position, Quaternion.identity);
    }

    public void NewRound()
    {
        int playerTeam = (int)PhotonNetwork.LocalPlayer.CustomProperties["team"];
        if (playerTeam == 0)
        {
            _spawnPoint = blueTeamSpawnPoints[(int)PhotonNetwork.LocalPlayer.CustomProperties["spawnPoint"]];
        }
        else if (playerTeam == 1)
        {
            _spawnPoint = redTeamSpawnPoints[(int)PhotonNetwork.LocalPlayer.CustomProperties["spawnPoint"]];
        }
        else
        {
            _spawnPoint = noTeamSpawnPoint;
        }
        _player.transform.position = _spawnPoint.position;
    }

    private void Update()
    {
        if (_player.transform.position.y < -20)
        {
            if (_playerTeam == 0)
            {
                _player.transform.position = blueTeamRespawnPoint.position;
            }
            else if (_playerTeam == 1)
            {
                _player.transform.position = redTeamRespawnPoint.position;
            }
        }
    }
}

