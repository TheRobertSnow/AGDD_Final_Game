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


    private bool isMovingTarget = false;
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


        if (!PhotonNetwork.IsMasterClient) return;
        // only the master client will set sizes/directions
        isMovingTarget = Random.Range(0, 4) == 1;
        if (isMovingTarget)
        {
            _photonView.RPC(nameof(GetNewDestination), RpcTarget.All, Random.Range(minY, maxY),
                Random.Range(minZ, maxZ));
        }
        else
        {
            float scale = Random.Range(sizeScaleMin, sizeScaleMax);
            _photonView.RPC(nameof(SetScale), RpcTarget.All, scale, scale);
            // transform.localScale = new Vector3(scale, transform.localScale.y, scale);
        }
    }

    [PunRPC]
    private void SetScale(float x, float z)
    {
        transform.localScale = new Vector3(x, transform.localScale.y, z);
    }

    [PunRPC]
    private void GetNewDestination(float y, float z)
    {
        isMovingTarget = true;
        destination = new Vector3(0, y, z);
    }

    void OnCollisionEnter(Collision other)
    {
        BulletProjectile bullet = other.gameObject.GetComponent<BulletProjectile>();
        if (bullet != null)
        {
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
        if (transform.position == destination && PhotonNetwork.IsMasterClient)
        {
            _photonView.RPC(nameof(GetNewDestination), RpcTarget.All, Random.Range(minY, maxY),
                Random.Range(minZ, maxZ));
            // GetNewDestination(Random.Range(minY, maxY),Random.Range(minZ, maxZ));
        }
    }
}