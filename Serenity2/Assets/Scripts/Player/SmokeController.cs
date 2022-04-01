using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SmokeController : MonoBehaviour
{
    public GameObject smokeEffect;
    public Vector3 velocity;
    private Rigidbody _rigidBody;
    private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _camera = GameObject.Find("CameraHolder").GetComponentInChildren<Camera>();
        Quaternion rot = _camera.transform.rotation;
        //rot *= Quaternion.Euler(0, 90, 0);
        //Vector3 targetForward = rot * _camera.transform.forward;
        //Vector3 targetUp = rot * _camera.transform.up;
        //_rigidBody.velocity = (targetForward + targetUp)*5;
        _rigidBody.velocity = _camera.transform.forward * 18;
        Invoke("RunSmokeAnimation", 5f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RunSmokeAnimation()
    {
        GameObject smokeEff = PhotonNetwork.Instantiate("SmokeEffect", transform.position, Quaternion.identity);
    }
}
