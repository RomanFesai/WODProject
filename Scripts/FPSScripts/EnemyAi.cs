
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    public Animator anim;

    public GameObject AttackHand;

    [SerializeField] private AudioSource EnemyAudioSource = default;
    [SerializeField] private AudioClip EnemyDetected = default;
    //public float health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    //public GameObject projectile;
    //public GameObject EnemyShootHole;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("ChaseNavPoint").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        //if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) 
        {
            agent.speed = 7;
            anim.SetBool("Chase", true);
            //FindObjectOfType<AudioManager>().Play("MonsterDetected");
            EnemyAudioSource.PlayOneShot(EnemyDetected);
            ChasePlayer();
        }
        else
        {
            agent.speed = 3;
            anim.SetBool("Chase", false);
        }
        if (playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
        }
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

     private void ChasePlayer()
     {
        transform.LookAt(player);
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        //agent.SetDestination(transform.position);
        AttackHand.GetComponent<BoxCollider>().enabled = true;

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            agent.isStopped = true;
            ///Attack code here
            anim.SetBool("Attack", true);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        AttackHand.GetComponent<BoxCollider>().enabled = false;
        anim.SetBool("Attack", false);
        alreadyAttacked = false;
        agent.isStopped = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
