using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyTileSpawner : MonoBehaviour
{
    public GameObject tileToSpawn;
    public GameObject spawnVFX;
    public bool _spawnReady = false;

    public void SpawnTile()
    {
        Vector3 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spawnPos += Vector3.forward * 10;

        var tile = Instantiate(tileToSpawn, spawnPos, Quaternion.identity);
        //tile.GetComponent<Animation>().Play();
        var vfx = Instantiate(spawnVFX, spawnPos, Quaternion.identity);

        //UI_Manager.instance.RemoveSelectedKey();
        Destroy(vfx, 3.0f);
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && _spawnReady) SpawnTile();
    }
}
