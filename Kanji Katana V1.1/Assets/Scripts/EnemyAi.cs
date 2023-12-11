using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.Image;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    //patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange = 50;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        walkPointSet = false;

    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }
    private void Patroling()
    {
        //Debug.Log("attack: " + playerInAttackRange + ", sight " + playerInSightRange);
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
   
        }
            
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Debug.Log("Distance to walkpoint: "+ distanceToWalkPoint.magnitude);
        if (distanceToWalkPoint.magnitude < 50f)
        {
            walkPointSet = false;
            //Debug.Log("Walk point set: " + walkPointSet);
        }
           

    }
    private void SearchWalkPoint()
    {
        //float randomZ = Random.Range(-walkPointRange, walkPointRange);
        //float randomX = Random.Range(-walkPointRange, walkPointRange);

        //walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        //if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        //    walkPointSet = true;


        // Choose a random direction in the horizontal plane
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        Vector3 randomDirection = new Vector3(randomX, 0, randomZ).normalized;

        // Find a random point around the AI
        Vector3 randomPoint = transform.position + randomDirection * walkPointRange;

        // Translate this point upwards
        //float heightAboveGround = 10f; // Adjust this height as needed
        //randomPoint.y += heightAboveGround;

        RaycastHit hit;

        // Cast a ray downwards from the random point
        if (Physics.Raycast(randomPoint, Vector3.down, out hit, walkPointRange, whatIsGround))
        {
            walkPoint = hit.point;
            walkPointSet = true;
        }
        else
        {
            // Handle case where raycast doesn't hit anything suitable
            walkPointSet = false;
        }

        

    }
    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.Euler(0,-90,0)).GetComponent<Rigidbody>();

            //rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 2f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
