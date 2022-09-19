using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static GameState State = GameState.CountDown;

    public static event UnityAction<GameState> OnStateChange; 
    public GameObject player;
    public GameObject groundSpawner;

    private void Awake()
    {
        Instance = this;
        
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        UpdateGameState(GameState.CountDown);
    }

    public static void UpdateGameState(GameState newState)
    {
        State = newState;


        switch (newState) {
            case GameState.CountDown:
                // Create UI Text coundown
                // Text countdown
                Debug.Log("CountDown");
                break;
            case GameState.Run:
                // player control enable
                // Stage logic enable

                Debug.Log("Run");
                break;
            case GameState.Pause:
                // player control disable
                // Stage logic disable
                // IGM show
                Debug.Log("Pause");
                break;
            case GameState.End:
                Debug.Log("End");
                // Show end screen with score
                break;
        }

        OnStateChange?.Invoke(newState); // !!! "?" is to determine if anyone subscribe 
    }

    public enum GameState
    {
        CountDown,
        Run,
        Pause,
        Fail,
        Win,
        End
    }

    
}
