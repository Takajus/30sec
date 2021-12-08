using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldMecanic : MonoBehaviour
{
    [Header("GD Var")]
    public float waitingTime;

    [Header("Dev Var")]
    public float _radiusDetection;
    public LayerMask _AIObject;
    private Collider[] _AI;
    public GameObject _AIlistObject;
    private EnemyAI AiScript;
    public bool check;

    private void Start()
    {
        check = true;
    }

    public void AIAreaCall()
    {
        _AI = Physics.OverlapSphere(transform.position, _radiusDetection, _AIObject);

        foreach (Collider AIobject in _AI)
        {
            _AIlistObject = AIobject.gameObject;

            if (AIobject.tag == "AI" && !_AIlistObject.GetComponent<EnemyAI>()._bIsTrigger && check)
            {
                AiScript = _AIlistObject.GetComponent<EnemyAI>();
                AiScript.fireCheck = true;
                AiScript._otherPath = transform;
                AiScript.bot.ResetPath();
                AiScript.ChangePath(waitingTime);
            }
            else if(!check && AIobject.tag == "AI")
            {
                AiScript = _AIlistObject.GetComponent<EnemyAI>();
                AiScript.fireCheck = false; 
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, _radiusDetection);
    }
}
