using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    //[SerializeField] Obstacle_data _data;
    [SerializeField] GameObject _coin;
    [SerializeField] List<Vector3> pickUpSpawnPoints;

    private void Start()
    {
        int random = Random.Range(1, pickUpSpawnPoints.Count);
        if(random > 0)
        {
            Instantiate(_coin, transform.position + pickUpSpawnPoints[random], Quaternion.identity, transform);
        }
    }
}
