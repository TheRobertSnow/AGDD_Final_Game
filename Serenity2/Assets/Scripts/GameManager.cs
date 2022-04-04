using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviourPun
{
    #region Singleton
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
    #endregion

    public Vector3 MAXBOUNDS1 = new Vector3(62.5800018f, 1.5f, 40.0699997f);
    public Vector3 MAXBOUNDS2 = new Vector3(62.5800018f, 1.5f, -40.0699997f);
    public Vector3 MINBOUNDS1 = new Vector3(-62.5800018f, 1.5f, 40.0699997f);
    public Vector3 MINBOUNDS2 = new Vector3(-62.5800018f, 1.5f, -40.0699997f);

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        // Create Item Spawns
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("ItemSpawn", new Vector3(-48.2709999f, 4f, 4.8499999f), Quaternion.identity);
            PhotonNetwork.Instantiate("ItemSpawn", new Vector3(-48.2709999f, 4f, -31.4899998f), Quaternion.identity);
            PhotonNetwork.Instantiate("ItemSpawn", new Vector3(-39.3899994f, 4f, 29.4899998f), Quaternion.identity);
            PhotonNetwork.Instantiate("ItemSpawn", new Vector3(48.2709999f, 4f, -4.8499999f), Quaternion.identity);
            PhotonNetwork.Instantiate("ItemSpawn", new Vector3(48.2709999f, 4f, 31.4899998f), Quaternion.identity);
            PhotonNetwork.Instantiate("ItemSpawn", new Vector3(39.3899994f, 4f, -29.4899998f), Quaternion.identity);
        }
    }


}
