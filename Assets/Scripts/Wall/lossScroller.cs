using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class lossScroller : MonoBehaviour
{
    public float spd;
    public Vector3 direction = Vector3.up;
    public Vector3 naturalPos = new Vector3(0, 0, 130);
    public float yTarget = 550;
    // Start is called before the first frame update
    void Start()
    {
        direction = direction.normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * spd * Time.deltaTime;

        if(transform.position.y - yTarget <= 0.5f)
        {
            transform.position = naturalPos;
        }
    }
}
