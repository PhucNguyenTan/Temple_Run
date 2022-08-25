using UnityEngine;

[CreateAssetMenu(fileName ="Data", menuName = "ScriptableObject/Temple_data", order = 1)]
public class Player_data : ScriptableObject
{
    [Header("Movement")]
    public float SwipeDuration = 1f;
    public float SwipeSpeed = 1f;
    [Header("Jump")]
    public float maxJumpTime = 1.0f;
    public float maxJumpHeight = 0.3f;

    [Header("Lane")]
    public float laneMid = 0f;
    public float laneLeft = -0.25f;
    public float laneRight = 0.25f;
    public LayerMask Standable;
    public float GroudY;
    public float GroundDectectHeight;

    [Header("Properties")]
    public float health = 50f;
    public float forwardSpeed;

    [Header("World_Properties")]
    public float Gravity = -9.8f;
    public float GroundGravity = -0.5f;

    [Header("Debug")]
    public bool IsShowStatesName;



}
