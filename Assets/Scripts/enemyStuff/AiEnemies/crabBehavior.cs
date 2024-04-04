using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crabBehavior : MonoBehaviour
{
    //Getting the different crab componesnts
    //[SerializeField] private SphereCollider rightClaw;
    //[SerializeField] private SphereCollider leftClaw;
    //[SerializeField] private SphereCollider weakSpot;

    //attributes of the crab
    private float crabDistancing = 7f;
    private float crabSightRange = 25f;
    private float crabAttackRange = 9f;
    private float timer = 5f;
    private float bulletTime = 2f;
    public GameObject enemyBullet;
    public GameObject shootingPos;
    bool cantChase = false;
    float stunTimer = 5f;
    //bool isCrabStunned = false;
    public bool inAttackRange, inSightRange, inGround;

    //Animotor chit
    [SerializeField]private Animator crabAnim;

    //patrol variables
    [SerializeField]private Vector3 swimP; 
    [SerializeField]private Vector3 swimP1;

    //Reference to other objects
    //[SerializeField] private MeshCollider wall;
    [SerializeField] private LayerMask player;
    [SerializeField] private playerController playerObj;
    //[SerializeField] private GameObject aStar;
    [SerializeField]private Pathfinding pathFinder;
    
    //Animation curve of the crabs attack
    //[SerializeField] private AnimationCurve Jab;

    void Awake(){
        player = LayerMask.GetMask("player");
        crabAnim = GetComponent<Animator>();
        pathFinder = GameObject.Find("A_").GetComponent<Pathfinding>();
        GetComponent<Unit>().enabled = false;
        pathFinder.pausePath = true;
        playerObj = FindObjectOfType<playerController>();
    }

    void Start()
    {

    }

    void Update()
    {
        inSightRange = Physics.CheckSphere(transform.position, crabSightRange, player);
        inAttackRange = Physics.CheckSphere(transform.position, crabAttackRange, player);
        //inGround = Physics.CheckSphere(transform.position, crabDistancing, wall);

        if(cantChase)
    { 
        Waiting();
    }
    else
    {
        if(!inSightRange && !inAttackRange) CrabPatrol();

        if(inSightRange && !inAttackRange) CrabChase();

        if(inSightRange && inAttackRange) CrabAttack();
    }
    
}

void Waiting()
{
    if (stunTimer <= 0)
    {
        GetComponent<Unit>().enabled = true;
        cantChase = false;
        stunTimer = 5.0f; // Reset the stun timer to its original value if you want the stun to be repeatable
    }
    else
    {
        stunTimer -= Time.deltaTime;
    }
    }

    //help visuallize the different atributes of the crab
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, crabSightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, crabAttackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, crabDistancing);
    }

    void CrabPatrol()
    {
        StartCoroutine("StartWalking");
        GetComponent<Unit>().enabled = false;
        pathFinder.pausePath = true;
    }

    void CrabChase()
    {
        if(cantChase)
        {
            GetComponent<Unit>().enabled = false;
        }
        else
        {
            StopAllCoroutines();
            pathFinder.pausePath = false;
            GetComponent<Unit>().enabled = true;   
        }
    }

    void CrabAttack()
    {
        crabAnim.SetBool("Attack", true);
        GetComponent<Unit>().enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            cantChase = true;
            playerObj.isStunned = true;
            GetComponent<Unit>().enabled = false;
        }
    }

    IEnumerator StartWalking()
    {
        var swimP = transform.position;
        var swimP1X = Random.Range(swimP.x - 10, swimP.x + 10);
        var swimP1Y = Random.Range(swimP.y - 10, swimP.y + 10);
        swimP1 = new Vector3(swimP1X, swimP1Y, -31f);

        yield return StartCoroutine(MoveObject(transform, swimP, swimP1, 3.0f));
        yield return StartCoroutine(MoveObject(transform, swimP1, swimP, 3.0f));
        while(true)
        {
            yield return StartCoroutine(MoveObject(transform, swimP, swimP1, 3.0f));
            yield return StartCoroutine(MoveObject(transform, swimP1, swimP, 3.0f));
        } 
    }

    IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
    {
        var i= 0.0f;
        var rate= 1.0f/time;
        while(i < 1.0f)
        {
            i += Time.deltaTime * rate;
            thisTransform.position = Vector3.Lerp(startPos, endPos, i);
            yield return null;
        }
    }
}

/*
    How crab need to work

    4 states:
        partol/walking
            ~Need to be connected or on the wall
            ~Moves by finding ramdom points on the wall and going to them
            ~Checks to see if the player is in its radius of view
        /DONE chasing
            ~Player is in Sight range
            ~Faces the player
            ~Movest towards the player
            ~Checks to see if player is in the Attack radius
        attacking
            ~Player is in attack range
            ~stays slightly way from player with crabDistancing
            ~winds-up a claw
            ~jabs at players last loaction in the radius
            ~If the platyer is hit, the player is moved back
            ~Checks to see if player is still in the attack radius

        Maybe stunned
            ~Crab got "stunned" but player
            ~Crab turns around
            ~waits a few second
            ~If hit crab nio longer stunned
            ~If stun runs out, crab no longer stunned
            ~checks to see if player is in sight radius
            ~checks to see if player is in attack radius
*/
