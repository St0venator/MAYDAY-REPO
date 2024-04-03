using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missle : MonoBehaviour
{
    public playerController player;
    public Pathfinding pathFinding;
    [SerializeField]private float missileTimer  = 30.0f;

    void Start()
    {
        pathFinding = FindObjectOfType<Pathfinding>();
        player = FindObjectOfType<playerController>();
        pathFinding.pausePath = true;
    }

    void Update()
    {
        missileTimer -= Time.deltaTime;

        if(missileTimer <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //player.isStunned = true;
            Destroy(gameObject);
            Debug.Log("Be stunned");
        }
    }
}
