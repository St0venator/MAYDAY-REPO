using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] float yPos;
    [SerializeField] float zPos;
    [SerializeField] GameObject anchor;
    [SerializeField] GameObject whirlpool;
    [SerializeField] GameObject mine;
    [SerializeField] GameObject avalanche;
    [SerializeField] LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        spawnMine(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnMine()
    {
        bool validSpawn = false;
        float xRange;
        float yRange;

        do
        {
            xRange = Random.Range(-100, 100);
            yRange = Random.Range(0, 200);

            Ray zRay = new Ray(new Vector3(xRange, yRange), new Vector3(0, 0, 1));

            if (Physics.Raycast(zRay, out RaycastHit zHit, float.MaxValue, mask))
            {
                validSpawn = true;
            }
        } while(!validSpawn);

        Vector3 spawnPos = new Vector3(xRange, yRange, zPos);

        Instantiate(mine, spawnPos, Quaternion.identity);

        
    }

    void spawnWhirlpool()
    {
        float spawnX = Random.Range(-100, 100);
        float spawnY = Random.Range(0, 100);

        Vector3 spawnPos = new Vector3(spawnX, spawnY, zPos);

        Instantiate(whirlpool, spawnPos, Quaternion.identity);
    }

    void spawnAnchor()
    {
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        Vector3 spawnPos = new Vector3(playerPos.x, yPos, zPos);

        GameObject newAnchor = Instantiate(anchor, spawnPos, Quaternion.identity);

        newAnchor.transform.Rotate(new Vector3(-90, 0, 0));
    }

    void spawnAvalanche()
    {
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        Vector3 spawnPos = new Vector3(playerPos.x, yPos, zPos);

        Instantiate(avalanche, spawnPos, Quaternion.identity);
    }
}
