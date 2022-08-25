using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    GroundSpawner groundSpawner;
    [SerializeField] GameObject _obstacle;
    [SerializeField] Player_data _data;
    [SerializeField] bool _generateObstacle;
    float[] _h_positions = new float[3];
    bool _isPause = true;

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.gameObject.layer == 8)
        {
            groundSpawner.RemoveLastInList();
            //groundSpawner.SpawnGround();
            Destroy(gameObject, 2f); // Destroy object after 2 second

        }
    }

    private void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
        if (_generateObstacle)
        {
            _h_positions = new float[] {_data.laneLeft, _data.laneMid, _data.laneRight};
            SpawnObstacle();

        }

    }

    private void Update()
    {
        if (!_isPause)
        {
            Scrolling();
        }
    }

    private void SpawnObstacle()
    {
        int random_Lane = Random.Range(0, _h_positions.Length);
        Vector3 pointSpawnObstacle = new Vector3(_h_positions[random_Lane], transform.position.y + 0.1f,transform.position.z+0.3f);
        Instantiate(_obstacle, pointSpawnObstacle, Quaternion.identity, transform);
    }

    private void Scrolling()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2.0f * Time.deltaTime);
    }

    public void Pause()
    {
        _isPause = true;
    }

    public void UnPause()
    {
        _isPause = false;
    }

    
}
