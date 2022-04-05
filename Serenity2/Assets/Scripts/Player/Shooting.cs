using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Shooting : MonoBehaviourPun
{
    public Rigidbody bulletPrefab;
    public float bulletSpeed;
    public GameObject gun;

    private void Shoot()
    {
        Rigidbody bulletClone = Instantiate(bulletPrefab, gun.transform.position, this.transform.rotation);

        bulletClone.velocity = transform.TransformDirection(new Vector3(0, 0, bulletSpeed));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) Shoot();
    }
}