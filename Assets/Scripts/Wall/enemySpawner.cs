using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class enemySpawner : MonoBehaviour
{
    [SerializeField] float yPos;
    [SerializeField] float zPos;
    [SerializeField] GameObject anchor;
    // Start is called before the first frame update
    void Start()
    {
        spawnAnchor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnAnchor()
    {
        Vector3 playerPos = GameObject.FindGameObjectWithTag("Player").transform.position;

        Vector3 spawnPos = new Vector3(playerPos.x, yPos, zPos);

        GameObject newAnchor = Instantiate(anchor, spawnPos, Quaternion.identity);

        newAnchor.transform.Rotate(new Vector3(-90, 0, 0));
    }
}
