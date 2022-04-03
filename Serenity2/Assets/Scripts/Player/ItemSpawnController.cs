using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Data;
using sys = System;

enum PickupType
{
    SMOKE,
    AMMO,
}

public class ItemSpawnController : MonoBehaviourPun
{
    [Header("Prefabs")]
    [Tooltip("Point where items spawn")]
    public GameObject spawn;
    [Space()]
    [Tooltip("Currently Displayed Object")]
    public GameObject currentObject;

    private bool _canPickUp;
    private PickupType _objectType;
    private PhotonView _PV;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
        // Manually assign view id so photon stops whining
        _PV.ViewID = 1002;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
        SpawnSmoke();

        }
        //currentObject = PhotonNetwork.Instantiate("smokeCanSpawnItem", spawn.transform.position, Quaternion.Euler(-80, 0, 0));
        //Instantiate(smokeMesh, spawn.transform.position, Quaternion.identity);
        _canPickUp = true;
        _objectType = PickupType.SMOKE;
    }

    public void SetSpawnState()
    {
        currentObject = null;
        _canPickUp = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            Debug.Log("Something collided");
            if (_PV.IsMine)
            {
                //if (!PhotonNetwork.IsMasterClient)
                //{
                //    _PV.RequestOwnership();
                //}
                int rand = Random.Range(0, 2);
                _PV.RPC(nameof(DestroyCurrentObject), RpcTarget.MasterClient, rand.ToString());
            }
        }
    }

    [PunRPC]
    private void DestroyCurrentObject(string newObj)
    {
        PhotonNetwork.Destroy(currentObject);
        int stuff = sys.Int32.Parse(newObj);
        SpawnNewItem(stuff);
    }

    private void SpawnNewItem(int newObj)
    {
        PickupType item = (PickupType) newObj;
        Debug.Log(item);
        switch (item)
        {
            case PickupType.SMOKE:
                Invoke(nameof(SpawnSmoke), 5);
                break;
            case PickupType.AMMO:
                Invoke(nameof(SpawnAmmo), 5);
                break;
            default:
                break;
        }
    }

    private void SpawnSmoke()
    {
        currentObject = PhotonNetwork.Instantiate("smokeCanSpawnItem", spawn.transform.position, Quaternion.Euler(-80, 0, 0));
    }

    private void SpawnAmmo()
    {
        currentObject = PhotonNetwork.Instantiate("AmmoBox", spawn.transform.position, Quaternion.Euler(-80, 0, 0));
    }
}
