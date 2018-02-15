using System.Collections;
using Pathfinding;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class NastyRumorAI : Enemy
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
    public bool playersInRangeLeft = false;
    public bool playersInRangeRight = false;
    //player hits the vision collider
    float rightDot;
    public float chasespeed = 300f;

    public bool facingRight = false;
    private Transform nasty;
    private CircleCollider2D nastyCollider;
    private Animator mainAnim;
    private Transform girl;
    private Transform ghost;
    private SpriteRenderer NastyRumorSprite;
    public float searchRadius;
    private AudioSource aud;
    public AudioClip Sound;

    void Start()
    {
        aud = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        nasty = GetComponent<Transform>();
        mainAnim = GetComponent<Animator>();
        NastyRumorSprite = GetComponent<SpriteRenderer>();
        nastyCollider = GetComponent<CircleCollider2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        girl = GameObject.Find("Girl").GetComponent<Transform>();
        ghost = GameObject.Find("Ghost").GetComponent<Transform>();
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
			if (!playersInRangeLeft && !playersInRangeRight)
            {
                seeker.StartPath(transform.position, target.position, OnPathComplete);
                StartCoroutine(UpdatePath());
            }
           /* if (!playersInRangeLeft)
            {
                anim.SetBool("IsAttackL", false);

            }

            if (!playersInRangeRight)
            {
                anim.SetBool("IsAttackR", false);

            }*/

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

            if (!playersInRangeLeft && !playersInRangeRight)
            {
                seeker.StartPath(transform.position, target.position, OnPathComplete);
                yield return new WaitForSeconds(1f / updateRate);
                StartCoroutine(UpdatePath());
            }
            /*if (!playersInRangeLeft)
            {
                anim.SetBool("IsAttackL", false);
            }
            if (!playersInRangeRight)
            {
                anim.SetBool("IsAttackR", false);
            }*/
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

            mainAnim.SetBool("alive", false);
            searchingForTarget = false;
            speed = 0.0f;
            nastyCollider.enabled = false;
            if (NastyRumorSprite.color.a != 0f)
            {
                float k = NastyRumorSprite.color.a;
                k-=0.02f;
                NastyRumorSprite.color = new Color(1f, 1f, 1f, k);
            }
          
            Destroy(gameObject, 2f);
          
           
        }
    }

    private void FixedUpdate()
    {
        mainAnim.SetBool("movement", false);
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
        if (target)
        {
            aud.PlayOneShot(Sound, 0.1f);
        }
        if (path == null)
        {
            return;
        }
        if (currWayPoint >= path.vectorPath.Count)
        {
            if (PathHasEnded)
            {
                
                return;
            }

            //Debug.Log("path ended");
            PathHasEnded = true;
            return;
        }

        PathHasEnded = false;

        //direction towards next waypoint
        if (nasty.position.x > target.position.x)
        {
            if (facingRight)
            {           
                nasty.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
                facingRight = false;
            }
        }
        if (nasty.position.x < target.position.x)
        {
            if (!facingRight)
            {
                nasty.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
                facingRight = true;
            }
        }
        Vector3 dir = (path.vectorPath[currWayPoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;

        //move the AI
        mainAnim.SetBool("movement", true);
        rb.AddForce(dir, fMode);
        float dist = Vector3.Distance(transform.position, path.vectorPath[currWayPoint]);
        if (dist < nextWayPointDistance)
        {
            currWayPoint++;
            return;
        }



    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        }
        else if (collision.gameObject.tag == "PlayerBullet")
        {
            health -= playerDamage;
            rb.velocity = Vector2.zero;
        }
        else if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Player")
        {
            health = 0;
        }

        /*{
            //Destroy();
            health = 0;
            Girl.TakeDamage(5);
        }*/
    }
}