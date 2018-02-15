using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MyLvlManager : MonoBehaviour
{

    public static MyLvlManager S;

    private void Awake()
    {
        S = this;
    }
    public void LoadLevel(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void ExitApp()
    {
        Application.Quit();
    }
}
