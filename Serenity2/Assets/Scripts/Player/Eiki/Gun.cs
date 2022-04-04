using System;
using System.IO;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using TMPro;
using Object = UnityEngine.Object;

public class Gun : MonoBehaviour
{
    public enum ShootState
    {
        Ready,
        Shooting,
        Reloading
    }

    public Camera cam;

    // How far forward the muzzle is from the centre of the gun
    private float muzzleOffset;

    [Header("Magazine")] public GameObject round;
    public int ammunition;

    [Range(0.5f, 10)] public float reloadTime;

    private int remainingAmmunition;

    [Header("Shooting")]
    // How many shots the gun can make per second
    [Range(0.25f, 25)]
    public float fireRate;

    // The number of rounds fired each shot
    public int roundsPerShot;

    [Range(0.5f, 100)] public float roundSpeed;

    // The maximum angle that the bullet's direction can vary,
    // in both the horizontal and vertical axes
    [Range(0, 45)] public float maxRoundVariation;

    public Animator m_Animator;
    public AudioSource audioSource;

    [Header("Text properties")] public TextMeshProUGUI ammoText;

    private ShootState shootState = ShootState.Ready;

    // The next time that the gun is able to shoot at
    private float nextShootTime = 0;

    private PhotonView _PV;
    private int _team;

    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
    }

    void Start()
    {
        //muzzleOffset = GetComponent<Renderer>().bounds.extents.z;
        remainingAmmunition = ammunition;
        _team = (int)PhotonNetwork.LocalPlayer.CustomProperties["team"];
    }

    void Update()
    {
        if (_PV.IsMine)
        {
            ammoText.text = "Ammo:" + remainingAmmunition + "/" + ammunition;
        }
        switch (shootState)
        {
            case ShootState.Shooting:
                // If the gun is ready to shoot again...
                if (Time.time > nextShootTime)
                {
                    shootState = ShootState.Ready;
                }

                break;
            case ShootState.Reloading:
                // If the gun has finished reloading...
                if (Time.time > nextShootTime)
                {
                    int ammoBeforeReload = ammunition;
                    if (ammoBeforeReload == 0) ammoBeforeReload = 1;
                    ammunition = Math.Max(1, ammunition - 12);
                    remainingAmmunition = Math.Min(ammoBeforeReload, 12);
                    shootState = ShootState.Ready;
                }

                break;
        }
        if(Input.GetKey(KeyCode.F) && _PV.IsMine)
        {
            _PV.RPC("PlayFAnimation", RpcTarget.All);
        }
        else if (_PV.IsMine)
        {
            _PV.RPC("StopFAnimation", RpcTarget.All);
        }
    }

    [PunRPC]
    void PlayFAnimation()
    {
        m_Animator.SetBool("F_Pressed", true);
    }

    [PunRPC]
    void StopFAnimation()
    {
        m_Animator.SetBool("F_Pressed", false);
    }

    /// Attempts to fire the gun
    public void Shoot()
    {

        // Checks that the gun is ready to shoot
        if (shootState == ShootState.Ready && _team != 2)
        {
            for (int i = 0; i < roundsPerShot; i++)
            {
                // Create a ray from the camera going through the middle of your screen
                Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                RaycastHit hit;
                // Check whether your are pointing to something so as to adjust the direction
                Vector3 targetPoint;
                if (Physics.Raycast(ray, out hit))
                {
                    targetPoint = hit.point;
                }
                else
                {
                    targetPoint = ray.GetPoint(1000);
                }
                
                Vector3 velocity = (targetPoint - transform.position).normalized * roundSpeed;
                object[] data = new object[2];
                data[0] = velocity;
                data[1] = (int)PhotonNetwork.LocalPlayer.CustomProperties["team"];
                GameObject clone = PhotonNetwork.Instantiate(
                    "BulletProjectile",
                    transform.position + transform.forward * 1f,
                    transform.rotation, data: data);

                Rigidbody rb = clone.GetComponent<Rigidbody>();

                rb.velocity = (targetPoint - transform.position).normalized * roundSpeed;
            }
            m_Animator.SetTrigger("Shoot");
            audioSource.Play();
            remainingAmmunition--;
            if (remainingAmmunition > 0)
            {
                nextShootTime = Time.time + (1 / fireRate);
                shootState = ShootState.Shooting;
            }
            else
            {
                Reload();
            }
        }
    }

    /// Attempts to reload the gun
    public void Reload()
    {
        // Checks that the gun is ready to be reloaded
        if (shootState == ShootState.Ready)
        {
            nextShootTime = Time.time + reloadTime;
            shootState = ShootState.Reloading;
        }
    }

    public void ReloadInstantly()
    {
        Debug.Log("realod?!");
        // Checks that the gun is ready to be reloaded
        remainingAmmunition = ammunition;
        shootState = ShootState.Ready;
        nextShootTime = Time.time;
    }

    public void IncrementAmmo()
    {
        ammunition += 36;
    }
}