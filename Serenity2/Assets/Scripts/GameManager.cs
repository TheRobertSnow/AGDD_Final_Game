using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPun
{
    private static GameManager _instance;
    
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("Game Manager is null");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        // Create Item Spawns
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("ItemSpawn", new Vector3(-11.8438625f, 4.03000021f, 3.44995356f), Quaternion.identity);
        }
    }


}
