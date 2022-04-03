using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

enum PickupType
{
    SMOKE,
    SOMETHING
}

public class ItemSpawnController : MonoBehaviourPun
{
    [Header("Prefabs")]
    [Tooltip("Point where items spawn")]
    public GameObject spawn;
    [Tooltip("Smoke mesh")]
    public GameObject smokeMesh;
    [Space()]
    [Tooltip("Currently Displayed Object")]
    public GameObject currentObject;

    private bool _canPickUp;
    private PickupType _objectType;
    private PhotonView _PV;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
        _PV.ViewID = 1002;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentObject = PhotonNetwork.Instantiate("smokeCanSpawnItem", spawn.transform.position, Quaternion.Euler(-80, 0, 0));
        _canPickUp = true;
        _objectType = PickupType.SMOKE;
    }

    public void SetSpawnState()
    {
        currentObject = null;
        _canPickUp = false;
        _objectType = PickupType.SOMETHING;
    }
}
