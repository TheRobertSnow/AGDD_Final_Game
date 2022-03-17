using System;
using UnityEngine;
using Photon.Pun;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public GameObject cameraHolder;
    [SerializeField] public float mouseSensitivity, sprintSpeed, walkSpeed, jumpForce, smoothTime;

    private Rigidbody _rb;
    private float _verticalLookRotation;
    private bool _grounded;
    private Vector3 _smoothMoveVelocity;
    private Vector3 _moveAmount;
    private PhotonView _view;

    private void Awake()
    {
        _view = GetComponent<PhotonView>();
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (!_view.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(_rb);
        }
    }

    private void Update()
    {
        if (!_view.IsMine)
        {
            return;
        }

        Look();
        Move();
        Jump();
    }

    private void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        _verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        _verticalLookRotation = Mathf.Clamp(_verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * _verticalLookRotation;
    }

    private void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        _moveAmount = Vector3.SmoothDamp(
            _moveAmount,
            moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref _smoothMoveVelocity, smoothTime);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _grounded)
        {
            _rb.AddForce(transform.up * jumpForce);
        }
    }

    public void SetGroundedState(bool grounded)
    {
        _grounded = grounded;
    }

    void FixedUpdate()
    {
        if (!_view.IsMine)
        {
            return;
        }
        
        // Move the character here because we dont have to worry about fps.
        _rb.MovePosition(_rb.position + transform.TransformDirection(_moveAmount) * Time.fixedDeltaTime);
    }
}