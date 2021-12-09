using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    public NavMeshAgent bot;

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
    public Collider[] _triggerArea;
    private bool _bGoBack;
    public bool _bIsTrigger;
    [SerializeField] private float _waitingTime;
    [SerializeField] private float _radius;
    private bool _oneTime;
    public bool fireCheck;
    public float fireDistance;
    [Range(0, 360)]
    [SerializeField] private float _angle;
    public Transform _otherPath;
    [SerializeField] private LayerMask _targetMask;
    [SerializeField] private LayerMask _obstructionMask;
    [SerializeField] private bool _bOldPPSOn;

    [Header("Detection")]
    [SerializeField] private float timeStart;
    private float timer;
     

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
        //nextDestination();
        StartCoroutine(GoBackToPatrol());

        timer = timeStart;
        _oneTime = true;
    }

    void Update()
    {
        if(_bOldPPSOn != PrepPhaseSystem.instance.bPPSOn && _bOldPPSOn)
        {
            nextDestination();
        }

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
            //bot.speed = _defaultSpeed;
            /*_anim.speed = bot.velocity.magnitude * animspeed;
            _anim.SetFloat("velocity", bot.velocity.magnitude * animspeed);*/

            if(bot.velocity.magnitude > 0.2 && !_bIsTrigger)
            {
                _anim.speed = bot.velocity.magnitude * animspeed;
                _anim.SetFloat("velocity", bot.velocity.magnitude * animspeed);
            }
            if(bot.velocity.magnitude < 0.2 && !_bIsTrigger)
            {
                _anim.speed = 1;
            }
        }

        #endregion

        #region Patrol System

        if (!_bIsTrigger && !PrepPhaseSystem.instance.bPPSOn)
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
                    StartCoroutine(GoBackToPatrol());
                    //nextDestination();
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
                        //nextDestination();
                        StartCoroutine(GoBackToPatrol());
                        _nextPoint = 0;
                        _bGoBack = false;
                    }
                }
            }
        }
        else if (_bIsTrigger && !PrepPhaseSystem.instance.bPPSOn)
        {
            /*if (Vector3.Distance(transform.position, _otherPath.position) < 0.2f)
            {
                _bIsTrigger = false;
                StartCoroutine(GoBackToPatrol());
            }*/

            if(/*Vector3.Distance(transform.position, _otherPath.position) < 5f*/ bot.remainingDistance < 5f)
            {
                bot.speed = 0;
                _anim.SetFloat("velocity", 0f);
                //print("speed 0");
            }
            else if (Vector3.Distance(transform.position, _otherPath.position) > 5f)
            {
                bot.speed = _defaultSpeed;
                _anim.SetFloat("velocity", bot.velocity.magnitude * animspeed);
            }

        }

        #endregion

        #region Detection

        if (_bIsTrigger && _otherPath.tag == "Player")
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                // ----------------------------------GameOver-------------------------------------
                print("GameOver");
            }
        }
        else if(!_bIsTrigger)
        {
            timer = timeStart;
        }

        #endregion

        //Debug.Log(Vector3.Distance(transform.position, _otherPath.position));
        //Debug.Log(bot.velocity.magnitude);

        _bOldPPSOn = PrepPhaseSystem.instance.bPPSOn;

        if (_oneTime && !fireCheck)
        {
            _oneTime = false;
            StartCoroutine(FOVRoutine());
        }
        if (fireCheck)
        {
            if (/*Vector3.Distance(transform.position, _otherPath.position) < 5f*/ bot.remainingDistance < fireDistance)
            {
                bot.speed = 0;
                _anim.SetFloat("velocity", 0f);
                //print("speed 0");
            }
            else if (Vector3.Distance(transform.position, _otherPath.position) > fireDistance)
            {
                bot.speed = _defaultSpeed;
                _anim.SetFloat("velocity", bot.velocity.magnitude * animspeed);
            }
        }

    }

    private void nextDestination()
    {
        // WaitForSecond + _anim.SetFloat("velocity", 0);

        bot.speed = _defaultSpeed;
        bot.SetDestination(_destination[_nextPoint]);
    }

    public void ChangePath(/*Vector3 otherPath*/ float waitTime = 0)
    {
        
        _bIsTrigger = true;
        //_otherPath = otherPath;
        bot.SetDestination(_otherPath.position);
        _waitingTime = waitTime;
    }

    private IEnumerator GoBackToPatrol()
    {
        yield return new WaitForSeconds(_waitingTime);
        nextDestination();
    }


    private IEnumerator FOVRoutine()
    {
        yield return new WaitForSeconds(0.2f);
        FOVCheck();
        _oneTime = true;
    }

    private void FOVCheck()
    {
        _triggerArea = Physics.OverlapSphere(transform.position, _radius, _targetMask);

        if(_triggerArea.Length != 0)
        {
            _otherPath = _triggerArea[0].transform;

            /*if(_otherPath.tag == "FireTest" && _otherPath.GetComponent<FireField>().bOnFire == true && _oneTime == 0)
            {
                print("test");
                _oneTime++;
                bot.ResetPath();
                ChangePath(10);
            }*/

            Vector3 directionToTarget = (_otherPath.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, directionToTarget) < _angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, _otherPath.position);

                if(!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, _obstructionMask))
                {
                    //_bIsTrigger = true;

                    bot.ResetPath();
                    ChangePath(5);
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

