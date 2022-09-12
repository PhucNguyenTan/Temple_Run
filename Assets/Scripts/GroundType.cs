using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundType : MonoBehaviour
{
    Ground_data _data;
    [SerializeField] Player_data _player_data;
    [SerializeField] bool _hasObstacle;

    float _speed = 0f;
    bool _isPause = true;
    BoxCollider _box;
    GameObject _ground;

    private void Awake()
    {
        _box = GetComponent<BoxCollider>();
    }



    private void Update()
    {
        if (!_isPause)
        {
            Scrolling();
        }
    }

    public GroundType SetGroundData(Ground_data data)
    {
        _data = data;
        Initinalize();
        return this;

    }
    void Initinalize()
    {

        float zSizePrefab = _data.GroundPrefab.transform.localScale.z;
        _box.size = new Vector3(1f, 1f, zSizePrefab);
        _box.center = new Vector3(0f, 0f, zSizePrefab / 2);

        Vector3 groundSpawnPoint = transform.position + _box.center;
        //This is the prefab GroundSpawner is spawning and this GroundType try to Instantiate a GameObject
        Instantiate(_data.GroundPrefab, groundSpawnPoint, Quaternion.identity, transform);

        if (_hasObstacle && _data.ObstacleTypes.Length > 0)
        {
            SpawnObstacle();
        }
    }

    public Vector3 GetNextSpawnPoint()
    {
        return transform.position + _box.center * 2;
    }

    private void SpawnObstacle()
    {
        float[] lane = new float[3] { _player_data.laneLeft, _player_data.laneMid, _player_data.laneRight};
        int randomLane = Random.Range(0, lane.Length);
        Vector3 pointSpawnObstacle = new Vector3(lane[randomLane] , .15f, _box.center.z);
        int randomObstacleNum = Random.Range(0, _data.ObstacleTypes.Length);
        Instantiate(_data.ObstacleTypes[randomObstacleNum], transform.position + pointSpawnObstacle, Quaternion.identity, transform);
    }


    void Scrolling()
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

    void MoveToObject()
    {
        RaycastHit hit;
        bool touched = Physics.Raycast(transform.position, Vector3.back, out hit, 5.0f, 6);
        if (!touched)
        {
            Scrolling();
            return;
        }
        transform.position = hit.point;
    }

    public void OffsetZ(float offsetZ)
    {
        Vector3 offset = Vector3.zero;
        offset.z = offsetZ;
        transform.position += offset * Time.deltaTime;
    }
}
