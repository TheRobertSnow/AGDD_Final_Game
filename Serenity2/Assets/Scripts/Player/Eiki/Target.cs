using UnityEngine;

public class Target : MonoBehaviour 
{
    public TargetManager targetManager;

    private void Start() 
    {
        targetManager = GameObject.Find("TargetManager").GetComponent<TargetManager>();
    }
    void OnCollisionEnter(Collision other) {
        BulletProjectile bullet = other.gameObject.GetComponent<BulletProjectile>();
        if(bullet != null) {
            targetManager.GetComponent<TargetManager>().wasHit = true;
            Destroy(gameObject);
        }
    }
}