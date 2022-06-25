using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GroundTile groundTile;
    public Vector3 nextSpawnPoint = Vector3.zero;
    public int tileLimit = 5;
    private GroundTile currentGround;
    //private GameObject[10] currentGrounds;
    //public GameObject[] arrayGround;

    private int groundCount;

    
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
        if(!isPause)
            CreateNextGround();

    }

    public void RemoveLastInList()
    {
        listGround.RemoveAt(0);
    }

    public void CreateStartingGrounds() {
        for (int i = 0; i < tileLimit; i++)
        {
            currentGround = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity);
            currentGround.name = "Ground_" + groundCount;
            groundCount++;
            listGround.Add(currentGround);
            nextSpawnPoint = currentGround.transform.Find("SpawnPoint").transform.position; 
        }
    }

    public void CreateNextGround()
    {
        if (listGround.Count < tileLimit)
        {
            int currentListLength = listGround.Count;
            nextSpawnPoint = listGround[currentListLength - 1].transform.GetChild(0).transform.position;
            currentGround = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity);
            currentGround.name = "Ground_" + groundCount;
            groundCount++;
            currentGround.UnPause();
            listGround.Add(currentGround);
            nextSpawnPoint = currentGround.transform.GetChild(0).transform.position;
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

    public void Initialize()
    {
        for (int i = 0; i<listGround.Count; i++)
        {
            Debug.Log($"{listGround[i].name} was destroyed");
            Destroy(listGround[i].gameObject);
        }
        listGround.Clear();
        nextSpawnPoint = Vector3.zero;
        groundCount = 0;
        CreateStartingGrounds();
    }
}
