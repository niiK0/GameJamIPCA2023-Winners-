using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyTileSpawner : MonoBehaviour
{
    public GameObject tileToSpawn;
    public bool _spawnReady = false;

    public void SpawnTile()
    {
        Vector3 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spawnPos += Vector3.forward * 10;

        Instantiate(tileToSpawn, spawnPos, Quaternion.identity);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && _spawnReady) SpawnTile();
    }
}
