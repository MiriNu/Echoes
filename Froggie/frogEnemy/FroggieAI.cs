using System.Collections;
using Pathfinding;
using UnityEngine;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class FroggieAI : MonoBehaviour
{

    //what to chase
    public Transform target;
    public Animator anim;
    //times per sec to change path
    public float updateRate = 2f;

    //caching
    private Seeker seeker;
    private Rigidbody2D rb;

    //calculated path
    public Path path;

    //AI speed
    public float speed = 300f;
    public ForceMode2D fMode;

    public bool PathHasEnded = false;

    //max distance to the next point
    public float nextWayPointDistance = 3;

    //currently moves towards
    private int currWayPoint = 0;

    public bool searchingForTarget = false;

    //players are in range collider check
    public bool playersInRangeLeft = true;
    public bool playersInRangeRight = true;
    //player hits the vision collider
    float rightDot;
    public float chasespeed = 300f;
    void Start()
    {
        anim = gameObject.transform.GetChild(0).GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if (target == null)
        {
            if (!searchingForTarget)
            {
                searchingForTarget = true;
                StartCoroutine(searchingTarget());

                return;
            }
            //start new path towards target position
            //
            //seeker.StartPath(transform.position, target.position, OnPathComplete);
            //StartCoroutine(UpdatePath());
            if (!playersInRangeLeft && !playersInRangeRight)
            {
                Debug.Log("spam 3");
                seeker.StartPath(transform.position, target.position, OnPathComplete);
                StartCoroutine(UpdatePath());
            }
            if (!playersInRangeLeft)
            {
                anim.SetBool("IsAttackL", false);

            }

            if (!playersInRangeRight)
            {
                anim.SetBool("IsAttack", false);

            }

        }
    }

    IEnumerator searchingTarget()
    {
        GameObject sResult= GameObject.FindGameObjectsWithTag("Player")[0];
       
        if (sResult == null)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(searchingTarget());
        }
        else
        {
            target = sResult.transform;
            searchingForTarget = false;
            StartCoroutine(UpdatePath());
            yield return false;
        }
    }
    IEnumerator UpdatePath()
    {

        if (target == null)
        {
            if (!searchingForTarget)
            {
                searchingForTarget = true;
                StartCoroutine(searchingTarget());
            }
            yield return false;
        }

        else
        {

        //seeker.StartPath(transform.position, target.position, OnPathComplete);
        //yield return new WaitForSeconds(1f / updateRate);
        //StartCoroutine(UpdatePath());

         if (!playersInRangeLeft && !playersInRangeRight)
         {
             Debug.Log("spam 1");
             seeker.StartPath(transform.position, target.position, OnPathComplete);
             yield return new WaitForSeconds(1f / updateRate);
             StartCoroutine(UpdatePath());
         }
        if (!playersInRangeLeft)
            {
                anim.SetBool("IsAttackL", false);
            }
        if (!playersInRangeRight)
            {
                anim.SetBool("IsAttack", false);
            }
        }
    }

    public void OnPathComplete(Path p)
    {
       // Debug.Log("got path, did it get an error?"+p.error);
        if (!p.error)
        {
            path = p;
            currWayPoint = 0;
        }
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
           if (!searchingForTarget)
            {
                searchingForTarget = true;
                StartCoroutine(searchingTarget());
            }
            return;
            
        }

        //always has to look at player

        if (path == null)
            return;
        if (currWayPoint >= path.vectorPath.Count)
        {
            if (PathHasEnded)
                return;

           // Debug.Log("path ended");
            PathHasEnded = true;
            return;
        }

        PathHasEnded = false;

        //direction towards next waypoint
        Vector3 dir = (path.vectorPath[currWayPoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        //move the AI
        
        rb.AddForce(dir, fMode);
        float dist= Vector3.Distance(transform.position, path.vectorPath[currWayPoint]);
        if (dist < nextWayPointDistance)
        {
            currWayPoint++;
            return;
        }
        
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if ((this.transform.position.x - collision.transform.position.x) < 0)
        {
          
            rightDot = 1;
        }
        else if ((this.transform.position.x - collision.transform.position.x) > 0)
        {
         
            rightDot = -1;
        }

        if (collision.tag == "Player")
        {
            if (rightDot > 0)
            {
                playersInRangeRight = true;
                anim.SetBool("IsAttack", true);
              
            }
            if (rightDot < 0)
            {
                playersInRangeLeft = true;
                anim.SetBool("IsAttackL", true);
             
            }
            speed = 0;
         }
    }
 
    //the problem is probably in the spam of looking for a new path
    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((this.transform.position.x - collision.transform.position.x) < 0)
        {
            
            rightDot = 1;
        }
        else if ((this.transform.position.x - collision.transform.position.x) > 0)
        {
          
            rightDot = -1;
        }

        if (collision.tag == "Player")
        {
            if (rightDot > 0)
            {
                playersInRangeRight = false;
                anim.SetBool("IsAttack", false);
                
            }
            if (rightDot < 0)
            {
                playersInRangeLeft = false;
                anim.SetBool("IsAttackL", false);
               
            }
            speed = chasespeed;
            //Vector3 dir = (path.vectorPath[currWayPoint] - transform.position).normalized;
            // dir *= speed * Time.fixedDeltaTime;
            Debug.Log("spam 2");
            seeker.StartPath(transform.position, target.position, OnPathComplete);
            StartCoroutine(UpdatePath());
        }
    }

}