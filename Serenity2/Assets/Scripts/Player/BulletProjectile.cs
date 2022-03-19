using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class BulletProjectile : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    public Rigidbody rb;

    void OnCollisionEnter(Collision other) {
        Destroy(gameObject);
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] data = info.photonView.InstantiationData;
        foreach (var velocity in data)
        {
            rb.velocity = (Vector3) velocity;
        }
    }
}
