using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    public int Score { get; private set; } = 0;
    public int CoinOwned { get; private set; } = 0;
    public int CurrentLevel { get; private set; } = 0;

    [SerializeField] Player _player;
    [SerializeField] GroundSpawner _spawner;
    [SerializeField] int _maxLevel;
    int _pastGroundCount = 0;
    int _maxPastGround = 10;

    public UnityAction<int> OnScoreUpdate;
    public UnityAction<int> OnCoinUpdate;
    public UnityAction<int> OnLevelUp;
    public UnityAction<int> OnLevelDown;
    public UnityAction<int> OnLevelUpdate;

    private void OnEnable()
    {
        _player.OnPickupCoin += AddCoin;
        _player.OnPickupCoin += AddScoreCoin;
        _player.OnBigObstacleCollided += SubtractOneLevel;
        _spawner.OnRemoveGround += AddScoreGround;
        GameManager.OnStateChange += GameManager_OnStateChange;
    }

    private void OnDisable()
    {
        _player.OnPickupCoin -= AddCoin;
        _player.OnPickupCoin -= AddScoreCoin;
        _player.OnBigObstacleCollided -= SubtractOneLevel;
        _spawner.OnRemoveGround -= AddScoreGround;
        GameManager.OnStateChange -= GameManager_OnStateChange;
    }

    private void GameManager_OnStateChange(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.CountDown:
                Initialize();
                break;
            case GameManager.GameState.Pause:
                break;
            case GameManager.GameState.Run:
                break;
            case GameManager.GameState.End:
                break;
        }
    }

    void Initialize()
    {
        Score = 0;
        CoinOwned = 0;
        CurrentLevel = 1;
        OnScoreUpdate?.Invoke(Score);
        OnCoinUpdate?.Invoke(CoinOwned);
        OnLevelUpdate?.Invoke(CurrentLevel);

    }

    public void AddScoreCoin()
    {
        Score += 10;
        OnScoreUpdate?.Invoke(Score);
    }


    public void AddCoin()
    {
        CoinOwned += 1;
        OnCoinUpdate?.Invoke(CoinOwned);
    }

    public void AddScoreGround()
    {
        Score += 5;
        OnScoreUpdate?.Invoke(Score);
        _pastGroundCount++;
        if(_pastGroundCount == _maxLevel)
        {
            _pastGroundCount = 0;
            AddOneLevel();
        }
    }

    void AddOneLevel()
    {
        if (CurrentLevel == _maxLevel) return;
        CurrentLevel++;
        OnLevelUp?.Invoke(CurrentLevel);
        OnLevelUpdate?.Invoke(CurrentLevel);
    }

    void SubtractOneLevel()
    {
        if (CurrentLevel == 1) return;
        CurrentLevel--;
        OnLevelDown?.Invoke(CurrentLevel);
        OnLevelUpdate?.Invoke(CurrentLevel);

        if(_pastGroundCount > 5)
            _pastGroundCount = 5;
    }

}
