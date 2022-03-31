using Photon.Pun;
using UnityEngine;

public class Target : MonoBehaviour 
{
    public TargetManager targetManager;

    private PhotonView _photonView;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start() 
    {
        targetManager = GameObject.Find("TargetManager").GetComponent<TargetManager>();
    }
    void OnCollisionEnter(Collision other) {
        BulletProjectile bullet = other.gameObject.GetComponent<BulletProjectile>();
        if(bullet != null) {
            if (_photonView.IsMine && PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(_photonView.gameObject);
                targetManager.GetComponent<TargetManager>().wasHit = true;
            }
        }
    }
}