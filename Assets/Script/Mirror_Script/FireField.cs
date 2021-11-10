using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireField : MonoBehaviour
{
    [SerializeField] private bool bOnFire;
    [SerializeField] private bool bActivation;
    public bool bFlammable;
    private Collider[] _hit;
    [SerializeField] private LayerMask flammableObject;
    private GameObject _fieldObject;
    [SerializeField] private float radius = 3.0f;
    [SerializeField] private float _fireChance;

    public bool test;
    public Material _material, _material1;

    // Start is called before the first frame update
    void Start()
    {
        bOnFire = false;
        bActivation = false;
        bFlammable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && test)
        {
            LightningActivation();
        }

        if (bOnFire)
        {
            bOnFire = false;

            StartCoroutine(SetOnFire());
        }
    }

    public IEnumerator SetOnFire()
    {
        yield return new WaitForSeconds(1);

        _hit = Physics.OverlapSphere(transform.position, radius, flammableObject);

        foreach (Collider fireField in _hit)
        {
            _fieldObject = fireField.gameObject;

            if (fireField.tag == "FireTest" && _fieldObject.GetComponent<FireField>().bFlammable)
            {
                int rand = Random.Range(1, 100);

                if (_fieldObject.GetComponent<FireField>()._fireChance > rand)
                {
                    _fieldObject.GetComponent<FireField>().LightningActivation();
                }
                else if (_fieldObject.GetComponent<FireField>()._fireChance < rand)
                {
                    _fieldObject.GetComponent<FireField>()._fireChance += Random.Range(1, 16);
                }
            }
        }

        bOnFire = true;
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    public void LightningActivation()
    {
        if (!bActivation)
        {
            bFlammable = false;
            bActivation = true;
            bOnFire = true;

            // Fire VFX Activation
            gameObject.GetComponent<MeshRenderer>().material = _material;

            StartCoroutine(EndFire());
        }
    }

    public IEnumerator EndFire()
    {
        yield return new WaitForSeconds(Random.Range(10, 15));

        gameObject.GetComponent<MeshRenderer>().material = _material1;
    }
}
