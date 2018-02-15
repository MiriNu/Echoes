using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(AudioSource))]
public class NastyRumor : Enemy
{


    //what to chase
    public Transform target;
    //times per sec to change path
    public float updateRate = 2f;
    //caching
    private Seeker seeker;
    private Rigidbody2D rb;
    //calculated path
    public Path path;
    //AI speed
    public float speed = 260f;
    public ForceMode2D fMode;

    public bool PathHasEnded = false;
    //max distance to the next point
    public float nextWayPointDistance = 3;
    //currently moves towards
    private int currWayPoint = 0;

    public bool searchingForTarget = false;
    //players are in range collider check
    public bool playersInRange = false;
    //player hits the vision collider
    public float chasespeed = 260f;

    public bool facingRight = false;
    private Transform NastyRum;
    private Animator mainAnim;
    private Transform girl;
    private Transform ghost;
    public float searchRadius = 170f;
    public Transform startPosition;
    public GameObject SPosition;
    private AudioSource aud;
    public AudioClip Sound;
    public AudioClip SoundDMG;
    void Start()
    {
        aud = GetComponent<AudioSource>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        NastyRum = GetComponent<Transform>();
        mainAnim = GetComponent<Animator>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        girl = GameObject.Find("Girl").GetComponent<Transform>();
        ghost = GameObject.Find("Ghost").GetComponent<Transform>();
        //SPosition = GameObject.Find("StartPosition");
        startPosition = SPosition.transform;
        //startPosition.parent = null;
        mainAnim.SetBool("alive", true);
        mainAnim.SetBool("movement", false);



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
            if (!playersInRange)
            {
                
               
                seeker.StartPath(transform.position, target.position, OnPathComplete);
                StartCoroutine(UpdatePath());
            }
        }
    }

    IEnumerator searchingTarget()
    {
        //GameObject sResult = GameObject.FindGameObjectsWithTag("Player")[0];
        GameObject sResult;

        if ((girl.position - transform.position).sqrMagnitude <= searchRadius)
        {
            sResult = GameObject.FindGameObjectsWithTag("Player")[0];
        }
        else if ((ghost.position - transform.position).sqrMagnitude <= searchRadius)
        {
            sResult = GameObject.FindGameObjectsWithTag("Player")[1];
        }
        else
        {
            sResult = null;
        }

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

            if (!playersInRange)
            {
                seeker.StartPath(transform.position, target.position, OnPathComplete);
                yield return new WaitForSeconds(1f / updateRate);
                StartCoroutine(UpdatePath());
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

    public void Update()
    {
        if (health <= 0)
        {
            searchingForTarget = false;
            target = null;
            speed = 0.0f;
            Destroy(startPosition.gameObject, 2f);
            Destroy(gameObject, 2f);
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
        if (target)
        {
            aud.PlayOneShot(Sound, 3f);
            Sound = null;
        }

        if ((target.position - transform.position).sqrMagnitude > searchRadius && target != startPosition)
        {
            target = startPosition;
          
        }
       

        if (target == startPosition && PathHasEnded)
        {
            target = null;
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
        if (NastyRum.position.x > target.position.x)
        {
            if (facingRight)
            {
                NastyRum.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
                facingRight = false;
            }
        }
        if (NastyRum.position.x < target.position.x)
        {
            if (!facingRight)
            {
                NastyRum.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
                facingRight = true;
            }
        }
        Vector3 dir = (path.vectorPath[currWayPoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        //move the AI
        rb.AddForce(dir, fMode);
        float dist = Vector3.Distance(transform.position, path.vectorPath[currWayPoint]);
        if (dist < nextWayPointDistance)
        {
            currWayPoint++;
            return;
        }


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && target != startPosition)
        {
            playersInRange = true;
            speed = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {

        }
    }



    //the problem is probably in the spam of looking for a new path
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && target != startPosition)
        {
            playersInRange = false;
            speed = chasespeed;
            //Vector3 dir = (path.vectorPath[currWayPoint] - transform.position).normalized;
            // dir *= speed * Time.fixedDeltaTime;

            seeker.StartPath(transform.position, target.position, OnPathComplete);
            StartCoroutine(UpdatePath());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        }
        if (collision.gameObject.tag == "PlayerBullet")
        {
            health -= playerDamage;
            aud.PlayOneShot(SoundDMG,3f);
            rb.velocity = Vector2.zero;
        }
    }
}
