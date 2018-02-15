using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Girl : MonoBehaviour {

    public float speed = 3f;
	public GameObject girl;
    private Sprite girlSpr;
	private Rigidbody2D girlBody;
	public int jumpStrength = 150;
	public Transform BottomPart;
    public GameObject RightBullet;
    public GameObject LeftBullet;
    private Transform FirePos;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private bool faceRightGirl = false;
    public float fireRate = 0.5F;
    private float nextFire = 0.0F;
    public float healthAdded;
    public float AnorexDmg;
    public float NastyRumorDmg;

    public Players playerUiControl;


    private AudioSource aud;
    public AudioClip SoundDmg;
    public AudioClip SoundJump;
    void Start()
    {
        girlBody = GetComponent<Rigidbody2D>();
        girlBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        boxCollider = GetComponent<BoxCollider2D>();
        girlSpr = GetComponent<Sprite>();
        FirePos = transform.Find("FirePos");
        animator = GetComponent<Animator>();
        InvokeRepeating("fire", 5f, 5f);
        BottomPart = transform.Find("Bot");
        aud = GetComponent<AudioSource>();
        animator.SetBool("isAlive", true);
    }

    IEnumerator AnimDisabler()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isShoot", false);
    }
    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsRunning", false);
        animator.SetBool("isShoot", false);
        //(walking
        if (animator.GetBool("isAlive"))
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {

                animator.SetBool("IsRunning", true);
                girl.transform.Translate(Vector3.left * speed * Time.deltaTime);
                if (Input.GetKey(KeyCode.LeftArrow) && faceRightGirl)
                {
                    transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
                    faceRightGirl = false;

                }
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                animator.SetBool("IsRunning", true);
                girl.transform.Translate(Vector3.right * speed * Time.deltaTime);
                if (Input.GetKey(KeyCode.RightArrow) && !faceRightGirl)
                {

                    transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
                    faceRightGirl = true;

                }

            }
            /*if (Input.GetKey(KeyCode.UpArrow) && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)))
            {
                Debug.Log("ShootWhileRun");
                animator.SetBool("isJump", true);
                animator.SetBool("isAir", true);
                StartCoroutine(AnimDisabler2());

            }*/


            //fire bullets
            if (Input.GetKey(KeyCode.Space) && Time.time > nextFire)
            {
                animator.SetBool("isShoot", true);
                nextFire = Time.time + fireRate;
                if (!faceRightGirl)
                    Instantiate(LeftBullet, FirePos.position, Quaternion.identity);
                else
                    Instantiate(RightBullet, FirePos.position, Quaternion.identity);
                //StartCoroutine(AnimDisabler());
            }

        }
	}

    IEnumerator AnimDisabler2()
    {
        yield return new WaitForSeconds(0.4f);
            animator.SetBool("isJump", false);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (animator.GetBool("isAlive"))
        {
            if (collision.gameObject.tag == "Ground" && Input.GetKey(KeyCode.UpArrow) && (girl.transform.position.y - 0.4f) >= collision.transform.position.y)
            {
                animator.SetBool("isJump", true);
                girlBody.AddForce(Vector2.up * jumpStrength);
                aud.PlayOneShot(SoundJump, 0.5f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Lives")
        {
            playerUiControl.DealWithHealth(-healthAdded);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "NastyRumor")
        {
            aud.PlayOneShot(SoundDmg, 1f);
            playerUiControl.DealWithHealth(NastyRumorDmg);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "DeathZone")
        {
            Destroy(gameObject);
            MyLvlManager.S.LoadLevel("End Menu");
        }
        if (collision.gameObject.tag == "AnorexBullet"|| collision.gameObject.tag == "EnemyBullet")
        {
            aud.PlayOneShot(SoundDmg,1f);
            playerUiControl.DealWithHealth(AnorexDmg);
            speed = 1f;
            StartCoroutine(SpeedDisabler());

        }
        if(collision.gameObject.tag == "EnemyBullet")
        {
            aud.PlayOneShot(SoundDmg, 1f);
            //DealWithHealth(AnorexDmg);
            speed = 1f;
            StartCoroutine(SpeedDisabler());
        }


        if (collision.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(collision.collider, boxCollider);

        }

        if(collision.gameObject.tag == "Ground")
        {
            animator.SetBool("isAir", false);
            animator.SetBool("isJump", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            animator.SetBool("isAir", true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Lives")
        {
            playerUiControl.DealWithHealth(-healthAdded);

        }
    }

    IEnumerator SpeedDisabler()
    {
        yield return new WaitForSeconds(3);
        speed = 3f;
    }
}


