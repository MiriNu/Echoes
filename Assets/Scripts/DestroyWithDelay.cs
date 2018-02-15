using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithDelay : MonoBehaviour {
    public float delay;
	


    void Start()
    {
        Destroy(gameObject,delay);
        //Debug.Log("a shield is here.");

    }
    //public void StartShielding(Animator a)
    //{
    //    Debug.Log("Someone called me too11111!");
    //    a.SetBool("isShielding", false);
    //    StartCoroutine(DestoryWithActionAndDelay(
    //    () =>
    //    {
    //        a.SetBool("isShielding", true);
    //    }));
                                        
    //}

    //IEnumerator DestoryWithActionAndDelay(System.Action callback = null)
    //{
    //    float timer = Time.realtimeSinceStartup;
    //    while (Time.realtimeSinceStartup - timer  > delay)
    //    {
    //        yield return null;    
    //    }
    //    //now that we are done, lets do whatever they asked us, and destroy.

    //    if (callback != null)
    //        callback();
        
    //    Destroy(this.gameObject);


    //}

}
