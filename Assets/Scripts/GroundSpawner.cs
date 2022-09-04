using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [SerializeField] GroundTile _groundTile;
    [SerializeField] Vector3 _nextSpawnPoint = Vector3.zero;
    [SerializeField] int _tileLimit = 10;
    //private GameObject[10] currentGrounds;
    //public GameObject[] arrayGround;

    GroundTile _currentGround;
    int _groundCount;
    int _currentLevel = 0;
    int _lastLevel = 0;
    int _incremetal = 10;
    float _groundSpeed = 2.0f;
    
    private List<GroundTile> listGround = new List<GroundTile>();
    public bool isPause { get; private set; } = false;

    /*public void SpawnGround(GroundTile prevGround)
    {
        Vector3 spawnPOint = prevGround.transform.GetChild(0).transform.position;
        currentGround = Instantiate(groundTile, spawnPOint, Quaternion.identity);
        
    }*/

    /*public void StartSpawning()
    {
        currentGround = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity);
        nextSpawnPoint = currentGround.transform.GetChild(0).transform.position;
    }*/

    private void Awake()
    {
        GameManager.OnStateChange += GameManager_OnStateChange;
    }

    private void OnDestroy()
    {
        GameManager.OnStateChange -= GameManager_OnStateChange;
    }

    private void GameManager_OnStateChange(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.CountDown:
                Initialize();
                PauseScrolling();
                break;
            case GameManager.GameState.Pause:
                PauseScrolling();
                break;
            case GameManager.GameState.Run:
                UnPauseScrolling();
                break;
            case GameManager.GameState.End:
                PauseScrolling();
                break;
        }
        //throw new System.NotImplementedException();
    }

    private void Start()
    {
        
    }


    private void Update()
    {
        if (!isPause)
        {
            CreateNextGround();
            if (_lastLevel < _currentLevel)
            {
                _lastLevel = _currentLevel;
                _groundSpeed += 1f;
                SpeedUp();
            }
        }
        
    }

    public void RemoveLastInList()
    {
        listGround.RemoveAt(0);
    }

    public void CreateStartingGrounds() {
        for (int i = 0; i < _tileLimit; i++)
        {
            _currentGround = CreateNewGround();
            if (i == 0) 
                _currentGround.NoObstacle();
            listGround.Add(_currentGround);
        }
    }

    GroundTile CreateNewGround()
    {
        _groundCount++;
        _currentLevel = _groundCount / _incremetal;
        GroundTile newGround = Instantiate(_groundTile, _nextSpawnPoint, Quaternion.identity);
        _nextSpawnPoint = newGround.transform.Find("SpawnPoint").transform.position;
        newGround.UpdateScrollSpeed(_groundSpeed);
        return newGround;
    }

    public void CreateNextGround()
    {
        if (listGround.Count < _tileLimit)
        {
            int lastestGround = listGround.Count - 1;
            _nextSpawnPoint = listGround[lastestGround].transform.Find("SpawnPoint").transform.position;
            _currentGround = CreateNewGround();
            _currentGround.UnPause();
            listGround.Add(_currentGround);
        }
    }

    public void PauseScrolling()
    {
        for (int i = 0; i < listGround.Count; i++)
        {
            listGround[i].Pause();
        }
    }
    
    public void UnPauseScrolling()
    {
        for (int i = 0; i < listGround.Count; i++)
        {
            listGround[i].UnPause();
        }
    }

    public void SpeedUp()
    {
        for (int i = 0; i < listGround.Count; i++)
        {
            listGround[i].UpdateScrollSpeed(_groundSpeed);
        }
    }

    public void Initialize()
    {
        for (int i = 0; i<listGround.Count; i++)
        {
            Debug.Log($"{listGround[i].name} was destroyed");
            Destroy(listGround[i].gameObject);
        }
        listGround.Clear();
        _nextSpawnPoint = Vector3.zero;
        _groundCount = 0;
        CreateStartingGrounds();
    }
}
