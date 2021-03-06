using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class PlayerController2 : MonoBehaviour
{
    public CharacterController characterController;
    public float speed = 6;

    private float gravity = 9.87f;
    private float verticalSpeed = 0;

    public Transform cameraHolder;
    public float mouseSensitivity = 0.5f;
    public float upLimit = -50;
    public float downLimit = 50;

    private void Start() {
        Cursor.visible = false;
    }
    void Update()
    {
        Move();
        Rotate();
        if (Input.GetKey(KeyCode.Space))
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
    }
    private void Move()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        if(characterController.isGrounded) verticalSpeed = 0;
        else verticalSpeed -= gravity * Time.deltaTime;

        Vector3 gravityMove = new Vector3(0, verticalSpeed, 0);
        Vector3 move = transform.forward * verticalMove + transform.right * horizontalMove;
        characterController.Move(speed * Time.deltaTime * move + gravityMove * Time.deltaTime);
    }

    public void Rotate()
    {
        float horizontalRotation = Input.GetAxis("Mouse X");
        float verticalRotation = Input.GetAxis("Mouse Y");

        transform.Rotate(0, horizontalRotation * mouseSensitivity, 0);
        cameraHolder.Rotate(-verticalRotation*mouseSensitivity, 0, 0);

        Vector3 currentRotation = cameraHolder.localEulerAngles;
        if (currentRotation.x > 180) currentRotation.x -= 360;
        currentRotation.x = Mathf.Clamp(currentRotation.x, upLimit, downLimit);
        cameraHolder.localRotation = Quaternion.Euler(currentRotation);
    }

}
