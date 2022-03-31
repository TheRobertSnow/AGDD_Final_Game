using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeController : MonoBehaviour
{
    public GameObject smokeEffect;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("RunSmokeAnimation", 5f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RunSmokeAnimation()
    {
        GameObject smokeEff = Instantiate(smokeEffect, transform.position, Quaternion.identity);
    }
}
