using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public playerController player;
    void Start()
    {
        player = FindObjectOfType<playerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.isStunned = true;
            Destroy(gameObject);
        }
    }
}
