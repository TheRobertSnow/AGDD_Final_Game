using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SmokeCanSpawnItem : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    PhotonView _PV;
    public bool destroySelf = false;
    public void Awake()
    {
        _PV = GetComponent<PhotonView>();
    }
    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        
    }

    private void Start()
    {
        if (this.gameObject.GetComponent<Animation>() != null) this.gameObject.GetComponent<Animation>().Play();
    }
    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        transform.Rotate(0, dt * 90, 0, Space.World);
    }
    //private void OnTriggerEnter(Collider collider)
    //{
    //    if (collider.tag == "Player")
    //    {
    //        collider.GetComponent<PlayerController>().IncrementSmokeCount();
    //        _PV.RPC("RemoveObj", RpcTarget.All);
    //        //RemoveObj();
    //    }
    //}

    public void KillMySelf()
    {
        _PV.RPC(nameof(RemoveObj), RpcTarget.All);
    }

    [PunRPC]
    private void RemoveObj()
    {

        if (_PV.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
