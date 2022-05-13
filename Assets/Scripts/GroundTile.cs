using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : MonoBehaviour
{
    GroundSpawner groundSpawner;

    private void OnTriggerExit(Collider other)
    {
        groundSpawner.SpawnGround();
        Destroy(gameObject, 2f);
    }

    private void Start()
    {
        groundSpawner = GameObject.FindObjectOfType<GroundSpawner>();
    }
}
