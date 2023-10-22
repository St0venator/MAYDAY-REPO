using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crabBehavior : MonoBehaviour
{
    //Getting the different crab componesnts
    Rigidbody rb;
    [SerializeField] private GameObject rightClaw;
    [SerializeField] private GameObject leftClaw;
    [SerializeField] private GameObject weakSpot;

    //attributes of the crab
    [SerializeField] private float carbSpeed;
    [SerializeField] private float crabDistancing;
    [SerializeField] private float crabSightRange;
    [SerializeField] private float crabAttackRange;
    [SerializeField] private float stunTime;
    bool isCrabStunned = false;

    //Animation curve of the crabs attack
    [SerializeField] private AnimationCurve Jab;

    void Start()
    {
        
    }

    void Update()
    {
        
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
