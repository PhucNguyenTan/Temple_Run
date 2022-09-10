using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [SerializeField] Ground_data[] _groundDatas;
    [SerializeField] GroundType _ground;
    [SerializeField] Vector3 _nextSpawnPoint = Vector3.zero;
    [SerializeField] int _tileLimit = 10;
    [SerializeField] int _maxLevel = 10;

    GroundType _currentGround;
    int _groundCount;
    int _currentLevel = 0;
    int _lastLevel = 0;
    int _incremetal = 10;
    float _groundSpeed = 2.0f;
    List<GroundType> listGround = new List<GroundType>();
    
    public bool isPause { get; private set; } = false;

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
            CheckLevelUp();
            CreateNextGround();


        }
        
    }

    void CheckLevelUp()
    {
        if (_currentLevel >= _maxLevel) return;

        _currentLevel = _groundCount / _incremetal;
        if (_lastLevel < _currentLevel)
        {
            _lastLevel = _currentLevel;
            _groundSpeed += 1f;
            SpeedUp();
        }
    }

    public void RemoveFirstInList()
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

    GroundType CreateNewGround()
    {
        _groundCount++;

        //This is GroundSpawner in the scene
        GroundType newGround = Instantiate(_ground, _nextSpawnPoint, Quaternion.identity);
        //newGround.SetGroundData(_groundDatas[1]);
        newGround.SetGroundData(_groundDatas[Random.Range(0, _groundDatas.Length)]);
        newGround.name += "_" + _groundCount;

        _nextSpawnPoint = newGround.GetNexSpawnPoint();
        newGround.UpdateScrollSpeed(_groundSpeed);
        newGround.UnPause();
        return newGround;
    }

    public void CreateNextGround()
    {
        if (listGround.Count < _tileLimit)
        {
            int lastestGround = listGround.Count - 1;
            _nextSpawnPoint = listGround[lastestGround].GetNexSpawnPoint(); 
            Vector3 test = _nextSpawnPoint;
            _currentGround = CreateNewGround();
            _currentGround.OffsetZ(-_groundSpeed);
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

    private void OnTriggerExit(Collider other)
    {
        int objLayerNum = other.transform.gameObject.layer;
        if (objLayerNum == 9)
        {
            RemoveFirstInList();
            Destroy(other.gameObject);
        }
    }
}
