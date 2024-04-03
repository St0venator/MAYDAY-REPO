using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileSpawner : MonoBehaviour
{
    [SerializeField]private float spawnInterval;
    [SerializeField]private float spawnDelay = 10.0f;
    [SerializeField]private GameObject missilePrefab;

    void Start()
    {
        Invoke("SpawnMissile", spawnDelay);
    }

    void SpawnMissile()
    {
        spawnInterval = Random.Range(5f, 10f);
        Invoke("SpawnMissile", spawnInterval);
        Debug.Log("Missle Inbound: " + spawnInterval);

        Instantiate(missilePrefab, gameObject.transform);
    }
}
