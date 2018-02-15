using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



[RequireComponent(typeof(AudioSource))]
public class Ghost : MonoBehaviour
{
    private Rigidbody2D ghostBody;
    public GameObject ghost;
    public int jumpStrength = 170;
    public int acceleration = 2;
    public int maxSpeed = 15;
    float speed = 3;
    public Transform BottomPart;
    //sprite change
    private bool ghostFaceRight = true;

    //for fireing
    public GameObject RightShield;
    public GameObject LeftShield;
    private Transform FirePosGhost;
    public float fireRate = 2.2F;
    private float nextFire = 0.0F;
    public int MaxHealth;
    public Text healthText;
    public Image visualHeealth;
    public float coolDown;
    public float anoRexiaBulletDamage;
    public float healthAdded;
    public float AnorexDmg;
    public float NastyRumorDmg;
    public Players playerUiControl;

    //Animation
    private Animator mainAnim;

    private AudioSource aud;
    public AudioClip SoundDmg;
    public AudioClip SoundJump;


    void Start()
    {
        ghostBody = GetComponent<Rigidbody2D>();
        ghostBody.constraints = RigidbodyConstraints2D.FreezeRotation;
        mainAnim = GetComponent<Animator>();
        FirePosGhost = transform.Find("FirePos");
        InvokeRepeating("fire", 5f, 5f);
        aud = GetComponent<AudioSource>();
        mainAnim.SetBool("isAlive", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (mainAnim.GetBool("isAlive"))
        {

            if (Input.GetKey(KeyCode.A))
            {

                ghost.transform.Translate(Vector3.left * speed * Time.deltaTime);
                if (Input.GetKey(KeyCode.A) && ghostFaceRight)
                {
                    transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
                    ghostFaceRight = false;
                }
            }

            if (Input.GetKey(KeyCode.D))
            {

                ghost.transform.Translate(Vector3.right * speed * Time.deltaTime);
                if (Input.GetKey(KeyCode.D) && !ghostFaceRight)
                {
                    transform.localScale = new Vector3(transform.localScale.x * (-1), transform.localScale.y, transform.localScale.z);
                    ghostFaceRight = true;
                }

            }

            if (Input.GetKeyDown(KeyCode.Tab) && Time.time > nextFire)
            {


                nextFire = Time.time + fireRate;
                mainAnim.SetBool("isShielding", true);

                if (ghostFaceRight)
                {
                    Instantiate(RightShield, FirePosGhost.position, Quaternion.identity);

                }
                else
                {
                    Instantiate(LeftShield, FirePosGhost.position, Quaternion.identity);
                }
                StartCoroutine(AnimDisabler());

            }

            if (playerUiControl.ReturnHealth() <= 0)
            {
                mainAnim.SetBool("isAlive", false);
            }

        }
    }





    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Lives")
        {
            playerUiControl.DealWithHealth(-healthAdded);
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (mainAnim.GetBool("isAlive"))
        {
            if (collision.gameObject.tag == "Ground" && Input.GetKey(KeyCode.W) && (ghost.transform.position.y - 0.3f) >= collision.transform.position.y)
            {

                ghostBody.AddForce(Vector2.up * jumpStrength);
                aud.PlayOneShot(SoundJump, 0.5f);
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "AnorexBullet" || collision.gameObject.tag == "EnemyBullet")
        {
            aud.PlayOneShot(SoundDmg, 0.5f);
            playerUiControl.DealWithHealth(AnorexDmg);
            jumpStrength = 0;
            StartCoroutine(JumpStrengthDisabler());
        }
        if (collision.gameObject.tag == "NastyRumor")
        {
            aud.PlayOneShot(SoundDmg, 0.5f);
            playerUiControl.DealWithHealth(NastyRumorDmg);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "DeathZone")
        {
            Destroy(gameObject);
            MyLvlManager.S.LoadLevel("End Menu");
        }
       

    }
    IEnumerator JumpStrengthDisabler()
    {
        yield return new WaitForSeconds(2);
        jumpStrength = 170;
    }
    IEnumerator AnimDisabler()
    {
        yield return new WaitForSeconds(2f);
        mainAnim.SetBool("isShielding", false);
    }
}
