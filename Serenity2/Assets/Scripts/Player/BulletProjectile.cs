using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    void OnCollisionEnter(Collision other) {
        Destroy(gameObject);
    }
}
