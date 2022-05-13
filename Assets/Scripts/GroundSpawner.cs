using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public GameObject groundTile;
    public Vector3 nextSpawnPoint = Vector3.zero;
    public int tileLimit = 5;
    //public GameObject[] arrayGround;
    
    public void SpawnGround()
    {
        GameObject currentGround = Instantiate(groundTile, nextSpawnPoint, Quaternion.identity);
        nextSpawnPoint = currentGround.transform.GetChild(0).transform.position;
    }

    private void Start()
    {
        for(int i = 0; i<tileLimit; i++)
        {
            SpawnGround();
        }
    }
}
