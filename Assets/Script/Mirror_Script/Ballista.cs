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
    //[SerializeField] private bool _bReadyToShoot;

    [Header("GD Var")]
    [SerializeField] private float mouseSensitivity = 1f;
    // bullet
    [SerializeField] private GameObject _arrow;
    [SerializeField] private float _shootForce, _upwardForce;
    

    void Start()
    {
        //_bReadyToShoot = false;
        bCanRotate = false;
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

    public void ArrowShooting()
    {
        //_bReadyToShoot = true;

        Vector3 shootingDirection = new Vector3(0, _turretY.transform.position.y, _turretZ.transform.position.z);

        GameObject currentArrow = Instantiate(_arrow, _firePosition.position, Quaternion.identity);
        currentArrow.transform.forward = shootingDirection.normalized;
        currentArrow.GetComponent<Rigidbody>().AddForce(shootingDirection.normalized * _shootForce, ForceMode.Impulse);


    }
}
