using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/GroundTile")]
public class Ground_data : ScriptableObject
{
    public GameObject[] ObstacleTypes;
    public GameObject GroundPrefab;
    public int Level;



    [Header("Debug")]
    public bool ShowObstacle;
}
