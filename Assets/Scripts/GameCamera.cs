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
        targetPos.x = _offset.x;
        targetPos.y = _offset.y;
        transform.position = targetPos;
    }

    private void OnEnable()
    {
        _player.OnObstacleCollided += CameraShake;
        _player.OnBigObstacleCollided += CameraShakeBig;
        _player.OnCrashToGround += CameraShakeBigger;
    }

    private void OnDisable()
    {
        _player.OnBigObstacleCollided -= CameraShakeBig;
        _player.OnObstacleCollided -= CameraShake;
        _player.OnCrashToGround -= CameraShakeBigger;
    }

    public void CameraShake()
    {
        CameraShaker.Instance.ShakeOnce(_shakeMagniture, _shakeRange, _shakeFadeIn * 1.5f, _shakeFadeOut*1.5f);
    }

    public void CameraShakeBig()
    {
        CameraShaker.Instance.ShakeOnce(_shakeMagniture * 1.5f, _shakeRange * 1.5f , _shakeFadeIn, _shakeFadeOut);

    }

    public void CameraShakeBigger()
    {
        CameraShaker.Instance.ShakeOnce(_shakeMagniture * 2f, _shakeRange * 2f, _shakeFadeIn*1.5f, _shakeFadeOut*1.5f);

    }
}
