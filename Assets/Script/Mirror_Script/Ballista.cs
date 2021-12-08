using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballista : MonoBehaviour
{
    [Header("Dev Var")]
    [SerializeField] private GameObject _turretY;
    [SerializeField] private GameObject _turretZ;
    [SerializeField] private Transform _firePosition;
    public bool bCanRotate;
    public GameObject physicObeject;
    public bool p0;
    public bool p1;

    [Header("GD Var")]
    [SerializeField] private float mouseSensitivity = 1f;
    // bullet
    [SerializeField] private GameObject _arrow;
    [SerializeField] private float _shootForce, _upwardForce;
    

    void Start()
    {
        //_bReadyToShoot = false;
        bCanRotate = false;
        p1 = true;
        p0 = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (bCanRotate)
        {
            TurnTheBallista();
        }
        /*if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ArrowShooting();
        }*/
    }

    public void TurnTheBallista()
    {
        float yRot = Input.GetAxisRaw("Mouse X");
        float zRot = Input.GetAxisRaw("Mouse Y");
        _turretY.transform.Rotate(new Vector3(0, yRot, 0) * mouseSensitivity);
        _turretZ.transform.Rotate(new Vector3(0, 0, zRot) * mouseSensitivity);
    }

    public void activation(int p = 0)
    {
        //_bReadyToShoot = true;

        /*Vector3 shootingDirection = new Vector3(0, _turretY.transform.rotation.y, _turretZ.transform.rotation.z);

        GameObject currentArrow = Instantiate(_arrow, _firePosition.position, Quaternion.identity);
        currentArrow.transform.forward = shootingDirection.normalized;
        currentArrow.GetComponent<Rigidbody>().AddForce(shootingDirection.normalized * _shootForce, ForceMode.Impulse);*/
        if (p == 0 && !p0)
        {
            p0 = true;
            physicObeject.GetComponent<PhysicObject>().letsGo();
        }
        if(p == 1 && !p1)
        {
            p1 = true;
            Debug.LogWarning("Ballista Wind");
        }
    }
}
