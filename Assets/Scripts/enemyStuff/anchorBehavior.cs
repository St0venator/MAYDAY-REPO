using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class anchorBehavior : MonoBehaviour
{
    GameObject player;
    playerController PC;
    public LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        PC = player.GetComponent<playerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void movePlayer()
    {
        Vector3 currPos = player.transform.position;

        Vector3 newPos = currPos;

        newPos -= new Vector3(0, 20, 10);

        Ray zRay = new Ray(newPos, new Vector3(0, 0, 1));

        if (Physics.Raycast(zRay, out RaycastHit zHit, float.MaxValue, mask))
        {
            Debug.Log("found a point");
            newPos = new Vector3(newPos.x, newPos.y, zHit.point.z);

            PC.StopAllCoroutines();
            StartCoroutine(PC.fall(newPos, 25));
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("doing it");
            movePlayer();
        }
    }
}
