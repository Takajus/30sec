using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FireField : MonoBehaviour
{
    public bool bOnFire;
    [SerializeField] private bool bActivation;
    public bool bFlammable;
    private Collider[] _hit;
    private Collider[] _AI;
    [SerializeField] private float _radiusDetection;
    [SerializeField] private LayerMask _flammableObject, _AIObject;
    private GameObject _fieldObject;
    private GameObject _AIlistObject;
    [SerializeField] private float radius = 3.0f;
    [SerializeField] private float _fireChance;

    public bool test;
    public Material _material, _material1;

    private EnemyAI AiScript;
    private FieldMecanic fieldParentScript;

    public VisualEffect flamme;

    // Start is called before the first frame update
    void Start()
    {
        flamme = transform.GetChild(0).GetComponent<VisualEffect>();
        fieldParentScript = transform.parent.GetComponent<FieldMecanic>();
        bOnFire = false;
        bActivation = false;
        bFlammable = true;
        flamme.SetFloat("SpawnRate", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && test)
        {
            activation();
        }

        if (bOnFire && bActivation)
        {
            bOnFire = false;

            StartCoroutine(SetOnFire());
            fieldParentScript.AIAreaCall();
        }
    }

    public IEnumerator SetOnFire()
    {
        yield return new WaitForSeconds(1);

        _hit = Physics.OverlapSphere(transform.position, radius, _flammableObject);

        foreach (Collider fireField in _hit)
        {
            _fieldObject = fireField.gameObject;

            if (fireField.tag == "FireTest" && _fieldObject.GetComponent<FireField>().bFlammable)
            {
                int rand = Random.Range(1, 100);

                if (_fieldObject.GetComponent<FireField>()._fireChance > rand)
                {
                    _fieldObject.GetComponent<FireField>().activation();
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

    public void activation(int p = 0)
    {
        if (p == 0)
        {
            if (!bActivation)
            {
                bFlammable = false;
                bActivation = true;
                bOnFire = true;

                // Fire VFX Activation
                flamme.SetFloat("SpawnRate", 20);

                gameObject.GetComponent<MeshRenderer>().material = _material;

                StartCoroutine(EndFire());
            }
        }
    }

    public IEnumerator EndFire()
    {
        yield return new WaitForSeconds(Random.Range(10, 15));

        flamme.SetFloat("SpawnRate", 0);
        bActivation = false;
        fieldParentScript.check = false;
        gameObject.GetComponent<MeshRenderer>().material = _material1;
    }

    /*public IEnumerator AIAreaCheck()
    {
        yield return new WaitForSeconds(0.2f);

        _AI = Physics.OverlapSphere(transform.position, _radiusDetection, _AIObject);

        foreach(Collider AIobject in _AI)
        {
            _AIlistObject = AIobject.gameObject;

            if(AIobject.tag == "AI")
            {
                print("ttttt");
                AiScript = _AIlistObject.GetComponent<EnemyAI>();
                AiScript._otherPath.position = transform.position;
                AiScript.bot.ResetPath();
                AiScript.ChangePath(10);
            }
        }
    }*/

}
