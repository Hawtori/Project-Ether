using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask isGround, isPlayer;

    //Idle Walking
    public Vector3 walkPoint;
    bool walkPointSet;
    public float wpRange;

    //Attacking
    public float attackCooldown;
    bool didAttack;

    //States
    public float visionRadius, attackRange;
    public bool playerVisible, playerCanAttacked;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        playerVisible = Physics.CheckSphere(transform.position, visionRadius, isPlayer);
        playerCanAttacked = Physics.CheckSphere(transform.position, attackRange, isPlayer);

        if (!playerVisible && !playerCanAttacked) Patrol();
        if (playerVisible && !playerCanAttacked) Chase();
        if (playerVisible && playerCanAttacked) Attack();
    }

    private void Patrol()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 disToWalkPoint = transform.position - walkPoint;

        if (disToWalkPoint.magnitude < 0.5f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-wpRange, wpRange);
        float randomX = Random.Range(-wpRange, wpRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        //if (Physics.Raycast(walkPoint, -transform.up, 0f, isGround))
            walkPointSet = true;
    }

    private void Chase()
    {
        agent.SetDestination(player.position);
    }

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
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRadius);
        Gizmos.DrawRay(transform.position, transform.forward * 4f);
    }
}
