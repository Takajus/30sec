using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballista : MonoBehaviour
{
    [Header("Dev Var")]
    [SerializeField] private Transform _firePosition, _fireDirection;
    public GameObject physicObeject, GFX;
    public bool p0;
    public bool p1;

    [Header("GD Var")]
    [SerializeField] private float turnSpeed;
    

    void Start()
    {
        //_bReadyToShoot = false;
        p1 = false;
        p0 = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ArrowShooting();
        }*/
    }

    public void TurnTheBallista()
    {
        Vector3 dir = _fireDirection.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(GFX.transform.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        GFX.transform.rotation = Quaternion.Euler(-90f, rotation.y + 90f, 90f);
        _firePosition.rotation = Quaternion.Euler(0f, rotation.y, 0f);
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
            TurnTheBallista();
            Debug.LogWarning("Ballista Wind");
        }
    }
}
