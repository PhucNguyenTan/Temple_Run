using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundType : MonoBehaviour
{
    Ground_data _data;
    bool _hasObstacle = true;
    bool _hasPickup = true;

    float _speed = 0f;
    bool _isPause = true;
    BoxCollider _box;
    GameObject _ground;
    GameObject _obstacle;
    GameObject _pickup;

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

        if (_hasObstacle && _data.Obstacle != null)
        {
            SpawnObstacle();
        }
        if (_hasPickup && _data.Pickup != null)
        {
            SpawnPickup();
        }
    }

    public Vector3 GetNextSpawnPoint()
    {
        return transform.position + _box.center * 2;
    }

    private void SpawnObstacle()
    {
        var randomXYpos = _data.ObstacleSpawnPoints[Random.Range(0, _data.ObstacleSpawnPoints.Length)];
        Vector3 randomPoint = new Vector3(randomXYpos.x, randomXYpos.y, _box.size.z * 0.5f);
        _obstacle = Instantiate(_data.Obstacle, transform.position + randomPoint, Quaternion.identity, transform);
    }

    private void SpawnPickup()
    {
        //Pickup will be spawn either Behind or Above Obstacle;
        var randomYZpos = _data.PickupSpawnPoints[Random.Range(0, _data.PickupSpawnPoints.Length)];
        Vector3 randomPoint = new Vector3(_obstacle.transform.position.x, randomYZpos.x, _box.size.z*0.5f - randomYZpos.y);
        _pickup = Instantiate(_data.Pickup, transform.position + randomPoint, Quaternion.identity, transform);
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
        _hasPickup = false;
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
