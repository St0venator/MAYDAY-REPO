using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    // Getting a referance to the sound manager
    [SerializeField]public SoundManager SoundManager;

    //The animation curves the player follows when they jump from one node to another
    [SerializeField] private AnimationCurve climbCurve;
    [SerializeField] private AnimationCurve fallCurve;

    //The speed the player jumps up to another node
    [SerializeField] private float climbSpeed;
    bool canJump = true;

    //The height a node needs to be above the player to be considered valid
    public float upLimit;

    //Bool controlling when the player is using the sword or jumping at all
    bool isSlash = false;
    bool isJump = false;

    //References
    Rigidbody rb;
    [SerializeField] GameObject worldCursor;
    [SerializeField] GameObject childObj;
    [SerializeField] GameObject bulletRef;
    public LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        climbSpeed /= 100f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Debug.Log("doing the input");
            isSlash = true;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl) && canJump)
        {
            GameObject bullet = Instantiate(bulletRef, transform.position, Quaternion.identity);
            bullet.transform.position -= new Vector3(0, 0, 10);
            bullet.GetComponent<bulletController>().velocity = (worldCursor.transform.position - transform.position).normalized * 10;
            SoundManager.PlayShootSFX();
        }

        //If the player hits Left Shift, stop all current climb coroutines, and start a new one targeting the node the cursor is selecting
        if (Input.GetMouseButtonDown(1) && canJump) 
        {
            StopAllCoroutines();
            if(isSlash){
                    SoundManager.PlaySwordSFX();//Plays sword SFX when moving
            }
            else{
                SoundManager.PlayJumpSound();//Plays jump SFX when moving
            }
            //If the current node is above the player, climbing
            if (worldCursor.transform.position.y >= transform.position.y)
            {
                
                StartCoroutine(climb(worldCursor.transform.position, climbSpeed));
            }
            //Otherwise, fall
            else
            {
                StartCoroutine(fall(worldCursor.transform.position, climbSpeed));
            }
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Anchor"))
        {
            Vector3 currPos = transform.position;

            Vector3 newPos = currPos;

            newPos -= new Vector3(0, 30);

            

            Ray zRay = new Ray(newPos, new Vector3(0, 0, 1));

            if (Physics.Raycast(zRay, out RaycastHit zHit, float.MaxValue, mask))
            {
                newPos.z = zHit.point.z;
                Debug.Log(currPos.x + " " + currPos.y + " " + currPos.z);
                Debug.Log(newPos.x + " " + newPos.y + " " + newPos.z);

                StopAllCoroutines();
                StartCoroutine(fall(newPos, climbSpeed * 1.5f, false));
            }

            Destroy(other.gameObject);
        }
    }

    public IEnumerator climb(Vector3 dest, float speed)
    {
        /*
         * dest  - The transform.position of the node we're climbing to
         * speed - The amount we move along the x-axis of our animation curve each loop of the coroutine
         * 
         * climb: Moves the player from one node to another by LERPing, sampling an animation curve
        */

        //Resetting all of our variables, starting the animation curve at 0, and our starting position as our current position
        float pos = 0.0f;
        Vector3 startPos = transform.position;
        canJump = false;

        //Keeping the player in front of the wall, from the camera's POV by moving the player slightly closer to the camera
        dest = Vector3.MoveTowards(dest, Camera.main.transform.position, 0.1f);

        Vector3 destDist = dest - startPos;

        GetComponent<oxygenController>().reduceOxygen(Mathf.Abs(destDist.magnitude));

        if(destDist.magnitude > 10)
        {
            destDist = destDist.normalized * 10;

            dest = startPos + destDist;

            Physics.Raycast(new Ray(new Vector3(dest.x, dest.y), new Vector3(0, 0, 1)), out RaycastHit hit, float.MaxValue);

            dest.z = hit.point.z;
        }

        //Calculating the midpoint between the current and target nodes, and them moving it away from the camera
        Vector3 midPoint = (dest + startPos) / 2;
        midPoint -= Vector3.MoveTowards(midPoint, Camera.main.transform.position, 3.14f) - midPoint;

        //Getting our modified coordinates for our SLERP
        Vector3 newStart = startPos - midPoint;
        Vector3 newDest = dest - midPoint;

        bool innerIsSlash = isSlash;
        Debug.Log(innerIsSlash);

        if (innerIsSlash)
        {
            childObj.SetActive(true);
        }
        //LERPing towards "dest" until we reach the end of our animation curve
        while(pos < 1f)
        {

            //Moving n% of the way between our start and end positions, with n being the y-value of our animation curve at x = "pos"
            transform.position = Vector3.Slerp(newStart, newDest, climbCurve.Evaluate(pos)) + midPoint;
            pos = Mathf.MoveTowards(pos, 1.0f, speed * Time.deltaTime * 3);

            if(pos > 0.2f && innerIsSlash)
            {
                childObj.SetActive(false);
                innerIsSlash = false;
                isSlash = false;
            }

            if (pos > 0.4f && !isSlash)
            {
                canJump = true;
                
            }
            else if(pos > 0.6f)
            {
                canJump = true;
            }

            yield return null;
        }
        childObj.SetActive(false);
        canJump = true;
        
    }

    public IEnumerator fall(Vector3 dest, float speed = 25, bool isPlayerJump = true)
    {
        Debug.Log("in the fall");
        /*
         * dest  - The transform.position of the node we're climbing to
         * speed - The amount we move along the x-axis of our animation curve each loop of the coroutine
         * 
         * fall: Moves the player from one node to another by LERPing, sampling an animation curve
        */

        //Resetting all of our variables, starting the animation curve at -0.1, and our starting position as our current position
        float pos = 0.0f;
        Vector3 startPos = transform.position;
        canJump = false;

        //Keeping the player in front of the wall, from the camera's POV by moving the player slightly closer to the camera
        dest = Vector3.MoveTowards(dest, Camera.main.transform.position, 0.1f);

        Vector3 destDist = dest - startPos;

        
        if (destDist.magnitude > 10 && isPlayerJump)
        {
            destDist = destDist.normalized * 10;

            dest = startPos + destDist;

            Physics.Raycast(new Ray(new Vector3(dest.x, dest.y), new Vector3(0, 0, 1)), out RaycastHit hit, float.MaxValue);

            dest.z = hit.point.z;
        }

        //Calculating the midpoint between the current and target nodes, and them moving it away from the camera
        Vector3 midPoint = (dest + startPos) / 2;
        midPoint -= Vector3.MoveTowards(midPoint, Camera.main.transform.position, 3.14f) - midPoint;

        //Getting our modified coordinates for our SLERP
        Vector3 newStart = startPos - midPoint;
        Vector3 newDest = dest - midPoint;

        //childObj.SetActive(true);
        //LERPing towards "dest" until we reach x = 1.1 on our animation curve
        while (pos < 1)
        {
            Debug.Log("falling");
            //Moving n% of the way between our start and end positions, with n being the y-value of our animation curve at x = "pos"
            transform.position = Vector3.Slerp(newStart, newDest, fallCurve.Evaluate(pos)) + midPoint;
            pos = Mathf.MoveTowards(pos, 1.0f, speed * Time.deltaTime * 3);

            /*
            if (pos > 0.2f)
            {
                childObj.SetActive(false);
            }
            */

            if (pos > 0.4f)
            {
                Debug.Log("fallOver");
                canJump = true;
            }

            yield return null;
        }
        childObj.SetActive(false);
        canJump = true;
        //nodeManager.instance.updateNodes();
    }
}
