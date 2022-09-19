using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using System;

public class IGMUI : MonoBehaviour
{
    public bool isPause = false;
    public bool isCountDown;
    [SerializeField] GameObject _pauseMenuUI;
    [SerializeField] GameObject _retryMenuUI;
    [SerializeField] GameObject _countDownUI;
    [SerializeField] ScoreManager _scoreManager;

    [SerializeField] int CountDown = 3;
    int CountDownDisplay;
    TextMeshProUGUI _countdownText;

    GameObject _countDownTextObj;
    //private Panel RetryMenu;
    Slider _healthSlider;
    TextMeshProUGUI _scoreText;
    TextMeshProUGUI _levelText;
    TextMeshProUGUI _coinText;

    public InputHandler controlInput { get; private set; }

    void Awake()
    {
        CountDownDisplay = CountDown;
        GameManager.OnStateChange += GameManager_OnStateChange;
        _countDownTextObj = transform.Find("CountDown").gameObject;
        _countdownText = _countDownTextObj.GetComponent<TextMeshProUGUI>();
        _healthSlider = transform.Find("Health").gameObject.GetComponent<Slider>();
        _scoreText = transform.Find("Score_label").gameObject.GetComponent<TextMeshProUGUI>();
        _coinText = transform.Find("Coin").gameObject.GetComponent<TextMeshProUGUI>();
        _levelText = transform.Find("Level").gameObject.GetComponent<TextMeshProUGUI>();
        //RetryMenu = transform.Find("RetryIGM").gameObject.GetComponent<Panel>();
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
                InitializeIGMUI();
                //StartCoroutine(CountingDown());
                CountingDown();
                break;
            case GameManager.GameState.Pause:
                InputHandler.Instance.Pause.AddListener(HandlePressingPause);
                break;
            case GameManager.GameState.Run:
                InputHandler.Instance.Pause.AddListener(HandlePressingPause);

                break;
            case GameManager.GameState.End:
                InputHandler.Instance.Pause.RemoveAllListeners();
                _retryMenuUI.SetActive(true);
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

    private void OnEnable()
    {
        InputHandler.Instance.Pause.AddListener(HandlePressingPause);
        _scoreManager.OnScoreUpdate += UpdateScore;
        _scoreManager.OnCoinUpdate += UpdateCoin;
        _scoreManager.OnLevelUpdate += UpdateLevel;
    }


    private void OnDisable()
    {
        InputHandler.Instance.Pause.RemoveListener(HandlePressingPause);
        _scoreManager.OnScoreUpdate -= UpdateScore;
        _scoreManager.OnCoinUpdate -= UpdateCoin;
        _scoreManager.OnLevelUpdate -= UpdateLevel;

    }

    void UpdateScore(int score)
    {
        _scoreText.text = "Score: " + score;
    }

    void UpdateCoin(int coin)
    {
        _coinText.text = "Coin: " + coin;
    }

    void UpdateLevel(int level)
    {
        _levelText.text = "Level: " + level;
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
        
    }

    public void Pause() {
        _pauseMenuUI.SetActive(true);
        GameManager.UpdateGameState(GameManager.GameState.Pause);
        isPause = true;

    }

    public void Resume()
    {
        _pauseMenuUI.SetActive(false);
        GameManager.UpdateGameState(GameManager.GameState.Run);
        isPause = false;
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    #endregion

    private IEnumerator CountDownAsync()
    {
        while (CountDownDisplay > 0)
        {
            _countdownText.text = CountDownDisplay.ToString();
            yield return new WaitForSeconds(1f);

            CountDownDisplay--;
        }

        _countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);
        _countDownTextObj.SetActive(false);
        GameManager.UpdateGameState(GameManager.GameState.Run);
    }

    private async void CountingDown()
    {
        while (CountDownDisplay > 0)
        {
            _countdownText.text = CountDownDisplay.ToString();
            await Task.Delay(1000);
            //yield return new WaitForSeconds(1f);

            CountDownDisplay--;
        }

        _countdownText.text = "GO!";
        //yield return new WaitForSeconds(1f);
        await Task.Delay(1000);
        _countDownTextObj.SetActive(false);
        GameManager.UpdateGameState(GameManager.GameState.Run);
    }

    public void SetHealthValue(float health)
    {
        _healthSlider.value = health;
    }

    public void RetryEndless()
    {
        GameManager.UpdateGameState(GameManager.GameState.CountDown);
    }

    public void InitializeIGMUI()
    {
        _retryMenuUI.SetActive(false);
        _pauseMenuUI.SetActive(false);
        _countDownUI.SetActive(true);
        CountDownDisplay = 3;
    } 

}
