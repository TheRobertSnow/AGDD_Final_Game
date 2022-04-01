using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SmokeEffect : MonoBehaviour
{
    // Start is called before the first frame update
    private PhotonView _view;
    private ParticleSystem _particleSystem;


    private void Awake()
    {
        _view = GetComponent<PhotonView>();
        _particleSystem = GetComponent<ParticleSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Invoke("EndParticleEmitter", 15);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndParticleEmitter()
    {
        if (_particleSystem != null)
        {
            _particleSystem.Stop();
        }
    }
}
