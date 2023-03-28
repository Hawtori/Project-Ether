using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player; //, vent;
                             //public Transform[] vents;
                             //public GameObject ventMaster;

    public LayerMask isGround, isPlayer; //, isVent;

    //Idle Walking
    public Vector3 walkPoint;
    bool walkPointSet;
    public float wpRange;

    //Attacking
    public float attackCooldown;
    bool didAttack;

    //States
    public float visionRadius, attackRange; //, ventLocator;
    public bool playerVisible, playerCanAttacked; //, ventFound, goVent;

    //Vent Times
   // private float goVentCountdown = 10.0f;

    private void Awake()
    {
        

        player = GameObject.Find("Player").transform;
        //ventMaster = GameObject.Find("VentMaster");
        //vents = ventMaster.GetComponentsInChildren<Transform>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerVisible = Physics.CheckSphere(transform.position, visionRadius, isPlayer);
        playerCanAttacked = Physics.CheckSphere(transform.position, attackRange, isPlayer);
        //ventFound = Physics.CheckSphere(transform.position, ventLocator, isVent);

        if (!playerVisible && !playerCanAttacked /*&& !goVent*/) Patrol();
        //if (!playerVisible && !playerCanAttacked /*&& goVent && ventFound*/) Vent();
        if (playerVisible && !playerCanAttacked)
        {
            //goVent = false;
            Chase();
        }
        if (playerVisible && playerCanAttacked)
        {
            //goVent = false;
            Attack(); 
        }
    }

    private void Patrol()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (CheckIfOnNavMesh() && walkPointSet)
            agent.SetDestination(walkPoint);
   

        Vector3 disToWalkPoint = transform.position - walkPoint;

        if (disToWalkPoint.magnitude < 0.5f)
            walkPointSet = false;
       // goVentCountdown -= Time.deltaTime;
        //if (goVentCountdown <= 0f)
       // {
        //    goVent = true;
       // }
    }
    private void SearchWalkPoint()
    {
        // testing random
        if (NetInfo.Instance != null) Random.InitState(NetInfo.Instance.GetSeed());

        float randomZ = Random.Range(-wpRange, wpRange);
        float randomX = Random.Range(-wpRange, wpRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        //if (Physics.Raycast(walkPoint, -transform.up, 0f, isGround))
            walkPointSet = true;
    }

    private void Chase()
    {
        // goVentCountdown = 10.0f;
        if (CheckIfOnNavMesh())
            agent.SetDestination(player.position);
        else Debug.Log("Agent " + transform.name + " is not on a navmesh");
    }

    /*
    private void Vent()
    {
        FindClosestVent();
        agent.SetDestination(vent.position);
    }
    
    private void FindClosestVent()
    {
        float closestDis = Mathf.Infinity;
        for (int i = 0; i < vents.Length; i++)
        {
            float dis = Vector3.Distance(vents[i].transform.position, transform.position);
            if (dis < closestDis)
            {
                closestDis = dis;
                vent = vents[i];
            }
        }
    }
    */
    private void Attack()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!didAttack)
        {
            //put attacking code here
            


            //

            didAttack = true;
            Invoke(nameof(ResetAttack), attackCooldown);
        }
    }

    private void ResetAttack()
    {
        didAttack = false;
    }

    //see vision radius of enemies
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
       // Gizmos.color = Color.blue;
       // Gizmos.DrawWireSphere(transform.position, ventLocator);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
        Gizmos.DrawRay(transform.position, transform.forward * 4f);
    }

    NavMeshHit navHit;
    private bool CheckIfOnNavMesh() => NavMesh.SamplePosition(agent.transform.position, out navHit, 0.1f, NavMesh.AllAreas);
}
