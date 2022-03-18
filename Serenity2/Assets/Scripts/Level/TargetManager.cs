using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    [Header("Target Prefab")]
    public GameObject target;

    [Header("Time")]
    [Tooltip("Current time")]
    public float time;
    [Tooltip("Time at which to spawn prefab")]
    public float spawnTime;

    [Tooltip("Minimum time to wait for respawn")]
    public float minTime;
    [Tooltip("Maximum time to wait for respawn")]
    public float maxTime;

    [Header("Spawn Position")]
    public float minY;
    public float maxY;
    public float minZ;
    public float maxZ;

    [Header("Other")]
    [Tooltip("Player has hit last target")]
    public bool wasHit;

    // Start is called before the first frame update
    void Start()
    {
        SpawnObject();
        SetRandomTime();
    }

    private void FixedUpdate()
    {
        // Add to counter
        time += Time.fixedDeltaTime;

        // Check if time to spawn object
        if (time >= spawnTime)
        {
            if (wasHit)
            {
                SpawnObject();
                SetRandomTime();
                wasHit = false;
            }
        }
    }
    private void SetRandomTime()
    {
        spawnTime = Random.Range(minTime, maxTime);
    }

    private void SpawnObject()
    {
        time = 0;
        Vector3 pos = new Vector3(0, Random.Range(minY, maxY), Random.Range(minZ, maxZ));
        Instantiate(target, pos, target.transform.rotation);
    }
}
