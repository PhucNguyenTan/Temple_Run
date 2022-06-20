using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class IGMUI : MonoBehaviour
{
    public bool isPause = false;
    public bool isCountDown;
    public GameObject PauseMenuUI;

    [SerializeField]
    private int CountDown = 3;
    private int CountDownDisplay;
    private TextMeshProUGUI displayText;

    private GameObject countDownText;
    private Slider healthSlider;


    public InputHandler controlInput { get; private set; }

    void Awake()
    {
        CountDownDisplay = CountDown;
        GameManager.OnStateChange += GameManager_OnStateChange;
        countDownText = transform.Find("CountDown").gameObject;
        displayText = countDownText.GetComponent<TextMeshProUGUI>();
        healthSlider = transform.Find("Health").gameObject.GetComponent<Slider>();
        Debug.Log(healthSlider.value);
        

    }

    private void OnDestroy()
    {
        GameManager.OnStateChange -= GameManager_OnStateChange;
    }

    private void GameManager_OnStateChange(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.CountDown:
                StartCoroutine(CountingDown());
                break;
            case GameManager.GameState.Pause:
                break;
            case GameManager.GameState.Run:
                break;
            case GameManager.GameState.End:
                break;
        }
        //throw new System.NotImplementedException();
    }

    void Start()
    {
        controlInput = GetComponent<InputHandler>();
    }

    public void Update()
    {
        
    }

    #region IGM
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
        GameManager.UpdateGameState(GameManager.GameState.Pause);
        isPause = true;

    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        GameManager.UpdateGameState(GameManager.GameState.Run);
        isPause = false;
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    #endregion

    private IEnumerator CountingDown()
    {
        while (CountDownDisplay > 0)
        {
            displayText.text = CountDownDisplay.ToString();
            yield return new WaitForSeconds(1f);

            CountDownDisplay--;
        }

        displayText.text = "GO!";
        yield return new WaitForSeconds(1f);
        countDownText.SetActive(false);
        GameManager.UpdateGameState(GameManager.GameState.Run);
    }

    public void SetHealthValue(float health)
    {
        healthSlider.value = health;
    }
}
