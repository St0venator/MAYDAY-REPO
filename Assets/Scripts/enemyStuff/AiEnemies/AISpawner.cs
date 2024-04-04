using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISpawner : MonoBehaviour
{
    [SerializeField]private GameObject crabby;
    [SerializeField]private GameObject inky;
    [SerializeField]private Grid grid;
    [SerializeField] LayerMask mask;

    void Start()
    {
        grid = FindObjectOfType<Grid>();
        SpawnLocation();
    }

    void Update()
    {
        
    }

    void SpawnLocation()
    {
        int spawnNum = Random.Range(4, 10);

        for(int i = 0; i < spawnNum; i++)
        {
            int randObj = Random.Range(0, 2);

            if(randObj == 0)
            {
                SpawnCrab();
            }
            else if(randObj == 1)
            {
                SpawnSquid();
            }
        }
    }

    void SpawnCrab()
    {
        bool validSpawn = false;
        float xRange;
        float yRange;
        float x = grid.gridSizeX/2 * -1;

        do
        {
            xRange = Random.Range(x, grid.gridSizeX/2);
            yRange = Random.Range(0, grid.gridSizeY);

            Collider[] hitColliders1 = Physics.OverlapSphere(new Vector3(xRange, yRange, -30), 1f, mask);


    

            if (hitColliders1.Length > 0)

            {
                validSpawn = true;
            }

        Vector3 spawnPos = new Vector3(xRange, yRange, -31);

        GameObject newCrabby = Instantiate(crabby, spawnPos, Quaternion.identity);
        newCrabby.transform.Rotate(new Vector3(0, 90, 0));
        }while(!validSpawn);
    }

    void SpawnSquid()
    {
        bool validSpawn = false;
        float xRange;
        float yRange;
        float x = grid.gridSizeX/2 * -1;

        do
        {
            xRange = Random.Range(x, grid.gridSizeX/2);
            yRange = Random.Range(0, grid.gridSizeY);

            Collider[] hitColliders1 = Physics.OverlapSphere(new Vector3(xRange, yRange, -30), 1f, mask);


            if(hitColliders1.Length > 0)

            {
                validSpawn = true;
            }

        Vector3 spawnPos = new Vector3(xRange, yRange, -31);

        GameObject newInky = Instantiate(inky, spawnPos, Quaternion.identity);
        newInky.transform.Rotate(new Vector3(90, 0, 0));
        }while(!validSpawn);
    }
}
