using UnityEngine;
using EZCameraShake;
using UnityEngine.Events;

public class GameCamera : MonoBehaviour
{
    [SerializeField] float _xPos;
    [SerializeField] float _yPos;
    [SerializeField] Player _player;
    [SerializeField] float _smoothSpeed;
    [SerializeField] Vector3 _offset;

    [SerializeField] float _shakeMagniture;
    [SerializeField] float _shakeRange;
    [SerializeField] float _shakeFadeIn;
    [SerializeField] float _shakeFadeOut;
    private void LateUpdate()
    {
        Vector3 targetPos = _player.transform.position + _offset;
        targetPos.x = 0f;
        targetPos.y = 0.5f;
        transform.position = targetPos;
    }

    private void OnEnable()
    {
        _player.OnObstacleCollided += CameraShake;
    }

    private void OnDisable()
    {
        _player.OnObstacleCollided -= CameraShake;
    }

    public void CameraShake()
    {
        CameraShaker.Instance.ShakeOnce(_shakeMagniture, _shakeRange, _shakeFadeIn, _shakeFadeOut);
    }
}
