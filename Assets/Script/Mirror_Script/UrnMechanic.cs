using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class UrnMechanic : MonoBehaviour
{
    private VisualEffect _vfx;
    [SerializeField] private GameObject _urnGfx;
    public float _radiusDetection;
    public LayerMask _AIObject;
    private Collider[] _AI;
    public GameObject _AIlistObject;
    private EnemyAI AiScript;
    public bool check;
    public float waitingTime;
    public Transform otherPath;

    void Start()
    {
        _vfx = GetComponent<VisualEffect>();
        _vfx.Stop();
        check = true;
    }

    private void AreaTrigger()
    {
        AiScript = _AIlistObject.GetComponent<EnemyAI>();
        AiScript.fireCheck = true;
        if (!AiScript._bIsVip)
        {
            AiScript._otherPath = transform;
        }
        else if (AiScript._bIsVip)
        {
            AiScript._otherPath = otherPath;
        }
        AiScript.bot.ResetPath();
        AiScript.ChangePath(waitingTime);
    }

    public void activation(int p = 0)
    {
        if(p == 0)
        {
            _vfx.Play();
            _urnGfx.SetActive(false);
            AreaTrigger();
        }
    }

}
