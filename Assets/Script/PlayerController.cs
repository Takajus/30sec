using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviourPunCallbacks
{

    #region Variable

    [Header("Player Movements")]

    public CharacterController controller;
    public GameObject cameraHolder;

    public float speed = 6f;
    public float gravity = -9.18f;
    public float jumpHeighht = 3f;

    [SerializeField] float mouseSensitivity;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private float _verticalLookRotation;
    private bool bGrounded;
    private Vector3 _velocity;
    private Vector3 _moveAmount;

    PhotonView PV;

    private bool _cursorSet;

    [Header("Player Actions")]

    public float powerDistance;
    [SerializeField] private Transform _handPower;
    [SerializeField] private Transform _handHook;
    private Transform _targetPower;
    private Transform _targetHook;

    private Vector3 _powerDirection;
    [SerializeField] private Camera _myCam;
    private GameObject _powerObject;
    private RaycastHit _hit;
    private Ray _ray;

    #endregion

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        controller = GetComponent<CharacterController>();
        _targetPower = _handPower.GetChild(0);
        _targetHook = _handHook.GetChild(0);
        _cursorSet = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
        }
    }

    private void Update()
    {
        if (!PV.IsMine)
            return;

        if (PV.IsMine)
        {
            MovementInput();
            Look();
        }
        

        #region _powerObject Check

        if (_powerObject != null) // Si il y a un objet dans _powerObject
        {
            _targetPower.position = _powerObject.transform.position;
        }

        if(_powerObject == null)
        {
            _targetPower.localPosition = Vector3.zero;
        }

        #endregion

        #region Ray

        _ray = _myCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // position de départ du rayon
        
        if(Input.GetButtonDown("Fire1"))
        {
            if(Physics.Raycast(_ray, out _hit, powerDistance))
            {
                if(_hit.transform.tag == "ObjectTest01" && _powerObject == null)
                {
                    _powerObject = _hit.collider.gameObject;
                    if(_powerObject.GetComponent<Rigidbody>() == null)
                    {
                        _powerObject.AddComponent<Rigidbody>();
                    }
                    if (_powerObject.GetComponent<ObjectTest01>() == null)
                    {
                        _powerObject.AddComponent<ObjectTest01>();
                    }
                    _powerObject.SendMessage("Floating", true);
                }

            }
            Vector3 foward = _myCam.transform.TransformDirection(Vector3.forward) * 10;
            Debug.DrawRay(_myCam.transform.position, foward, Color.red, 10);
        }

        if (Input.GetButtonUp("Fire1") && _powerObject != null)
        {
            _powerObject.SendMessage("Floating", false);
            if (Physics.Raycast(_ray, out _hit, powerDistance))
            {
                if (_hit.collider.gameObject != _powerObject)
                {
                    _powerDirection = _hit.point + Vector3.up;
                    //_powerObject.GetComponent<ObjectTest01>().transform.localPosition = _powerDirection;
                    _powerObject.SendMessage("Launching", _powerDirection);
                    _powerObject = null;
                }
                if (_hit.collider.gameObject == _powerObject)
                {
                    //_powerObject.SendMessage("annulation");
                    _powerObject = null;
                }
            }
            else
            {
                _powerDirection = _myCam.transform.position + _myCam.transform.forward * powerDistance;
                _powerObject.SendMessage("Launching", _powerDirection);
                //_powerObject.GetComponent<physicObject>().target = _powerDirection;
                //_powerObject.SendMessage("letsGo");
                _powerObject = null;
            }
            
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if(Physics.Raycast(_ray, out _hit, powerDistance))
            {
                if(_hit.transform.tag == "ObjectTest01" && _powerObject == null)
                {
                    _powerObject = _hit.collider.gameObject;
                    if (_powerObject.GetComponent<Rigidbody>() == null)
                    {
                        _powerObject.AddComponent<Rigidbody>();
                    }
                    if (_powerObject.GetComponent<ObjectTest01>() == null)
                    {
                        _powerObject.AddComponent<ObjectTest01>();
                    }
                    _powerObject.GetComponent<Rigidbody>().AddForce(gameObject.transform.forward * 20f);
                }
            }
        }

        #endregion

        #region Cursor Settings

        if (Input.GetKeyDown(KeyCode.Escape) && _cursorSet)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _cursorSet = false;
        }
        if(Input.GetKeyDown(KeyCode.Escape) && !_cursorSet)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _cursorSet = true;
        }

        #endregion

    }

    private void FixedUpdate()
    {
        
    }

    void MovementInput()
    {
        bGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(bGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        /*float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        controller.Move(move * speed * Time.deltaTime);*/

        if(Input.GetButtonDown("Jump") && bGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeighht * -2 * gravity);
        }

        _velocity.y += gravity * Time.deltaTime;

        controller.Move(_velocity * Time.deltaTime);

        if (Input.GetKey(KeyCode.Z))
        {
            controller.Move(transform.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            controller.Move(-transform.right * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            controller.Move(-transform.forward * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            controller.Move(transform.right * Time.deltaTime * speed);
        }
    }

    public void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);

        _verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        _verticalLookRotation = Mathf.Clamp(_verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * _verticalLookRotation;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
