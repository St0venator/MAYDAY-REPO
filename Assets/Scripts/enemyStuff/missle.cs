using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missle : MonoBehaviour
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
            //player.isStunned = true;
            Destroy(gameObject);
            Debug.Log("Be stunned");
        }
    }
}
