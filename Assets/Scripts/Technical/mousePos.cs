using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class mousePos : MonoBehaviour
{
    Camera cam;
    float zComponent = 0;
    public LayerMask mask;
    public float range;
    public Vector3 jumpPos2;
    public Vector3 jumpPos3;
    GameObject playerRef;
    Vector3 playerPos;
    Vector2 localHitPoint;
    Vector3 temp;
    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        playerPos = playerRef.transform.position;
        cam = Camera.main;
        //localHitPoint = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

        //transform.position = cam.transform.position;

        playerPos = playerRef.transform.position;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        Debug.DrawRay(cam.gameObject.transform.position, ray.direction);

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, mask))
        {
            
            localHitPoint = hit.point;
            transform.position = new Vector3(hit.point.x, hit.point.y, -30);
            //Debug.Log(localHitPoint.ToString());

            Ray zRay = new Ray(new Vector3(localHitPoint.x, localHitPoint.y), new Vector3(0, 0, 1));
            

            if (Physics.Raycast(zRay, out RaycastHit zHit, float.MaxValue, mask))
            {
                temp = new Vector3(localHitPoint.x, localHitPoint.y, zHit.point.z);
            }   
        }
        

        //transform.position = temp;

        Vector3 diff = transform.position - playerPos;

        jumpPos2 = transform.position + diff;
        jumpPos3 = transform.position + (2f * diff);

        /*
        Ray twoRay = new Ray(new Vector3(twoPos.x, twoPos.y), new Vector3(0, 0, 1));
        Ray threeRay = new Ray(new Vector3(threePos.x, threePos.y), new Vector3(0, 0, 1));
        

        if (Physics.Raycast(twoRay, out RaycastHit twoHit, float.MaxValue, mask))
        {
            jumpPos2 = new Vector3(twoPos.x, twoPos.y, twoHit.point.z);
        }
        else
        {
            jumpPos2 = Vector3.zero;
        }

        if (Physics.Raycast(threeRay, out RaycastHit threeHit, float.MaxValue, mask))
        {
            jumpPos3 = new Vector3(threePos.x, threePos.y, threeHit.point.z);
        }
        else
        {
            jumpPos3 = Vector3.zero;
        }
        */
    }

    Vector3 clampPos(Vector3 startPos, int r)
    {
        Vector2 newPlayerPos = (Vector2)playerPos;
        Vector2 newMousePos = (Vector2)startPos;

        Vector2 newPos = (newMousePos - newPlayerPos);

        newPos = newPos.normalized * r;

        Vector3 pos = new Vector3(newPos.x, newPos.y, startPos.z);

        Debug.DrawRay(newPlayerPos, pos);

        return pos;
    }
}
