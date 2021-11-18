using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private NavMeshAgent bot;
    public GameObject monPath;
    private Vector3[] destination;
    private int nextPoint;
    public bool loop;

    void Start()
    {
        bot = GetComponent<NavMeshAgent>();
        destination = new Vector3[monPath.transform.childCount];

        for (int i = 0; i < destination.Length; i++)
        {
            destination[i] = monPath.transform.GetChild(i).position;
        }

        nextDestination();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, destination[nextPoint]) < 0.2f)
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
        }
    }

    public void nextDestination()
    {
        bot.SetDestination(destination[nextPoint]);
    }
}
