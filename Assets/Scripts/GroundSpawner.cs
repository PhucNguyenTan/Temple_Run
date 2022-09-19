using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundSpawner : MonoBehaviour
{
    [SerializeField] GroundType _ground;
    [SerializeField] Vector3 _nextSpawnPoint = Vector3.zero;
    [SerializeField] ScoreManager _scoreManager;
    [SerializeField] int _tileLimit = 10;

    [SerializeField] Ground_data _startGround;
    [SerializeField] List<Ground_data> _easyGrounds;
    [SerializeField] List<Ground_data> _normalGrounds;
    [SerializeField] List<Ground_data> _hardGrounds;
    [SerializeField] float _speedPerLevel;

    [SerializeField] float _groundSpeed = 2.0f;
    GroundType _currentGround;

    List<Ground_data> _allowedGrounds = new List<Ground_data>();
    List<Ground_data> _disallowedGrounds = new List<Ground_data>();
    int _groundCount;
    int _currentLevel;
    int _lastLevel;
    List<GroundType> listGround = new List<GroundType>();

    public UnityAction OnRemoveGround;
    
    public bool isPause { get; private set; } = false;

    private void Awake()
    {
        GameManager.OnStateChange += GameManager_OnStateChange;
    }

    private void OnEnable()
    {
        _scoreManager.OnLevelUp += LevelUp;
        _scoreManager.OnLevelDown += LevelDown;
    }

    private void OnDisable()
    {
        _scoreManager.OnLevelDown -= LevelDown;
        _scoreManager.OnLevelUp -= LevelUp;
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
    }



    private void Update()
    {
        if (!isPause)
        {
            //CheckLevelUp();
            CreateNextGround();
        }
        
    }

    void LevelUp(int level)
    {
        _groundSpeed += _speedPerLevel;
        LevelChange(level);
    }

    void LevelDown(int level)
    {
        _groundSpeed -= _speedPerLevel;
        LevelChange(level);
    }

    void LevelChange(int level)
    {
        SpeedUp();
        if (level < 3)
        {
            _disallowedGrounds.Clear();
            _allowedGrounds.Clear();
            _allowedGrounds.AddRange(_easyGrounds);
        }
        else if(level < 7)
        {
            _disallowedGrounds.Clear();
            _allowedGrounds.Clear();
            _allowedGrounds.AddRange(_normalGrounds);
        }
        else
        {
            _disallowedGrounds.Clear();
            _allowedGrounds.Clear();
            _allowedGrounds.AddRange(_hardGrounds);
        }
    }

    public void RemoveFirstInList()
    {
        listGround.RemoveAt(0);
    }

    #region Spawn functions
    public void CreateStartingGrounds() {
        for (int i = 0; i < _tileLimit; i++)
        {
            if (i == 0)
                _currentGround = CreateFirstGround();
            else
                _currentGround = CreateNewGround();
            listGround.Add(_currentGround);
        }
    }

    GroundType CreateFirstGround()
    {
        _groundCount++;
        GroundType newGround = Instantiate(_ground, _nextSpawnPoint, Quaternion.identity);
        newGround.SetGroundData(_startGround);

        _nextSpawnPoint = newGround.GetNextSpawnPoint();
        newGround.UpdateScrollSpeed(_groundSpeed);
        newGround.UnPause();
        return newGround;
    }

    GroundType CreateNewGround()
    {
        _groundCount++;
        int randomGroundindex = Random.Range(0, _allowedGrounds.Count - 1);

        GroundType newGround = Instantiate(_ground, _nextSpawnPoint, Quaternion.identity);
        newGround.SetGroundData(_allowedGrounds[randomGroundindex]);
        newGround.name += "_" + _groundCount;

        _disallowedGrounds.Add(_allowedGrounds[randomGroundindex]);
        _allowedGrounds.RemoveAt(randomGroundindex);
        if(_allowedGrounds.Count == 0)
        {
            _allowedGrounds.AddRange(_disallowedGrounds);
            _disallowedGrounds.Clear();
        }

        _nextSpawnPoint = newGround.GetNextSpawnPoint();
        newGround.UpdateScrollSpeed(_groundSpeed);
        newGround.UnPause();
        return newGround;
    }

    public void CreateNextGround()
    {
        if (listGround.Count < _tileLimit)
        {
            int lastestGround = listGround.Count - 1;
            _nextSpawnPoint = listGround[lastestGround].GetNextSpawnPoint(); 
            Vector3 test = _nextSpawnPoint;
            _currentGround = CreateNewGround();
            _currentGround.OffsetZ(-_groundSpeed);
            listGround.Add(_currentGround);
        }
    }

    #endregion

    #region Handle Ground
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
    #endregion

    public void Initialize()
    {
        for (int i = 0; i<listGround.Count; i++)
        {
            Debug.Log($"{listGround[i].name} was destroyed");
            Destroy(listGround[i].gameObject);
        }
        _currentLevel = 0;
        _lastLevel = 0;
        listGround.Clear();
        _nextSpawnPoint = Vector3.zero;
        _groundCount = 0;
        _groundSpeed = 2f;
        _disallowedGrounds.Clear();
        _allowedGrounds.Clear();
        _allowedGrounds.AddRange(_easyGrounds);
        CreateStartingGrounds();

    }

    private void OnTriggerExit(Collider other)
    {
        int objLayerNum = other.transform.gameObject.layer;
        if (objLayerNum == 9)
        {
            RemoveFirstInList();
            Destroy(other.gameObject);
            OnRemoveGround?.Invoke();
        }
    }
}
