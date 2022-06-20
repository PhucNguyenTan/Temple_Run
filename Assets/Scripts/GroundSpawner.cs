using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundTile;
    public Vector3 nextSpawnPoint = Vector3.zero;
    public int tileLimit = 5;
    private GameObject currentGround;
    //private GameObject[10] currentGrounds;
    //public GameObject[] arrayGround;

    private List<GameObject> listGround = new List<GameObject>();
    private GroundTile ground;
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

    private void GameManager_OnStateChange(GameManager.GameState state)
    {
        switch (state)
        {
            case GameManager.GameState.CountDown:
                break;
            case GameManager.GameState.Pause:
                break;
            case GameManager.GameState.Run:
                break;
            case GameManager.GameState.End:
                break;
        }
        //throw new System.NotImplementedException();
    }

    private void Start()
    {
        
    }


    private void FixedUpdate()
    {
        
    }

    public void RemoveLastInList()
    {
        listGround.RemoveAt(0);
    }

    public void CreateStartingGrounds() {
        for (int i = 0; i < tileLimit; i++)
        {
            currentGround = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity);
            listGround.Add(currentGround);
            nextSpawnPoint = currentGround.transform.GetChild(0).transform.position; //
        }
    }

    public void CreateNextGround()
    {
        if (listGround.Count < tileLimit)
        {
            int currentListLength = listGround.Count;
            nextSpawnPoint = listGround[currentListLength - 1].transform.GetChild(0).transform.position;
            currentGround = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity);
            listGround.Add(currentGround);
            nextSpawnPoint = currentGround.transform.GetChild(0).transform.position;
        }
    }

    public void PauseScrolling()
    {
        for (int i = 0; i < listGround.Count; i++)
        {
            
        }
    }
}
