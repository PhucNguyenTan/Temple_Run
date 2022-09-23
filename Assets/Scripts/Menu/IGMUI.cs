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
    [SerializeField] Player_data _playerData;


    [SerializeField] int CountDown = 3;
    int CountDownDisplay;
    TextMeshProUGUI _countdownText;

    GameObject _countDownTextObj;
    //private Panel RetryMenu;
    Image _healthBar;
    Image _healthBarBackGround;
    TextMeshProUGUI _scoreText;
    TextMeshProUGUI _levelText;
    TextMeshProUGUI _coinText;
    Button _pauseBtn;

    public bool IsSaveBtnDisabled { get; private set; }

    public InputHandler controlInput { get; private set; }

    void Awake()
    {
        CountDownDisplay = CountDown;
        GameManager.OnStateChange += GameManager_OnStateChange;
        _countDownTextObj = transform.Find("CountDown").gameObject;
        _countdownText = _countDownTextObj.GetComponent<TextMeshProUGUI>();

        _healthBar = transform.Find("HealthBar").gameObject.GetComponent<Image>();
        _scoreText = transform.Find("Score_label").gameObject.GetComponent<TextMeshProUGUI>();
        _coinText = transform.Find("Coin").gameObject.GetComponent<TextMeshProUGUI>();
        _levelText = transform.Find("Level").gameObject.GetComponent<TextMeshProUGUI>();
        _pauseBtn = transform.Find("PauseBtn").gameObject.GetComponent<Button>();
        _healthBarBackGround = transform.Find("Health_background").gameObject.GetComponent<Image>();
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
                _pauseBtn.gameObject.SetActive(false);
                SetActiveGameGUi(false);
                IsSaveBtnDisabled = false;
                //StartCoroutine(CountingDown());
                CountingDown();
                break;
            case GameManager.GameState.Pause:
                SetActiveGameGUi(false);
                InputHandler.Instance.Pause.AddListener(HandlePressingPause);
                break;
            case GameManager.GameState.Run:
                SetActiveGameGUi(true);
                _pauseBtn.gameObject.SetActive(true);
                InputHandler.Instance.Pause.AddListener(HandlePressingPause);

                break;
            case GameManager.GameState.End:
                SetActiveGameGUi(false);
                _pauseBtn.gameObject.SetActive(false);
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

    public void LockSaveAbility()
    {
        IsSaveBtnDisabled = true;
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
        _pauseBtn.gameObject.SetActive(false);
        GameManager.UpdateGameState(GameManager.GameState.Pause);
        isPause = true;


    }

    public void Resume()
    {
        _pauseBtn.gameObject.SetActive(true);
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
        float healthPercent = health/_playerData.MaxHealth;
        _healthBar.fillAmount = healthPercent;
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

    void SetActiveGameGUi(bool isActive)
    {
        _healthBar.gameObject.SetActive(isActive);
        _scoreText.gameObject.SetActive(isActive);
        _levelText.gameObject.SetActive(isActive);
        _coinText.gameObject.SetActive(isActive);
        _healthBarBackGround.gameObject.SetActive(isActive);

    }


}
