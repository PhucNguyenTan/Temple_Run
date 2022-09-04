using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    GroundSpawner groundSpawner;
    [SerializeField] GameObject _obstacle;
    [SerializeField] Player_data _data;
    [SerializeField] bool _hasObstacle;
    float _speed = 0f;
    float[] _h_positions = new float[3];
    bool _isPause = true;

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.gameObject.layer == 8)
        {
            groundSpawner.RemoveLastInList();
            Destroy(gameObject, 1f);

        }
    }

    private void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
        if (_hasObstacle)
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
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - _speed * Time.deltaTime);
    }

    public void UpdateScrollSpeed(float newSpeed)
    {
        _speed = newSpeed;
    }

    public void Pause()
    {
        _isPause = true;
    }

    public void UnPause()
    {
        _isPause = false;
    }

    public void NoObstacle()
    {
        _hasObstacle = false;
    }

    
}
