using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System;

public class Walking : NetworkBehaviour
{


    public GameObject player;
    private Animator brocolliAnimator;
    private GameObject brocolliMonster;

    public GameObject healthbar;
    public float healthpoints = 100;
    private float totalhealthpoints;
    private float healthunit;
    private float fireballdmg = 40;
    private float flamethrowerdamage = 15;


    public Rigidbody[] bodyparts;
    public Collider[] cols;
    public int parts;

    private Rigidbody rb;
    private Collider coli;



    private bool attack;
    public bool isDead = false;

    public float area = 100;
    private Vector2 newWayPoint;
    private Vector3 wayPoint;
    private Vector3 oldWayPoint;


	private UnityEngine.AI.NavMeshAgent nav;                               // Reference to the nav mesh agent.
	public float patrolSpeed = 10f;                          // The nav mesh agent's speed when patrolling.
	public float patrolWaitTime = 1f;                       // The amount of time to wait when the patrol way point is reached.
	public Transform[] patrolWayPoints;                     // An array of transforms for the patrol route.

	private float patrolTimer;                              // A timer for the patrolWaitTime.
	private int wayPointIndex;                              // A counter for the way point array.


    // Use this for initialization
    void Start()
    {
		nav = GetComponent<UnityEngine.AI.NavMeshAgent>();

        totalhealthpoints = healthpoints;
        brocolliMonster = GetComponent<GameObject>();
        brocolliAnimator = GetComponent<Animator>();

        //store base health unit
        healthunit = healthbar.transform.localScale.x;

        rb = GetComponent<Rigidbody>();
        coli = GetComponent<Collider>();

        attack = true;

        bodyparts = GetComponentsInChildren<Rigidbody>();
        cols = GetComponentsInChildren<Collider>();
        int counter = 0;
        foreach(Rigidbody rig in bodyparts)
        {
            counter++;
            if (rig.gameObject.layer == 9)
            {
                rig.isKinematic = true;
            }
        }
        foreach(Collider col in cols)
        {
            if (col.gameObject.layer == 9)
            {
               col.isTrigger = true;
            }
        }

        //oldWayPoint = this.transform.position;
        makeWaypoint();
    }
    

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            if (!isDead)
            {

                transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);

                if (Vector3.Distance(this.transform.position, player.transform.position) < (1.0f))
                {
                    attacking();
                }
                else
                {
                    if (!brocolliAnimator.GetBool("closePunch") && !brocolliAnimator.GetBool("gethit"))
						Patrolling();
                }
            }

        }
        else if (GameObject.Find("PlayerTank(Clone)") != null)
        {
            player = GameObject.Find("PlayerTank(Clone)");
        }
    }

  
    void die()
    {


        isDead = true;
        healthbar.GetComponent<MeshRenderer>().enabled = false;
        brocolliAnimator.Stop();

        foreach (Rigidbody rig in bodyparts)
        {
            if (rig.gameObject.layer == 9)
            {
                rig.isKinematic = false;
                rig.mass = 0;
                //Destroy(rig);

            }
        }
        foreach (Collider col in cols)
        {
            if (col.gameObject.layer == 9)
            {
                col.isTrigger = false;
            }
            //col.enabled = false;
        }
        Destroy(rb);
        Destroy(gameObject, 2.0f);

        //delete();

        //Destroy(coli);

        //brocolliMonster.GetComponent<BoxCollider>().enabled = true;

        //Destroy(transform.root.gameObject, 10.0f);
        //transform.tag = "Pickupable";
    }
    

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            Invoke("die", 0);
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), col.gameObject.GetComponent<Collider>());

            //die();
        }
    }


    void attacking()
    {
        brocolliAnimator.SetBool("closePunch", true);
        Invoke("stopPunch", 1.4f);
    }

    void stopPunch()
    {
        brocolliAnimator.SetBool("closePunch", false);
    }

    void takehit()
    {
        brocolliAnimator.SetBool("gethit", true);
        Invoke("stopHit", 0.35f);
    }


    void stopHit()
    {
        brocolliAnimator.SetBool("gethit", false);
    }



    void walking()
    {
        //random walking


        if (Vector3.Distance(this.transform.position, oldWayPoint) == 0.0f)
        {
            makeWaypoint();
        }
        this.transform.LookAt(wayPoint);
        this.transform.position = Vector3.MoveTowards(transform.position, wayPoint, Time.deltaTime * (2.5f));
        brocolliAnimator.SetFloat("xBlend", 2.0f, 0.1f, Time.deltaTime);

    }

	void Patrolling ()
	{
		// Set an appropriate speed for the NavMeshAgent.
		nav.speed = patrolSpeed;
		brocolliAnimator.SetFloat("xBlend", 2.0f, 0.1f, Time.deltaTime);

		// If near the next waypoint or there is no destination...
		if(nav.remainingDistance < nav.stoppingDistance)
		{
			// ... increment the timer.
			patrolTimer += Time.deltaTime;


			// If the timer exceeds the wait time...
			if(patrolTimer >= patrolWaitTime)
			{
				// ... increment the wayPointIndex.
				if(wayPointIndex == patrolWayPoints.Length - 1)
					wayPointIndex = 0;
				else
					wayPointIndex++;

				// Reset the timer.
				patrolTimer = 0;
			}
		}
		else
			// If not near a destination, reset the timer.
			patrolTimer = 0;

		// Set the destination to the patrolWayPoint.
		nav.destination = patrolWayPoints[wayPointIndex].position;
	}

    void makeWaypoint()
    {
        newWayPoint = UnityEngine.Random.insideUnitCircle * area;
        wayPoint = new Vector3(newWayPoint.x, gameObject.transform.position.y, newWayPoint.y);
        oldWayPoint = wayPoint;
    }



}


/*
using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
	public float patrolSpeed = 2f;                          // The nav mesh agent's speed when patrolling.
	public float chaseSpeed = 5f;                           // The nav mesh agent's speed when chasing.
	public float chaseWaitTime = 5f;                        // The amount of time to wait when the last sighting is reached.
	public float patrolWaitTime = 1f;                       // The amount of time to wait when the patrol way point is reached.
	public Transform[] patrolWayPoints;                     // An array of transforms for the patrol route.


	private EnemySight enemySight;                          // Reference to the EnemySight script.
	private NavMeshAgent nav;                               // Reference to the nav mesh agent.
	private Transform player;                               // Reference to the player's transform.
	private PlayerHealth playerHealth;                      // Reference to the PlayerHealth script.
	private LastPlayerSighting lastPlayerSighting;          // Reference to the last global sighting of the player.
	private float chaseTimer;                               // A timer for the chaseWaitTime.
	private float patrolTimer;                              // A timer for the patrolWaitTime.
	private int wayPointIndex;                              // A counter for the way point array.


	void Awake ()
	{
		// Setting up the references.
		enemySight = GetComponent<EnemySight>();
		nav = GetComponent<NavMeshAgent>();
		player = GameObject.FindGameObjectWithTag(Tags.player).transform;
		playerHealth = player.GetComponent<PlayerHealth>();
		lastPlayerSighting = GameObject.FindGameObjectWithTag(Tags.gameController).GetComponent<LastPlayerSighting>();
	}


	void Update ()
	{
		// If the player is in sight and is alive...
		if(enemySight.playerInSight && playerHealth.health > 0f)
			// ... shoot.
			Shooting();

		// If the player has been sighted and isn't dead...
		else if(enemySight.personalLastSighting != lastPlayerSighting.resetPosition && playerHealth.health > 0f)
			// ... chase.
			Chasing();

		// Otherwise...
		else
			// ... patrol.
			Patrolling();
	}


	void Shooting ()
	{
		// Stop the enemy where it is.
		nav.Stop();
	}


	void Chasing ()
	{
		// Create a vector from the enemy to the last sighting of the player.
		Vector3 sightingDeltaPos = enemySight.personalLastSighting - transform.position;

		// If the the last personal sighting of the player is not close...
		if(sightingDeltaPos.sqrMagnitude > 4f)
			// ... set the destination for the NavMeshAgent to the last personal sighting of the player.
			nav.destination = enemySight.personalLastSighting;

		// Set the appropriate speed for the NavMeshAgent.
		nav.speed = chaseSpeed;

		// If near the last personal sighting...
		if(nav.remainingDistance < nav.stoppingDistance)
		{
			// ... increment the timer.
			chaseTimer += Time.deltaTime;

			// If the timer exceeds the wait time...
			if(chaseTimer >= chaseWaitTime)
			{
				// ... reset last global sighting, the last personal sighting and the timer.
				lastPlayerSighting.position = lastPlayerSighting.resetPosition;
				enemySight.personalLastSighting = lastPlayerSighting.resetPosition;
				chaseTimer = 0f;
			}
		}
		else
			// If not near the last sighting personal sighting of the player, reset the timer.
			chaseTimer = 0f;
	}


	void Patrolling ()
	{
		// Set an appropriate speed for the NavMeshAgent.
		nav.speed = patrolSpeed;

		// If near the next waypoint or there is no destination...
		if(nav.destination == lastPlayerSighting.resetPosition || nav.remainingDistance < nav.stoppingDistance)
		{
			// ... increment the timer.
			patrolTimer += Time.deltaTime;

			// If the timer exceeds the wait time...
			if(patrolTimer >= patrolWaitTime)
			{
				// ... increment the wayPointIndex.
				if(wayPointIndex == patrolWayPoints.Length - 1)
					wayPointIndex = 0;
				else
					wayPointIndex++;

				// Reset the timer.
				patrolTimer = 0;
			}
		}
		else
			// If not near a destination, reset the timer.
			patrolTimer = 0;

		// Set the destination to the patrolWayPoint.
		nav.destination = patrolWayPoints[wayPointIndex].position;
	}
}
*/


