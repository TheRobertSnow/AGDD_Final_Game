using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BulletProjectile : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    //public Rigidbody rb;

    public int team;
    void OnCollisionEnter(Collision other) {

        Destroy(gameObject);
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] data = info.photonView.InstantiationData;
        GetComponent<Rigidbody>().velocity = (Vector3) data[0];
        team = (int)data[1];
    }
}
