using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickupAnim : MonoBehaviour
{
    public static UnityAction<Quaternion> OnUpdateCoinRotation;

    [SerializeField] float _speed;
    [SerializeField] Vector3 _direction;
    Vector3 _previousRotV3;
    bool _isPause;

    private void OnEnable()
    {
        GameManager.OnStateChange += GameManagerOnStateChange;
    }

    private void GameManagerOnStateChange(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.CountDown:
                _isPause = false;
                break;
            case GameManager.GameState.Run:
                _isPause = false;
                break;
            case GameManager.GameState.Pause:
                _isPause = true;
                break;
            case GameManager.GameState.End:
                _isPause = true;
                break;
            default:
                throw new Exception("Something wrong, Patrick?");

        }
    }

    private void OnDisable()
    {
        GameManager.OnStateChange -= GameManagerOnStateChange;

    }

    private void Update()
    {
        if (!_isPause)
        {
            if(_previousRotV3 == null)
            {
                _previousRotV3 = Vector3.zero;
            }
            var toRotate = _previousRotV3 + _direction * _speed * Time.deltaTime;
            var sendRotation = Quaternion.Euler(toRotate);
            _previousRotV3 = toRotate;
            OnUpdateCoinRotation?.Invoke(sendRotation);

        }
    }
}
