using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void Start_EndlessMode()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }
}
