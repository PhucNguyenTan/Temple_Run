using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IGMUI : MonoBehaviour
{
    public bool isPause = false;
    public bool isCountDown;
    public GameObject PauseMenuUI;

    public InputHandler controlInput { get; private set; }


    public void Start()
    {
        controlInput = GetComponent<InputHandler>();
    }

    public void Update()
    {
        
    }


    public void HandlePressingPause()
    {
        
            if (isPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
            controlInput.UsedPause();
        
    }

    public void Pause() {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPause = true;

    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPause = false;
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
