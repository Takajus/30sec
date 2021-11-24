using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    private NavMeshAgent bot;
    public GameObject monPath;
    private Vector3[] destination;
    [SerializeField] private Transform _otherPath;
    [SerializeField] private int nextPoint;
    [SerializeField] private float _waitingTime;
    public bool loop;
    private bool _bGoBack;
    [SerializeField] private bool _bIsTrigger;
    [SerializeField] private float _defaultSpeed;

    void Start()
    {
        _bIsTrigger = false;
        _bGoBack = false;
        bot = GetComponent<NavMeshAgent>();
        destination = new Vector3[monPath.transform.childCount];

        for (int i = 0; i < destination.Length; i++)
        {
            destination[i] = monPath.transform.GetChild(i).position;
        }

        _defaultSpeed = bot.speed;
        nextDestination();
    }

    void Update()
    {

        #region Test Section

        if (Input.GetKeyDown(KeyCode.L))
        {
            bot.speed = 0;
            // Animator.speed = 0;
            /*bot.ResetPath();
            ChangePath();*/
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            bot.speed = 6;
        }

        #endregion

        if(PrepPhaseSystem.instance.bPPSOn == true)
        {
            bot.speed = 0;
        }
        else if(PrepPhaseSystem.instance.bPPSOn == false)
        {
            bot.speed = _defaultSpeed;
        }

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

            if (Vector3.Distance(transform.position, destination[nextPoint]) < 0.2f && loop)
            {
                nextPoint++;

                if (nextPoint < monPath.transform.childCount)
                {
                    nextDestination();
                }
                else if (nextPoint >= monPath.transform.childCount)
                {
                    nextPoint = 0;
                    nextDestination();
                }
            }
            else if (Vector3.Distance(transform.position, destination[nextPoint]) < 0.2f && !loop)
            {
                if (!_bGoBack)
                {
                    nextPoint++;
                    if (nextPoint < monPath.transform.childCount)
                    {
                        nextDestination();
                    }
                    else if (nextPoint >= monPath.transform.childCount)
                    {
                        nextPoint = monPath.transform.childCount - 1;
                        _bGoBack = true;
                    }
                }
                else if (_bGoBack)
                {
                    nextPoint--;
                    if (nextPoint > 0)
                    {
                        nextDestination();
                    }
                    else if (nextPoint <= 0)
                    {
                        nextDestination();
                        nextPoint = 0;
                        _bGoBack = false;
                    }
                }
            }
        }
        else if (_bIsTrigger)
        {
            if (Vector3.Distance(transform.position, _otherPath.position) < 0.2f)
            {
                _bIsTrigger = false;
                StartCoroutine(GoBackToPatrol());
            }
                
        }
    }

    public void nextDestination()
    {
        bot.SetDestination(destination[nextPoint]);
    }

    public void ChangePath(/*Vector3 otherPath*/)
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
}

