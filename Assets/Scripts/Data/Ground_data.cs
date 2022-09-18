using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "ScriptableObject/GroundTile")]
public class Ground_data : ScriptableObject
{
    public GameObject Obstacle;
    public Vector2[] ObstacleSpawnPoints;
    public GameObject Pickup;
    public Vector2[] PickupSpawnPoints;
    public GameObject GroundPrefab;

    public float Probability;
    public float Weight;

}
