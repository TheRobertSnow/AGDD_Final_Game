using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public GameObject cameraHolder;
    [SerializeField] public Camera camera;
    [SerializeField] public float sprintSpeed, walkSpeed, jumpForce, smoothTime;
    [SerializeField] public TextMesh playerName;
    [SerializeField] public GameObject smokePrefab;
    [SerializeField] public int numberOfSmokes;
    [SerializeField] public float energy = 100f;

    private Rigidbody _rb;
    private float _verticalLookRotation;
    private bool _grounded;
    private Vector3 _smoothMoveVelocity;
    private Vector3 _moveAmount;
    private PhotonView _view;
    private int _team; // 0 = blue, 1 = red, 2 = spectator

    private Slider _energySlider;
    private Image _energySliderImage;

    public Animator modelAnimator;
    Player player;

    private bool _playerBlownUp = false;

    private TMP_Text _grenadeText;

    private void Awake()
    {
        _view = GetComponent<PhotonView>();
        _rb = GetComponent<Rigidbody>();
        if ((int)PhotonNetwork.LocalPlayer.CustomProperties["team"] != 2)
        {
            _energySlider = GameObject.Find("SliderYellow").GetComponent<Slider>();
            _energySliderImage = _energySlider.GetComponentInChildren<Image>();
            _grenadeText = GameObject.Find("SmokeCountText").GetComponent<TMP_Text>();
            Debug.Log(_grenadeText);
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerName.text = PhotonNetwork.LocalPlayer.NickName;
        _team = (int)PhotonNetwork.LocalPlayer.CustomProperties["team"];
        if (!_view.IsMine)
        {
            Destroy(_rb);
            SetName();
        }
        else
        {
            camera.gameObject.SetActive(true);
        }
        //Destroy(GetComponentInChildren<Camera>().gameObject);


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
        throwSmoke();
        //if (!_playerBlownUp)
        //{
        //    CheckBounds();
        //}
        //else
        //{
        //    _rb.velocity = Vector3.zero;
        //}
    }

    private void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * PlayerPrefs.GetFloat("sensitivity"));

        _verticalLookRotation += Input.GetAxisRaw("Mouse Y") * PlayerPrefs.GetFloat("sensitivity");
        _verticalLookRotation = Mathf.Clamp(_verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * _verticalLookRotation;
    }

    private bool isSprinting()
    {
        return Input.GetKey(KeyCode.LeftShift) && energy > 0f;
    }

    private void UpdateEnergy()
    {
        if (_team != 2)
        {
            if (!isSprinting())
            {
                if (energy < 5f)
                {
                    // energy is restore slower if almost empty to stop spam
                    energy = Math.Min(100f, energy + Time.fixedDeltaTime);
                }
                else
                {
                    energy = Math.Min(100f, energy + Time.fixedDeltaTime * 5f);
                }
                
            }
            else
            {
                // deplete energy
                energy = Math.Max(-5f, energy - Time.fixedDeltaTime * 20f);
            }

            _energySlider.value = Math.Max(0, energy);

            _energySliderImage.color = energy < 10f ? new Color(161, 139, 50) : new Color(255, 218, 0);

        }
    }

    private void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        _moveAmount = Vector3.SmoothDamp(
            _moveAmount,
            moveDir * (isSprinting() ? sprintSpeed : walkSpeed), ref _smoothMoveVelocity, smoothTime);

        float velocityZ = Vector3.Dot(moveDir, transform.forward);
        float velocityX = Vector3.Dot(moveDir, transform.right);

        modelAnimator.SetFloat("VelocityZ", velocityZ , 0.1f, Time.deltaTime);
        modelAnimator.SetFloat("VelocityX", velocityX , 0.1f, Time.deltaTime);
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
        UpdateEnergy();
    }

    private void SetName()
    {
        playerName.text = _view.Owner.NickName;
    }

    public void throwSmoke()
    {
        if (Input.GetKeyDown(KeyCode.G) && _view.IsMine && _team != 2)
        {
            if (numberOfSmokes > 0)
            {
                System.Object[] data = new System.Object[1];
                Vector3 velocity = GameObject.Find("CameraHolder").GetComponentInChildren<Camera>().transform.forward;
                data[0] = velocity * 2;
                Vector3 spawn = GameObject.Find("SmokeSpawn").transform.position;
                PhotonNetwork.Instantiate("smokeCan", spawn, Quaternion.identity, data: data);
                --numberOfSmokes;
            }
        }
        
        // update text for number of smokes
        _grenadeText.text = numberOfSmokes.ToString();
    }

    public void IncrementSmokeCount()
    {
        ++numberOfSmokes;
    }

    public void ReloadSmokes()
    {
        numberOfSmokes = 3;
    }
    public void ReloadEnergy()
    {
        energy = 100f;
    }

    //private void CheckBounds()
    //{
    //    Vector3 max1 = GameManager.Instance.MAXBOUNDS1;
    //    Vector3 max2 = GameManager.Instance.MAXBOUNDS2;
    //    Vector3 min1 = GameManager.Instance.MINBOUNDS1;
    //    Vector3 min2 = GameManager.Instance.MINBOUNDS2;
    //    if (((transform.position.x > max1.x) && (transform.position.z > max1.z))
    //        || ((transform.position.x > max2.x) && (transform.position.z < max2.z)))
    //    {
    //        Debug.Log("Bigger");
    //        BlowUpPlayer();
    //    }
    //    else if (((transform.position.x < min1.x) && (transform.position.z > min1.z)) ||
    //        ((transform.position.x < min2.x) && (transform.position.z < min2.z)))
    //    {
    //        Debug.Log("Smaller");
    //        BlowUpPlayer();
    //    }
    //    else if (transform.position.y < min1.y)
    //    {
    //        BlowUpPlayer();
    //    }
    //}

    //public void FixCamPos()
    //{
    //    Camera cam = cameraHolder.GetComponentInChildren<Camera>();
    //    cam.transform.position = new Vector3(0f, 0.5f, 0f);
    //    cam.transform.rotation = Quaternion.identity;
    //}

    //public void BlowUpPlayer()
    //{
    //    Camera cam = cameraHolder.GetComponentInChildren<Camera>();
    //    //cam.transform.position = new Vector3(0f, 3.76999998f, -8.76000023f);
    //    //cam.transform.rotation = Quaternion.Euler(16.2700024f, 0f, 0f);
    //    if (this.GetComponent<Animation>() != null) this.GetComponent<Animation>().Play();
    //    // Add pos
    //    _view.RPC(nameof(SpawnConfetti), RpcTarget.MasterClient);
    //    _playerBlownUp = true;
    //}

    //[PunRPC]
    //public void SpawnConfetti()
    //{
    //    PhotonNetwork.Instantiate("PlayerConfetti", new Vector3(0f, 0f, 0f), Quaternion.identity);
    //}
}