using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineExplosions : MonoBehaviour
{
    [SerializeField]private ParticleSystem explosion;
    [SerializeField] private playerController player;
    public AudioSource source;

    void Start() {
        source.Play();
    }

    /*void Update()
    {
        if(player.isExploded)
        {
            explosion.Play();
            player.isExploded = false;
            explosion.Stop();
        }
    }*/

    /*
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Bo0om");
            explosion.Play();
        }
    }
    */
}
