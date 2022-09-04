using UnityEngine;
using EZCameraShake;
using UnityEngine.Events;

public class Camera : MonoBehaviour
{
    [SerializeField] float xPos;
    [SerializeField] float yPos;
    [SerializeField] Player player;
    [SerializeField] float smoothSpeed;
    [SerializeField] Vector3 offset;

    private void LateUpdate()
    {
        Vector3 targetPos = player.transform.position + offset;
        targetPos.x = 0f;
        targetPos.y = 0.5f;
        transform.position = targetPos;
    }

    private void OnEnable()
    {
        Player.OnObstacleCollided += CameraShake;
    }

    private void OnDisable()
    {
        Player.OnObstacleCollided -= CameraShake;
    }

    public void CameraShake()
    {
        CameraShaker.Instance.ShakeOnce(3f, 3f, 0.2f, 0.4f);
    }
}
