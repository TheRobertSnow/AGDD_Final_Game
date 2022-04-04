using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SmokeController : MonoBehaviour, IPunInstantiateMagicCallback
{
    public GameObject smokeEffect;
    public Vector3 velocity;
    private Rigidbody _rigidBody;
    private Camera _camera;
    private GameObject _smokeEff;

    // Start is called before the first frame update
    private PhotonView _view;

    private void Awake()
    {
        _view = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    //void Update()
    //{
    //    // Moves the smoke with canister but the result is scuffed
    //    //_smokeEff.transform.position = transform.position;
    //}

    public void RunSmokeAnimation()
    {
        if (_view.IsMine)
        {
            _smokeEff = PhotonNetwork.Instantiate("SmokeEffect", transform.position, Quaternion.identity);
            Invoke("RemoveSmokeEffect", 25);
        }
    }

    public void RemoveSmokeEffect()
    {
        PhotonNetwork.Destroy(_smokeEff);
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        _rigidBody = GetComponent<Rigidbody>();
        object[] data = info.photonView.InstantiationData;
        _rigidBody.velocity = (Vector3)data[0] * 18;
        //_camera = GameObject.Find("CameraHolder").GetComponentInChildren<Camera>();
        //Quaternion rot = _camera.transform.rotation;

        //Vector3 targetUp = rot * _camera.transform.up;
        //_rigidBody.velocity = (targetForward + targetUp)*5;
        //_rigidBody.velocity = _camera.transform.forward * 18;
        //if (_view.IsMine)
        //{
        //    _rigidBody.velocity = _camera.transform.forward * 18;
        //}
        //else
        //{
        //    rot *= Quaternion.Euler(0, 90, 0);
        //    Vector3 targetForward = rot * _camera.transform.forward;
        //    _rigidBody.velocity = targetForward * 18;
        //}
        Invoke("RunSmokeAnimation", 5f);
    }
}
