using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class AIController : MonoBehaviour
{

    public enum AI_States
    {
        Roaming,
        FollowPlayer,
        Attack
    }

    public AI_States currentState = AI_States.Roaming;

    public NavMeshAgent thisAgent;
    public GameObject playerTarget;

    [Header("Waypoints")]
    public bool findWaypointsByStart;
    public GameObject[] waypoints;
    public GameObject currentWayPoint;
    public int curWaypointIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (findWaypointsByStart) waypoints = GameObject.FindGameObjectsWithTag("Waypoints");
        if (playerTarget == null) GameObject.FindGameObjectWithTag("Player");

        thisAgent = GetComponent<NavMeshAgent>();

        curWaypointIndex = UnityEngine.Random.Range(0, waypoints.Length);
        currentWayPoint = waypoints[curWaypointIndex];

        thisAgent.SetDestination(waypoints[curWaypointIndex].transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        ExicuteStates();
    }

    private void ExicuteStates()
    {

        switch (currentState)
        {
            case AI_States.Roaming:
                CheckWaypoints();
                break;
            case AI_States.FollowPlayer:
                FollowPlayer();
                break;
            case AI_States.Attack:
                break;
        }
    }

    private void FollowPlayer()
    {
        Debug.Log("Following the Player");
        if (Vector3.Distance(transform.position, playerTarget.transform.position) > 10)
        {
            Debug.Log("Going back to Path");
            currentState = AI_States.Roaming;
        }

        currentWayPoint = playerTarget;
        thisAgent.SetDestination(currentWayPoint.transform.position);


    }

    private void CheckWaypoints()
    {
        Debug.Log("Checking for Path");
        if(Vector3.Distance(transform.position, playerTarget.transform.position) < 20)
        {
            currentState = AI_States.FollowPlayer;
        }

        int tempWaypoint = curWaypointIndex;

        if(thisAgent.remainingDistance < 2)
        {
            while (curWaypointIndex == tempWaypoint)
            {
                curWaypointIndex = Random.Range(0, waypoints.Length);
            }

            currentWayPoint = waypoints[curWaypointIndex];
            thisAgent.SetDestination(currentWayPoint.transform.position);
        }
    }

   
}
