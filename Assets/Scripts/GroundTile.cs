using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    GroundSpawner groundSpawner;
    [SerializeField]
    GameObject obstacle;
    [SerializeField]
    Player_data data;
    float[] h_positions = new float[3];

    private void OnTriggerExit(Collider other)
    {
        groundSpawner.RemoveLastInList();
        //groundSpawner.SpawnGround();
        Destroy(gameObject, 2f); // Destroy object after 2 second
    }

    private void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
        h_positions = new float[] {data.laneLeft, data.laneMid, data.laneRight};
        SpawnObstacle();
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - 2.0f* Time.deltaTime);
    }

    private void SpawnObstacle()
    {
        int random_Lane = Random.Range(0, h_positions.Length);
        Vector3 pointSpawnObstacle = new Vector3(h_positions[random_Lane], transform.position.y + 0.1f,transform.position.z+0.3f);
        Instantiate(obstacle, pointSpawnObstacle, Quaternion.identity, transform);
    }

}
