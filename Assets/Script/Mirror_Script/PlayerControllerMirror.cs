using UnityEngine;
using Mirror;

[RequireComponent(typeof(PlayerMotor))] 
// Permet de dire que le script ne peux fonctionner ce dernier et ni le supprimer
public class PlayerControllerMirror : NetworkBehaviour
{
    #region Variable

    [SerializeField] private float speed = 3f;
    [SerializeField] private float crouchSpeed = 1.5f;
    private float currentSpeed;
    [SerializeField] private float mouseSensitivityX = 3f;
    [SerializeField] private float mouseSensitivityY = 3f;
    [SerializeField] private bool _cursorSet;

    [Header("Player Jump")]

    [SerializeField] private float thrusterForce = 1000f;
    private bool bGrounded;
    [SerializeField] private Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    [Header("Player Action")]

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

    [Header("Field Fire")]

    public GameObject fireField;

    public bool bCameraRotate;

    private PlayerMotor motor;
    [SerializeField] private Animator _anim;
    private Rigidbody rb;
    private bool _bIsCrouch;
    private CapsuleCollider col;

    #endregion

    private void Awake()
    {
        _targetPower = _handPower.GetChild(0);
        _targetHook = _handHook.GetChild(0);
        _cursorSet = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        currentSpeed = speed;
        bCameraRotate = true;
    }

    private void Start()
    {
        motor = GetComponent<PlayerMotor>();
        rb = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();

    }

    private void Update()
    {
        #region Movement

        float xMov = Input.GetAxisRaw("Horizontal"); // A = -1, D = 1
        float zMov = Input.GetAxisRaw("Vertical"); // S = -1, Z = 1

        Vector3 moveHorizontal = transform.right * xMov;
        Vector3 moveVertical = transform.forward * zMov;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * currentSpeed;

        motor.Move(velocity);

        _anim.SetFloat("velocity", rb.velocity.magnitude);

        #endregion

        #region Animation

        _anim.SetFloat("velocityZ", zMov);
        _anim.SetFloat("velocityX", xMov);

        #endregion

        #region Rotation Horizontal

        float yRot = Input.GetAxisRaw("Mouse X");

        if (bCameraRotate)
        {
            Vector3 rotation = new Vector3(0, yRot, 0) * mouseSensitivityX;
            motor.Rotate(rotation);
        }
        else if (!bCameraRotate)
        {
            Vector3 rotation = new Vector3(0, 0, 0);
            motor.Rotate(rotation);
        }

        

        #endregion

        #region Rotation Vertical

        float xRot = Input.GetAxisRaw("Mouse Y");

        if (bCameraRotate)
        {
            float cameraRotationX = xRot * mouseSensitivityY;
            motor.RotateCamera(cameraRotationX);
        }
        else if (!bCameraRotate)
        {
            float cameraRotationX = 0f;
            motor.RotateCamera(cameraRotationX);
        }

        #endregion

        #region Jump

        bGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        _anim.SetBool("bGrounded", bGrounded);

        //Calcul de la force du saut/ jetpack
        if (Input.GetButton("Jump") && bGrounded)
        {
            motor.ApplyThruster(Vector3.up * thrusterForce);
            if (_bIsCrouch)
            {
                _bIsCrouch = false;
                col.height = 1.8f;
                col.center = new Vector3(0, 0.9f, 0);
                currentSpeed = speed;

            }
        }
        // Appliquer la force
        

        #endregion

        #region Crouch

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            if (_bIsCrouch)
            {
                _bIsCrouch = false;
                col.height = 1.8f;
                col.center = new Vector3(0, 0.9f, 0);
                currentSpeed = speed;
                _anim.SetTrigger("CrouchEnd");

            }
            else if (!_bIsCrouch)
            {
                _bIsCrouch = true;
                col.height = 0.9f;
                col.center = new Vector3(0, 0.45f, 0);
                currentSpeed = crouchSpeed;
                _anim.SetTrigger("CrouchStart");
            }
        }

        #endregion

        #region _powerObject Check

        if (_powerObject != null) // Si il y a un objet dans _powerObject
        {
            _targetPower.position = _powerObject.transform.position;
        }

        if (_powerObject == null)
        {
            _targetPower.localPosition = Vector3.zero;
        }

        #endregion

        #region Fonction d'action

        /*_ray = _myCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // position de départ du rayon
        
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
                    if (_powerObject.GetComponent<ObjectTest>() == null)
                    {
                        _powerObject.AddComponent<ObjectTest>();
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
        }*/

        #endregion

        Laser();

        #region Cursor Settings

        if (Input.GetKeyDown(KeyCode.Escape) && _cursorSet)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _cursorSet = false;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !_cursorSet)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            _cursorSet = true;
        }

        #endregion
    }

    public void Laser()
    {
        _ray = _myCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // position de départ du rayon

        if (Input.GetButtonDown("Fire1"))
        {
            if (Physics.Raycast(_ray, out _hit, powerDistance))
            {
                if (_hit.transform.tag == "ObjectTest01" && _powerObject == null)
                {
                    _powerObject = _hit.collider.gameObject;
                    if (_powerObject.GetComponent<Rigidbody>() == null)
                    {
                        _powerObject.AddComponent<Rigidbody>();
                    }
                    if (_powerObject.GetComponent<ObjectTest>() == null)
                    {
                        _powerObject.AddComponent<ObjectTest>();
                    }
                    //_powerObject.SendMessage("Floating", true);
                    SendFloating(true);
                }
                else if(_hit.transform.tag == "FireTest" && PrepPhaseSystem.instance.bPPSOn == true)
                {
                    // Field Fire Function
                    Debug.Log("firetest laser");
                    _powerObject = _hit.collider.gameObject;
                    if (_powerObject.GetComponent<FireField>().bFlammable)
                    {
                        // Enregistrer l'action pour la VFX

                        //_powerObject.GetComponent<FireField>().LightningActivation();
                        //PrepPhaseSystem.instance.LightningActivationFireField(_powerObject);
                        PrepPhaseSystem.instance.AddEvent(_powerObject, 0);
                    }

                }
                else if(_hit.collider.tag == "Ballista" && PrepPhaseSystem.instance.bPPSOn == true)
                {
                    Debug.Log("Arrow Light");
                    _powerObject = _hit.collider.gameObject;
                    PrepPhaseSystem.instance.AddEvent(_powerObject, 0);
                    //_powerObject.GetComponent<Ballista>().ArrowShooting();
                    //PrepPhaseSystem.instance.letsGoPhysicObject(_powerObject);
                    
                }
                else if(_hit.collider.tag == "Item" && PrepPhaseSystem.instance.bPPSOn == true)
                {
                    _powerObject = _hit.collider.gameObject;
                    PrepPhaseSystem.instance.AddEvent(_powerObject, 0);
                }
                

            }
            Vector3 foward = _myCam.transform.TransformDirection(Vector3.forward) * 10;
            Debug.DrawRay(_myCam.transform.position, foward, Color.red, 10);
        }

        if (Input.GetButtonUp("Fire1") && _powerObject != null)
        {
            /*
            //_powerObject.SendMessage("Floating", false);
            SendFloating(false);
            if (Physics.Raycast(_ray, out _hit, powerDistance))
            {
                if (_hit.collider.gameObject != _powerObject)
                {
                    _powerDirection = _hit.point + Vector3.up;
                    //_powerObject.GetComponent<ObjectTest01>().transform.localPosition = _powerDirection;
                    //_powerObject.SendMessage("Launching", _powerDirection);
                    SendLaunching(_powerDirection);
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
                //_powerObject.SendMessage("Launching", _powerDirection);
                SendLaunching(_powerDirection);
                //_powerObject.GetComponent<physicObject>().target = _powerDirection;
                //_powerObject.SendMessage("letsGo");
                _powerObject = null;
            }*/
            _powerObject = null;
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (Physics.Raycast(_ray, out _hit, powerDistance))
            {
                if (_hit.transform.tag == "ObjectTest01" && _powerObject == null)
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
                else if(_hit.collider.tag == "Ballista" && PrepPhaseSystem.instance.bPPSOn == true)
                {
                    Debug.Log("Arrow Wind");
                    _powerObject = _hit.collider.gameObject;
                    PrepPhaseSystem.instance.AddEvent(_powerObject, 1);
                    /*_powerObject = _hit.collider.gameObject;
                    _powerObject.GetComponent<Ballista>().bCanRotate = true;*/
                    //bCameraRotate = false;
                }
                else if(_hit.collider.tag == "Item" && PrepPhaseSystem.instance.bPPSOn == true)
                {
                    Debug.Log("Lamp Wind");
                    _powerObject = _hit.collider.gameObject;
                    PrepPhaseSystem.instance.AddEvent(_powerObject, 1);
                }
            }
        }
    }

    public void SendFloating(bool bFloating)
    {
        _powerObject.SendMessage("Floating", bFloating);
    }

    public void SendLaunching(Vector3 powerDirection)
    {
        _powerObject.SendMessage("Launching", powerDirection);
    }
}
