using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Players : MonoBehaviour
{



   
    public int mutualHealth;
    public Sprite[] spriteList; 
    private Canvas MyCanvas;
    public GameObject Ghost;
    Animator GhostAnimator;
    public GameObject Girl;
    Animator GirlAnimator;
    Image temp;
    private void Start()
    {
        MyCanvas = GetComponent<Canvas>();
        temp = MyCanvas.GetComponentInChildren<Image>();
        temp.sprite = spriteList[mutualHealth];
        GhostAnimator =  Ghost.GetComponent<Animator>();
        GirlAnimator = Girl.GetComponent<Animator>();
    }
    private void Update()
    {
        if (mutualHealth <= 0)
        {
            GhostAnimator.SetBool("isAlive", false);
            GirlAnimator.SetBool("isAlive", false);
            temp.sprite = spriteList[0];
            StartCoroutine(DeathWait());

        }
    }

    public float ReturnHealth()
    {
        return mutualHealth;
    }
    public void DealWithHealth(float damadge)
    {

        mutualHealth -= (int)damadge;
        if (mutualHealth > -1 && mutualHealth < spriteList.Length)
        {
            //Image temp = MyCanvas.GetComponentInChildren<Image>();
            temp.sprite = spriteList[mutualHealth];
        }
    }

    IEnumerator DeathWait()
    {
        
        yield return new WaitForSeconds(3.5f);
        Destroy(gameObject);
        Initiate.Fade("End Menu", Color.black, 1f);
       
    }

}
