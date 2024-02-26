using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] float yPos;
    [SerializeField] float zPos;
    [SerializeField] GameObject anchor;
    [SerializeField] GameObject whirlpool;
    // Start is called before the first frame update
    void Start()
    {
        spawnWhirlpool();
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
