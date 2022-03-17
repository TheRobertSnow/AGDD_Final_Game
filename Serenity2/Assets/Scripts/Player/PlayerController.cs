using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviour
{
    public float speed;

    PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        view = gameObject.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (view.IsMine)
        {

        }
    }
}
