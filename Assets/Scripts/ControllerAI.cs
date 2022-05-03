using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; 

public class ControllerAI : MonoBehaviour
{
    // VARIABLES

    public NavMeshAgent thisAgent;
    public float startWaitTime = 4f;
    public float timeToRotate = 2f;
    public float speedWalk = 6f;
    public float speedRun = 9f;

    public float viewRadius = 15f;
    public float viewAngle = 90f;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1f;
    public int edgeteration = 4;
    public float edgeDistance = 0.5f;

    // Array with patrol points
    [SerializeField]
    private Transform[] waypoints;
    int curWaypointIndex;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 m_PlayerPosition;

    float m_WaitTime;
    float m_TimeRotate;
    bool m_PlayerInRage;
    bool m_PlayerNear;
    bool m_IsPatrol;
    bool m_CaughtPlayer;





    // Start is called before the first frame update
    void Start()
    {
        // Inizialze Variables
        m_PlayerPosition = Vector3.zero;
        m_IsPatrol = true;
        m_CaughtPlayer = false;
        m_PlayerInRage = false;
        m_WaitTime = startWaitTime;
        m_TimeRotate = timeToRotate;

        curWaypointIndex = 0;
        thisAgent.isStopped = false;
        thisAgent.speed = speedWalk;
        thisAgent.SetDestination(waypoints[curWaypointIndex].position);
    }

    // Update is called once per frame
    void Update()
    {
        EnviromentView();

        if (!m_IsPatrol)
        {
            ChasingPlayer();
        }
        else
        {
            Patrolling();
        }
    }

    private void ChasingPlayer()
    {
        m_PlayerNear = false;
        playerLastPosition = Vector3.zero;

        if (!m_CaughtPlayer)
        {
            Move(speedRun);
            thisAgent.SetDestination(m_PlayerPosition);
        }
        if(thisAgent.remainingDistance <= thisAgent.stoppingDistance)
        {
            if(m_WaitTime <= 0 && !m_CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                m_IsPatrol = true;
                m_PlayerNear = false;
                Move(speedWalk);
                m_TimeRotate = timeToRotate;
                m_WaitTime = startWaitTime;
                thisAgent.SetDestination(waypoints[curWaypointIndex].position);
            }
            else
            {
                if(Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                {
                    Stop();
                    m_WaitTime -= Time.deltaTime; 
                }
            }
        }
    }

    private void Patrolling()
    {
        // need to check  if enemy is near the player
        if (m_PlayerNear)
        {
            if(m_TimeRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);

            }
            else
            {
                Stop();
                m_TimeRotate -= Time.deltaTime;
            }
        }
        else
        {
            m_PlayerNear = false;
            playerLastPosition = Vector3.zero;
            thisAgent.SetDestination(waypoints[curWaypointIndex].position);
            if(thisAgent.remainingDistance <= thisAgent.stoppingDistance)
            {
                if(m_WaitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    m_WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    m_TimeRotate -= Time.deltaTime; 
                }
            }
        }
    }

    void Move(float speed) // will make enemy move
    {
        thisAgent.isStopped = false;
        thisAgent.speed = speed;
    }

    void Stop() // will stop enemy movement 
    {
        thisAgent.isStopped = true;
        thisAgent.speed = 0;
    }

    public void NextPoint()
    {
        curWaypointIndex = (curWaypointIndex + 1) % waypoints.Length;
        thisAgent.SetDestination(waypoints[curWaypointIndex].position);
    }

    void CaughtPlayer()
    {
        m_CaughtPlayer = true;
    }


    void LookingPlayer(Vector3 player) // Parameter: last position of the player
    {
        thisAgent.SetDestination(player);
        if(Vector3.Distance(transform.position, player) <= 0.3)
        {
            if(m_WaitTime <= 0)
            {
                m_PlayerNear = false;
                Move(speedWalk);
                thisAgent.SetDestination(waypoints[curWaypointIndex].position);
                m_WaitTime = startWaitTime;
                m_TimeRotate = timeToRotate;

            }
            else
            {
                Stop();
                m_WaitTime -= Time.deltaTime;
            }
        }
    }

    void EnviromentView() // Allows enemy to see the player 
    {
        Collider[] playerInRange = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for (int i = 0; i < playerInRange.Length; i++)
        {
            Transform player = playerInRange[i].transform;
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, directionToPlayer) < viewAngle / 2)
            {
                float destinationToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, directionToPlayer, destinationToPlayer, obstacleMask))
                {
                    m_PlayerInRage = true;
                    m_IsPatrol = false;
                }
                else
                {
                    m_PlayerInRage = false;
                }
            }

            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                m_PlayerInRage = false;
            }


            if (m_PlayerInRage)
            {
                m_PlayerPosition = player.transform.position;
            }

        }
    }



} // CODE END
