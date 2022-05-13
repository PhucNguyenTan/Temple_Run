using UnityEngine;

[CreateAssetMenu(fileName ="Data", menuName = "ScriptableObject/Temple_data", order = 1)]
public class Player_data : ScriptableObject
{
    [Header("Swipe_Moment")]
    public float SwipeRange = 1f;
    public float SwipeSpeed = 1f;
    public Vector3 _goalPosition = new Vector3(1f, 1f, 1f);

    public Vector3[] test;

    public float forwardSpeed;

    public float gravity = -9.8f;

    public float maxJumpTime = 1.0f;

    public float laneMid = 0f;

    public float laneLeft = -1f;
    public float laneRight = 1f;

    public float level1Up = 0.5f;
    public float level2Up = 1f;
}
