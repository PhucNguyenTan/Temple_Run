using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private void LateUpdate()
    {
        Vector3 targetPos = target.position + offset;
        targetPos.x = 0f;
        targetPos.y = 0.5f;
        transform.position = targetPos;

    }
}
