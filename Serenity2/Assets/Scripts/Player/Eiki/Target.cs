using System;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class Target : MonoBehaviour
{
    public TargetManager targetManager;

    private HealthController healthController;
    private PhotonView _photonView;

    // if it randomizes to a moving target, then this is the speed
    public float targetSpeed = 2.5f;
    
    // if it is not a moving target then the size will be scaled somewhere between the scale mix/max
    public float sizeScaleMax = 1.0f;
    public float sizeScaleMin = 0.5f;


    private bool isMovingTarget;
    private float minY;
    private float maxY;
    private float minZ;
    private float maxZ;
    private Vector3 destination;
    private bool goingtoEnd = true;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    private void Start() 
    {
        targetManager = GameObject.Find("TargetManager").GetComponent<TargetManager>();
        healthController = GameObject.Find("HealthController").GetComponent<HealthController>();
        minY = targetManager.minY;
        minZ = targetManager.minZ;
        maxY = targetManager.maxY;
        maxZ = targetManager.maxZ;

        destination = GetNewDestination();
        isMovingTarget = Random.Range(0, 4) == 1;

        if (!isMovingTarget)
        {
            float scale = Random.Range(sizeScaleMin, sizeScaleMax);
            transform.localScale = new Vector3(scale, transform.localScale.y, scale);
        }
    }

    private Vector3 GetNewDestination()
    {
        return new Vector3(0, Random.Range(minY, maxY), Random.Range(minZ, maxZ));
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
            healthController.damageBlueTeam();
        }
        else
        {
            healthController.damageRedTeam();
        }
    }

    private void Update()
    {
        if (!isMovingTarget) return;

        float step = targetSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, destination, step);

        // if we reach the destination we make a new one.
        if (transform.position == destination)
        {
            destination = GetNewDestination();
        }
    }
}