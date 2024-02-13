using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rockController : MonoBehaviour
{
    public LayerMask mask;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = Random.Range(0f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        Ray zRay = new Ray(new Vector3(transform.position.x, transform.position.y, transform.position.z), new Vector3(0, 0, 1));

        if (Physics.Raycast(zRay, out RaycastHit zHit, float.MaxValue, mask))
        {
            
            transform.position = new Vector3(transform.position.x, transform.position.y, zHit.point.z - 5);
        }

        transform.Rotate(transform.right, -1);
    }

    private void FixedUpdate()
    {
        
    }
}
