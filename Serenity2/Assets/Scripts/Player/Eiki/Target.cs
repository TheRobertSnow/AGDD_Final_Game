using Photon.Pun;
using UnityEngine;

public class Target : MonoBehaviour 
{
    public TargetManager targetManager;

    private HealthController healthController;
    private PhotonView _photonView;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start() 
    {
        targetManager = GameObject.Find("TargetManager").GetComponent<TargetManager>();
        healthController = GameObject.Find("HealthController").GetComponent<HealthController>();
    }
    void OnCollisionEnter(Collision other) {
        BulletProjectile bullet = other.gameObject.GetComponent<BulletProjectile>();
        if(bullet != null) {
            if (_photonView.IsMine && PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(_photonView.gameObject);
                targetManager.GetComponent<TargetManager>().wasHit = true;
            }
            if (bullet.team == 0) {
                healthController.SetHealthRedTeam(10);
            }
            else {
                healthController.SetHealthBlueTeam(10);
            }
        }
    }
}