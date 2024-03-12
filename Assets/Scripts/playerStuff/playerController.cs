using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class playerController : MonoBehaviour
{
    //Particles
    public ParticleSystem jumpLand;
    public ParticleSystem explosion;
    public bool isExploded = false;

    // Getting a reference to the sound manager
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
    bool isSlowed = false;

    //References
    Rigidbody rb;
    Animator anim;
    [SerializeField] GameObject worldCursor;
    [SerializeField] GameObject childObj;
    [SerializeField] GameObject bulletRef;
    [SerializeField] TextMeshProUGUI oxygenText;
    [SerializeField] oxygenManager OXY;
    public LayerMask mask;
    public float walkSpeed = 1;
    public GameObject target;
    

    //variables for stunning player
    [HideInInspector] public bool isStunned;
    private float stunTimer = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        climbSpeed /= 100f;
    }

    // Update is called once per frame
    void Update()
    {
        if (isStunned)
        {
            if (stunTimer <= 0)
            {
                isStunned = false;
                stunTimer = 3.0f;

            }
            else
            {
                stunTimer -= Time.deltaTime;
                Debug.Log(stunTimer);
            }
            Debug.Log(isStunned);

        }

        OXY.oxygenDecrement(Time.deltaTime);
        //oxygenText.text = "Oxygen: " + OXY.displayOxygen();

        #region walking
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.position -= Vector3.right * Time.deltaTime * walkSpeed;
        }
        if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right * Time.deltaTime * walkSpeed;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            transform.position -= Vector3.up * Time.deltaTime * walkSpeed;
        }
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.up * Time.deltaTime * walkSpeed;
        }

        Ray zRay = new Ray(new Vector3(transform.position.x, transform.position.y), new Vector3(0, 0, 1));
        /*
        Ray zRay1 = new Ray(new Vector3(transform.position.x + 0.01f, transform.position.y), new Vector3(0, 0, 1));
        Ray zRay2 = new Ray(new Vector3(transform.position.x, transform.position.y + 0.01f), new Vector3(0, 0, 1));

        Vector3 zPos1;
        Vector3 zPos2;
        Vector3 zPos3;

        Physics.Raycast(zRay, out RaycastHit zHit0);
        Physics.Raycast(zRay1, out RaycastHit zHit1);
        Physics.Raycast(zRay2, out RaycastHit zHit2);

        zPos1 = zHit0.point;
        zPos2 = zHit1.point;
        zPos3 = zHit2.point;
        Plane plane = new Plane(zPos1, zPos2, zPos3);


        Vector3 dir1 = zPos1 - zPos2;
        Vector3 dir2 = zPos1 - zPos3;

        Vector3 cross = Vector3.Cross(dir1, dir2);

        transform.rotation = Quaternion.LookRotation(zHit0.normal, Vector3.up);
        */

        if (!isJump)
        {
            

            /*
            Transform newTrans = transform;
            newTrans.position = transform.position + cross.normalized;
            */

            

            if (Physics.Raycast(zRay, out RaycastHit zHit, float.MaxValue, mask))
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, zHit.point.z - .5f);
            }
        }
        

        #endregion

        if (OXY.oxygenAmnt < 0.5f)
        {
            MenuManager mngr = GameObject.Find("UIManager").GetComponent<MenuManager>();
            mngr.StartCoroutine("LoadAsynchronously", "LoseScene");
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            
            isSlash = true;
        }


            if (Input.GetKeyDown(KeyCode.LeftControl) && canJump)
            {
                GameObject bullet = Instantiate(bulletRef, transform.position, Quaternion.identity);
                bullet.transform.position -= new Vector3(0, 0, 10);
                bullet.GetComponent<bulletController>().velocity = (worldCursor.transform.position - transform.position).normalized * 10;
                SoundManager.PlayShootSFX();
            /*
                anim.SetBool("Jumped", true);
                anim.SetBool("IsMidair", true);
                anim.SetBool("IsGrounded", false);
                StartCoroutine(climb(new Vector3[] {worldCursor.transform.position }, climbSpeed));
            */
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
                    
                    StartCoroutine(climb(new Vector3[] { worldCursor.transform.position}, climbSpeed));
                }
                //Otherwise, fall
                else
                {
                    StartCoroutine(fall(worldCursor.transform.position, climbSpeed));
                }
                anim.SetBool("Jumped", true);
                anim.SetBool("IsMidair", true);
                anim.SetBool("IsGrounded", false);
                StartCoroutine(fall(worldCursor.transform.position, climbSpeed));
            }

        if(Input.GetKeyDown(KeyCode.Q) && canJump)
        {
            StopAllCoroutines();
            if (isSlash)
            {
                SoundManager.PlaySwordSFX();//Plays sword SFX when moving                  
            }
            else
            {
                SoundManager.PlayJumpSound();//Plays jump SFX when moving
            }
            //If the current node is above the player, climbing
            if (worldCursor.transform.position.y >= transform.position.y)
            {
                mousePos posScript = worldCursor.GetComponent<mousePos>();
                anim.SetBool("Jumped", true);
                anim.SetBool("IsMidair", true);
                anim.SetBool("IsGrounded", false);
                StartCoroutine(climb(new Vector3[] { worldCursor.transform.position, posScript.jumpPos2, posScript.jumpPos3}, climbSpeed * 3f));
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && canJump)
        {
            //If the current node is above the player, climbing
            if (worldCursor.transform.position.y >= transform.position.y)
            {
                mousePos posScript = worldCursor.GetComponent<mousePos>();
                anim.SetBool("Jumped", true);
                anim.SetBool("IsMidair", true);
                anim.SetBool("IsGrounded", false);
                if(transform.position.x >= worldCursor.transform.position.x)
                {
                    StartCoroutine(climb(new Vector3[] { findStrafePos(true) }, climbSpeed * 3f));
                }
                else
                {
                    StartCoroutine(climb(new Vector3[] { findStrafePos(false) }, climbSpeed * 3f));
                }
            }
        }
    }

    Vector3 findStrafePos(bool LR)
    {
        //true = Left
        //false = Right
        Vector3 strafePos = Vector3.zero;
        float mod;
        if (LR)
        {
            mod = -1f;
        }
        else
        {
            mod = 1f;
        }
        float dist = 20f;

        do
        {
            Ray zRay = new Ray(transform.position + (new Vector3(dist, 0) * mod), new Vector3(0, 0, 1));

            if (Physics.Raycast(zRay, out RaycastHit zHit, float.MaxValue, mask))
            {
                strafePos = zHit.point;
            }

            dist -= 2f;
            
        } while (strafePos == Vector3.zero);

        return strafePos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Whirlpool"))
        {
            isSlowed = true;
        }
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
            BoxCollider col;
            other.TryGetComponent<BoxCollider>(out col);

            if(col != null)
            {
                col.enabled = false;
            }
            Destroy(other.gameObject, 1.5f);
        }

        if (other.gameObject.CompareTag("NavalMine"))
        {
            isExploded = true;
            Debug.Log(isExploded);

            Vector3 currPos = transform.position;

            Vector3 newPos = currPos;

            newPos -= new Vector3(0, 25);

            Ray mRay = new Ray(newPos, new Vector3(0, 0, 1));

            if (Physics.Raycast(mRay, out RaycastHit mHit, float.MaxValue, mask))
            {
                newPos.y = mHit.point.y;

                StopAllCoroutines();
                StartCoroutine(fall(newPos, climbSpeed * 1.5f, false));
            }
            else
            {
                newPos.y = mHit.point.y;
                StopAllCoroutines();
                StartCoroutine(fall(newPos, climbSpeed * 1.5f, false));
            }
            Destroy(other.gameObject);
            explosion.Play();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Whirlpool"))
        {
            isSlowed = false;
        }
    }

    public IEnumerator climb(Vector3[] destList, float speed)
    {
        foreach(Vector3 destIter in destList)
        {
            if(destIter == Vector3.zero)
            {
                break;
            }
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

            //Particles
            jumpLand.Play();

            //Keeping the player in front of the wall, from the camera's POV by moving the player slightly closer to the camera
            Vector3 dest = Vector3.MoveTowards(destIter, Camera.main.transform.position, 0.1f);

            Vector3 destDist = dest - startPos;

            //GetComponent<oxygenController>().reduceOxygen(Mathf.Abs(destDist.magnitude));

            if (destDist.magnitude > 10)
            {

                if (isSlowed)
                {
                    destDist = destDist.normalized * 6.6f;
                    speed *= 0.66f;
                }
                else
                {
                    destDist = destDist.normalized * 10;
                }


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
            while (pos < 1f)
            {

                //Moving n% of the way between our start and end positions, with n being the y-value of our animation curve at x = "pos"
                transform.position = Vector3.Slerp(newStart, newDest, climbCurve.Evaluate(pos)) + midPoint;
                pos = Mathf.MoveTowards(pos, 1.0f, speed * Time.deltaTime * 3);

                if (pos > 0.2f && innerIsSlash)
                {
                    childObj.SetActive(false);
                    innerIsSlash = false;
                    isSlash = false;
                }

                if (pos > 0.4f && !isSlash)
                {
                    canJump = true;
                    anim.SetBool("Jumped", false);
                    anim.SetBool("IsMidair", false);
                    anim.SetBool("IsGrounded", true);
                    anim.SetBool("Mirrored", !anim.GetBool("Mirrored"));
                }
                else if (pos > 0.6f)
                {
                    canJump = true;
                    anim.SetBool("Jumped", false);
                    anim.SetBool("IsMidair", false);
                    anim.SetBool("IsGrounded", true);
                    anim.SetBool("Mirrored", !anim.GetBool("Mirrored"));
                    jumpLand.Stop(); // Particle End
                }

                yield return null;
            }
            childObj.SetActive(false);
            canJump = true;

        }

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

        jumpLand.Play(); // Particles play

        //Keeping the player in front of the wall, from the camera's POV by moving the player slightly closer to the camera
        dest = Vector3.MoveTowards(dest, Camera.main.transform.position, 0.1f);

        Vector3 destDist = dest - startPos;

        
        if (destDist.magnitude > 10 && isPlayerJump)
        {
            if (isSlowed)
            {
                destDist = destDist.normalized * 6.6f;
                speed *= 0.66f;
            }
            else
            {
                destDist = destDist.normalized * 10;
            }

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
                anim.SetBool("Jumped", false);
                anim.SetBool("IsMidair", false);
                anim.SetBool("IsGrounded", true);
                anim.SetBool("Mirrored", !anim.GetBool("Mirrored"));
                jumpLand.Stop(); // Particles
            }

            yield return null;
        }
        childObj.SetActive(false);
        canJump = true;
        //nodeManager.instance.updateNodes();
    }
}
