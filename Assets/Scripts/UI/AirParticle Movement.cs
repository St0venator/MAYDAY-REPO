using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirParticleMovement : MonoBehaviour
{
    
    public GameObject Anchor;
    public GameObject boulder;
    public GameObject Mine;

    

    void Start()
    {
        InvokeRepeating("PickRandom",1,5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PickRandom()
    {
        int rand = Random.Range(0, 3);
        if (rand == 0)
        {
            Instantiate(Anchor,gameObject.transform);
        }
        else if (rand == 1)
        {
            Instantiate(Mine, gameObject.transform);
        }
        else if (rand == 2)
        {
            Instantiate(boulder, gameObject.transform);
        }
    }

}
