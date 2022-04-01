using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    CreateAndJoinRooms manager;

    private void Awake() 
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        manager = FindObjectOfType<CreateAndJoinRooms>();
        manager.OnJoinedRoom();
    }
}
