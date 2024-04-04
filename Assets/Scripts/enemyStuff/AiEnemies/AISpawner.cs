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
        int spawnNum = Random.Range(5, 10);

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

            Ray zRay = new Ray(new Vector3(xRange, yRange), new Vector3(0, 0, 1));

            if (Physics.Raycast(zRay, out RaycastHit zHit, float.MaxValue, mask))
            {
                validSpawn = true;
            }
        } while(!validSpawn);

        Vector3 spawnPos = new Vector3(xRange, yRange, -31);

        Instantiate(crabby, spawnPos, Quaternion.identity);
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

            Ray zRay = new Ray(new Vector3(xRange, yRange), new Vector3(0, 0, 1));

            if (Physics.Raycast(zRay, out RaycastHit zHit, float.MaxValue, mask))
            {
                validSpawn = true;
            }
        } while(!validSpawn);

        Vector3 spawnPos = new Vector3(xRange, yRange, -31);

        GameObject newInky = Instantiate(inky, spawnPos, Quaternion.identity);
        newInky.transform.Rotate(new Vector3(270, 0, 0));
    }
}
