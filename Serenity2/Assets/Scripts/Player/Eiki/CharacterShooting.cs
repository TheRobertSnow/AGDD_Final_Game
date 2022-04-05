using Photon.Pun;
using UnityEngine;

public class CharacterShooting : MonoBehaviour {
    public Gun gun;

    public int shootButton;
    public KeyCode reloadKey;

    private PhotonView _PV;
    private void Awake()
    {
        _PV = GetComponent<PhotonView>();
    }
    void Update() {
        if (_PV.IsMine)
        {
            if(Input.GetMouseButton(shootButton)) {
                gun.Shoot();
            }

            if(Input.GetKeyDown(reloadKey)) {
                gun.Reload();
            }
        }
    }
}