using UnityEngine;

public class Round : MonoBehaviour {
    public float damage;

    void OnTriggerEnter(Collider other) {
        Target target = other.gameObject.GetComponent<Target>();
        // Only attempts to inflict damage if the other game object has
        // the 'Target' component
        Debug.Log(target);
        if(target != null) {
            target.Hit(damage);
            Destroy(gameObject); // Deletes the round
        }
    }
}