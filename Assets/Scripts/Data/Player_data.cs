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
}
