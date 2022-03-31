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
            // if (bullet.team == 0) {
            //     healthController.SetHealthRedTeam(10);
            // }
            // else if (bullet.team == 1) {
            //     healthController.SetHealthBlueTeam(10);
            // }

            if (_photonView.IsMine && PhotonNetwork.IsMasterClient)
            {
                _photonView.RPC(nameof(DoDamage), RpcTarget.All, bullet.team == 0 ? "blue" : "red");
                PhotonNetwork.Destroy(_photonView.gameObject);
                targetManager.GetComponent<TargetManager>().wasHit = true;
                PhotonNetwork.Destroy(other.gameObject);
                
            }
            
        }
    }

    [PunRPC]
    void DoDamage(string team)
    {
        if (team == "red")
        {
            healthController.SetHealthBlueTeam(10);
        }
        else
        {
            healthController.SetHealthRedTeam(10);
        }
    }
}