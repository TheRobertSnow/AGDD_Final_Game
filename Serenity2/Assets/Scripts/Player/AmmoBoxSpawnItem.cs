using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBoxSpawnItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (this.gameObject.GetComponent<Animation>() != null) this.gameObject.GetComponent<Animation>().Play();
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        transform.Rotate(0, dt * 90, 0, Space.World);
    }
}
