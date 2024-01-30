using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crabBehavior : MonoBehaviour
{
    //Getting the different crab componesnts
    [SerializeField] private SphereCollider rightClaw;
    [SerializeField] private SphereCollider leftClaw;
    [SerializeField] private SphereCollider weakSpot;

    //attributes of the crab
    [SerializeField] private float carbSpeed;
    [SerializeField] private float crabDistancing;
    [SerializeField] private float crabSightRange;
    [SerializeField] private float crabAttackRange;
    [SerializeField] private float stunTime;
    [SerializeField] private float timer = 5;
    [SerializeField] private float bulletTime;
    public GameObject enemyBullet;
    public GameObject shootingPos;
    //bool alreadyAttacked = false;
    bool isCrabStunned = false;
    [SerializeField] private bool inAttackRange, inSightRange, inGround;


    //Reference to other objects
    [SerializeField] private MeshCollider wall;
    [SerializeField] private LayerMask player;
    //[SerializeField] private GameObject aStar;
    [SerializeField]private Pathfinding pathFinder;
    
    //Animation curve of the crabs attack
    [SerializeField] private AnimationCurve Jab;

    void Start()
    {
        GetComponent<Unit>().enabled = false;
        pathFinder.pausePath = false;
    }

    void Update()
    {
        inSightRange = Physics.CheckSphere(transform.position, crabSightRange, player);
        inAttackRange = Physics.CheckSphere(transform.position, crabAttackRange, player);
        //inGround = Physics.CheckSphere(transform.position, crabDistancing, wall);

        if(!inSightRange && !inAttackRange) CrabPatrol();

        if(inSightRange && !inAttackRange) CrabChase();

        if(inSightRange && inAttackRange) CrabAttack();

        if(inGround)
        {

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
        GetComponent<Unit>().enabled = false;
        pathFinder.pausePath = false;
    }

    void CrabChase()
    {
        GetComponent<Unit>().enabled = true;   
        pathFinder.pausePath = true;
    }

    void CrabAttack()
    {
        bulletTime -= Time.deltaTime;
        if(bulletTime > 0) return;

        bulletTime = timer;

        Rigidbody bulletRb = Instantiate(enemyBullet, shootingPos.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        bulletRb.AddForce(transform.forward * 50f, ForceMode.Impulse);
    }

    void CrabStunned()
    {

    }
}

/*
    How crab need to work

    4 states:
        partol/walking
            ~Need to be connected or on the wall
            ~Moves by finding ramdom points on the wall and going to them
            ~Checks to see if the player is in its radius of view
        chasing
            ~Player is in Sight range
            ~Faces the player
            ~Movest towards the player
            ~Checks to see if player is in the Attack radius
        attacking
            ~Player is in attack range
            ~stays slightly way from player with crabDistancing
            ~winds-up a claw
            ~jabs at players last loaction in the radius
            ~Checks to see if player is still in the attack radius
        stunned
            ~Crab got "stunned" but player
            ~Crab turns around
            ~waits a few second
            ~If hit crab nio longer stunned
            ~If stun runs out, crab no longer stunned
            ~checks to see if player is in sight radius
            ~checks to see if player is in attack radius
*/
