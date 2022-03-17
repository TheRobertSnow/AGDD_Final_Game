using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public float minX;
    public float maxX;
    public float minZ;
    public float maxZ;
    
    void Start()
    {
        Vector3 randomPos = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));
        PhotonNetwork.Instantiate(playerPrefab.name, randomPos, Quaternion.identity);
    }


}

