using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class lossScroller : MonoBehaviour
{
    public float spd;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.up * spd * Time.deltaTime;

        if(transform.position.y >= 550)
        {
            transform.position = new Vector3(0, 0, 130);
        }
    }
}
