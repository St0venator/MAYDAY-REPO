using System.Collections;
using System.Collections.Generic;
//using UnityEditorInternal;
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

    [SerializeField] Image dangerUI;
    // Start is called before the first frame update
    void Start()
    {
        spawnStationary();
        StartCoroutine(spawnRandom());
        dangerUI.gameObject.SetActive(false); // Turn of indicator
        spawnMine();
    }

    // Update is called once per frame
    void Update()
    {
        moveDangerIcon();

    }

    IEnumerator spawnRandom()
    {
        while(true) 
        {
            int randOdds = Random.Range(0, 101);

            if(randOdds >= 95)
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
                //spawnMine(); //turn off mine spawn
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
        float zPos = 0;
        do
        {
            xRange = Random.Range(-100, 100);
            yRange = Random.Range(0, 200);
            
            //Ray zRay = new Ray(new Vector3(xRange, yRange), new Vector3(0, 0, 1));

            Collider[] hitColliders1 = Physics.OverlapSphere(new Vector3(xRange, yRange, -30), 1f, mask);

            if(hitColliders1.Length > 0)
            {
                validSpawn = true;
            }

        } while(!validSpawn);

        Vector3 spawnPos = new Vector3(xRange, yRange, -30);

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

        StartCoroutine(spawnDangerIcon());
        

        GameObject newAnchor = Instantiate(anchor, spawnPos, Quaternion.identity);

        newAnchor.transform.Rotate(new Vector3(-90, 0, 0));
    }

    void spawnAvalanche()
    {
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        Vector3 spawnPos = new Vector3(playerPos.x, yPos, zPos);

        StartCoroutine(spawnDangerIcon());

        Instantiate(avalanche, spawnPos, Quaternion.identity);
    }

    IEnumerator spawnDangerIcon()
    {
        Debug.Log("called");
        dangerUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.10f); //Flash 1
        dangerUI.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.10f);
        dangerUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.10f); //Flash 2
        dangerUI.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.10f);
        dangerUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.10f); // Flash 3
        dangerUI.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.10f);
        dangerUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.10f); // Flash 4
        dangerUI.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.10f);
        dangerUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.10f); // Flash 5
        dangerUI.gameObject.SetActive(false);

        StopCoroutine(spawnDangerIcon());
    }

    void moveDangerIcon()
    {
        Vector3 UIPos = GameObject.FindGameObjectWithTag("UIPos").transform.position;
        dangerUI.transform.position = new Vector3(UIPos.x, UIPos.y, UIPos.z);
    }


  

}
