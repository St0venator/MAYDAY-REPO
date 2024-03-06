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
        spawnStationary();
        StartCoroutine(spawnRandom());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator spawnRandom()
    {
        while(true) 
        {
            int randOdds = Random.Range(0, 101);

            if(randOdds >= 90)
            {
                int randChoice = Random.Range(0, 2);

                if(randChoice == 0)
                {
                    spawnAnchor();
                }
                else if(randChoice == 1)
                {
                    spawnAvalanche();
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void spawnStationary()
    {
        int spawnNum = Random.Range(1, 4);

        for(int i = 0; i < spawnNum; i++)
        {
            int randObj = Random.Range(0, 2);

            if(randObj == 0)
            {
                spawnMine();
            }
            else if(randObj == 1)
            {
                spawnWhirlpool();
            }
        }
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