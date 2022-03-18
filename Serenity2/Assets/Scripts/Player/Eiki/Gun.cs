using UnityEngine;
using TMPro;
public class Gun : MonoBehaviour {
    public enum ShootState {
        Ready,
        Shooting,
        Reloading
    }
    public Camera Camera;
    // How far forward the muzzle is from the centre of the gun
    private float muzzleOffset;

    [Header("Magazine")]
    public GameObject round;
    public int ammunition;

    [Range(0.5f, 10)] public float reloadTime;

    private int remainingAmmunition;

    [Header("Shooting")]
    // How many shots the gun can make per second
    [Range(0.25f, 25)] public float fireRate;

    // The number of rounds fired each shot
    public int roundsPerShot;

    [Range(0.5f, 100)] public float roundSpeed;

    // The maximum angle that the bullet's direction can vary,
    // in both the horizontal and vertical axes
    [Range(0, 45)] public float maxRoundVariation;

    public Animator m_Animator;
    public AudioSource audioSource;

    [Header("Text properties")]
    public TextMeshProUGUI ammoText;

    private ShootState shootState = ShootState.Ready;

    // The next time that the gun is able to shoot at
    private float nextShootTime = 0;

    void Start() {
        //muzzleOffset = GetComponent<Renderer>().bounds.extents.z;
        remainingAmmunition = ammunition;
    }

    void Update() {
        ammoText.text = "Ammo:" + remainingAmmunition + "/" + ammunition;
        switch(shootState) {
            case ShootState.Shooting:
                // If the gun is ready to shoot again...
                if(Time.time > nextShootTime) {
                    shootState = ShootState.Ready;
                }
                break;
            case ShootState.Reloading:
                // If the gun has finished reloading...
                if(Time.time > nextShootTime) {
                    remainingAmmunition = ammunition;
                    shootState = ShootState.Ready;
                }
                break;
        }
    }

    /// Attempts to fire the gun
    public void Shoot() {
        // Checks that the gun is ready to shoot
        if(shootState == ShootState.Ready) {
            for(int i = 0; i < roundsPerShot; i++) {

                 // Create a ray from the camera going through the middle of your screen
                Ray ray = Camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
                RaycastHit hit;
                // Check whether your are pointing to something so as to adjust the direction
                Vector3 targetPoint;
                if (Physics.Raycast(ray, out hit))
                {
                    targetPoint = hit.point;
                }    
                else
                {
                    targetPoint = ray.GetPoint( 1000 );
                }
                GameObject spawnedRound = Instantiate(
                    round, 
                    transform.position + transform.forward * 1f,
                    transform.rotation);

                Rigidbody rb = spawnedRound.GetComponent<Rigidbody>();

                rb.velocity = ( targetPoint - transform.position ).normalized * roundSpeed;
                

            
                /* 
                // Instantiates the round at the muzzle position
                GameObject spawnedRound = Instantiate(
                    round,
                    transform.position + transform.forward * muzzleOffset,
                    transform.rotation
                );

                // Add a random variation to the round's direction
                spawnedRound.transform.Rotate(new Vector3(
                    Random.Range(-1f, 1f) * maxRoundVariation,
                    Random.Range(-1f, 1f) * maxRoundVariation,
                    0
                ));

                Rigidbody rb = spawnedRound.GetComponent<Rigidbody>();
                rb.velocity = spawnedRound.transform.forward * roundSpeed;
                */
            }
            remainingAmmunition--;
            if(remainingAmmunition > 0) {
                nextShootTime = Time.time + (1 / fireRate);
                shootState = ShootState.Shooting;
                m_Animator.SetTrigger("Shoot");
                audioSource.Play();
            } else {
                Reload();
            }
        }
    }

    /// Attempts to reload the gun
    public void Reload() {
        // Checks that the gun is ready to be reloaded
        if(shootState == ShootState.Ready) {
            nextShootTime = Time.time + reloadTime;
            shootState = ShootState.Reloading;
        }
    }
}