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

    private void Start()
    {
        for(int i=0; i<tileLimit; i++)
        {
            currentGround = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity);
            listGround.Add(currentGround);
            nextSpawnPoint = currentGround.transform.GetChild(0).transform.position; //
        }
    }


    private void FixedUpdate()
    {
        Debug.Log(listGround.Count);
        if(listGround.Count < tileLimit)
        {
            int currentListLength = listGround.Count;
            nextSpawnPoint = listGround[currentListLength-1].transform.GetChild(0).transform.position;
            currentGround = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity);
            listGround.Add(currentGround);
            nextSpawnPoint = currentGround.transform.GetChild(0).transform.position;
        }
    }

    public void RemoveLastInList()
    {
        listGround.RemoveAt(0);
    }
}
