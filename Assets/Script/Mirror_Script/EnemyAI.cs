using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    private NavMeshAgent bot;

    [Header("Animation")]
    [SerializeField] private Animator _anim;
    public float animspeed = 0.5f;
    //public Animation deathAnim;

    [Header("Patrol Path")]
    public bool loop;
    public GameObject monPath;
    private Vector3[] _destination;
    [SerializeField] private int _nextPoint;
    [SerializeField] private float _defaultSpeed;

    [Header("Trigger System")]
    private Collider[] _triggerArea;
    private bool _bGoBack;
    [SerializeField] private bool _bIsTrigger;
    [SerializeField] private bool _bFOVCheck;
    [SerializeField] private float _waitingTime;
    [SerializeField] private float _radius;
    [Range(0, 360)]
    [SerializeField] private float _angle;
    [SerializeField] private Transform _otherPath;
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private LayerMask _obstructionMask;
     

    void Start()
    {
        _bIsTrigger = false;
        _bGoBack = false;
        bot = GetComponent<NavMeshAgent>();
        _destination = new Vector3[monPath.transform.childCount];

        for (int i = 0; i < _destination.Length; i++)
        {
            _destination[i] = monPath.transform.GetChild(i).position;
        }

        _defaultSpeed = bot.speed;
        nextDestination();

        _bFOVCheck = true;
        StartCoroutine(FOVRoutine());
    }

    void Update()
    {

        #region Test Section

        if (Input.GetKeyDown(KeyCode.L))
        {
            bot.ResetPath();
            ChangePath();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            bot.speed = 6;
        }

        #endregion

        #region PrepPhaseSystem

        if (PrepPhaseSystem.instance.bPPSOn == true)
        {
            bot.speed = 0;
            _anim.speed = 0;
        }
        else if(PrepPhaseSystem.instance.bPPSOn == false)
        {
            bot.speed = _defaultSpeed;
            _anim.speed = bot.velocity.magnitude * animspeed;
            _anim.SetFloat("velocity", bot.velocity.magnitude * animspeed);
        }

        #endregion

        #region Patrol System

        if (!_bIsTrigger)
        {
            #region Original

            /*if (Vector3.Distance(transform.position, new Vector3(destination[nextPoint].x, transform.position.y, destination[nextPoint].z)) < 0.2f)
            {
                nextPoint++;

                if (!loop && nextPoint < monPath.transform.childCount)
                {
                    nextDestination();
                }
                if (loop && nextPoint < monPath.transform.childCount)
                {
                    nextDestination();
                }

                else if (loop && nextPoint >= monPath.transform.childCount)
                {
                    nextPoint = 0;
                    nextDestination();
                }
                
            }*/

            #endregion

            if (Vector3.Distance(transform.position, _destination[_nextPoint]) < 0.2f && loop)
            {
                _nextPoint++;

                if (_nextPoint < monPath.transform.childCount)
                {
                    nextDestination();
                }
                else if (_nextPoint >= monPath.transform.childCount)
                {
                    _nextPoint = 0;
                    nextDestination();
                }
            }
            else if (Vector3.Distance(transform.position, _destination[_nextPoint]) < 0.2f && !loop)
            {
                if (!_bGoBack)
                {
                    _nextPoint++;
                    if (_nextPoint < monPath.transform.childCount)
                    {
                        nextDestination();
                    }
                    else if (_nextPoint >= monPath.transform.childCount)
                    {
                        _nextPoint = monPath.transform.childCount - 1;
                        _bGoBack = true;
                    }
                }
                else if (_bGoBack)
                {
                    _nextPoint--;
                    if (_nextPoint > 0)
                    {
                        nextDestination();
                    }
                    else if (_nextPoint <= 0)
                    {
                        nextDestination();
                        _nextPoint = 0;
                        _bGoBack = false;
                    }
                }
            }
        }
        else if (_bIsTrigger)
        {
            /*if (Vector3.Distance(transform.position, _otherPath.position) < 0.2f)
            {
                _bIsTrigger = false;
                StartCoroutine(GoBackToPatrol());
            }*/

            if(Vector3.Distance(transform.position, _otherPath.position) < 1f && _bIsTrigger)
            {
                bot.isStopped = true;
            }
            else if (Vector3.Distance(transform.position, _otherPath.position) > 1f && _bIsTrigger)
            {
                bot.isStopped = false;
            }

        }

        #endregion



    }

    private void nextDestination()
    {
        // WaitForSecond + _anim.SetFloat("velocity", 0);

        bot.SetDestination(_destination[_nextPoint]);
    }

    private void ChangePath(/*Vector3 otherPath*/)
    {
        
        _bIsTrigger = true;
        //_otherPath = otherPath;
        bot.SetDestination(_otherPath.position);
    }

    private IEnumerator GoBackToPatrol()
    {
        yield return new WaitForSeconds(_waitingTime);
        nextDestination();
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FOVCheck();
        }
    }

    private void FOVCheck()
    {
        _triggerArea = Physics.OverlapSphere(transform.position, _radius, _targetMask);

        if(_triggerArea.Length != 0)
        {
            _otherPath = _triggerArea[0].transform;
            Vector3 directionToTarget = (_otherPath.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, directionToTarget) < _angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, _otherPath.position);

                if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstructionMask))
                {
                    //_bIsTrigger = true;

                    bot.ResetPath();
                    ChangePath();
                }
                else
                {
                    _bIsTrigger = false;
                    StartCoroutine(GoBackToPatrol());
                }
            }
            else
            {
                _bIsTrigger = false;
                StartCoroutine(GoBackToPatrol());
            }
        }
        else if (_bIsTrigger)
        {
            _bIsTrigger = false;
            StartCoroutine(GoBackToPatrol());
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);

        Vector3 v2 = new Vector3(Mathf.Sin((_angle / 2 + transform.eulerAngles.y) * Mathf.Deg2Rad), 0, Mathf.Cos((_angle/2 + transform.eulerAngles.y) * Mathf.Deg2Rad));
        Vector3 v1 = new Vector3(Mathf.Sin(((-_angle)/2 + transform.eulerAngles.y) * Mathf.Deg2Rad), 0, Mathf.Cos(((-_angle)/2 + transform.eulerAngles.y) * Mathf.Deg2Rad));

        //Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + v1 * _radius);
        Gizmos.DrawLine(transform.position, transform.position + v2 * _radius);

        if (_bIsTrigger)
        {
            Gizmos.DrawLine(transform.position, _otherPath.position);
        }
    }
}

