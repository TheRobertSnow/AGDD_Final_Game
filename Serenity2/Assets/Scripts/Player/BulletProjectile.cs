using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BulletProjectile : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    //public Rigidbody rb;
    private PhotonView _photonView;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start()
    {
        Invoke(nameof(destroyInSeconds), 5f);
    }

    // 0 blue
    // 1 red
    public int team;
    void OnCollisionEnter(Collision other) {

        // Destroy(gameObject);
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] data = info.photonView.InstantiationData;
        GetComponent<Rigidbody>().velocity = (Vector3) data[0];
        team = (int)data[1];
    }

    private void OnDestroy()
    {
        Debug.Log(GetComponent<Rigidbody>().position);
    }

    void destroyInSeconds()
    {
        if (_photonView.IsMine && PhotonNetwork.IsMasterClient)
        {
            // PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
