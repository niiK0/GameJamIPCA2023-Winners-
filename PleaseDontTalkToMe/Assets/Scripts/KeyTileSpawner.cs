using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class KeyTileSpawner : MonoBehaviour
{
    public GameObject tileToSpawn;
    public GameObject spawnVFX;
    public bool _spawnReady = false;
    public int spawnerID;

    [Header("SpawnLimits")]
    public LayerMask spawnerLayer;
    public Rect exclusionRect;
    public float screenHeighNullPercentage = .25f;

    public void SpawnTile()
    {
        //if (!ClickedSpawner()) return;
        if(EventSystem.current.IsPointerOverGameObject()) return;

        Vector3 spawnPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spawnPos += Vector3.forward * 10;

        var tile = Instantiate(tileToSpawn, spawnPos, Quaternion.identity);
        //tile.GetComponent<Animation>().Play();
        var vfx = Instantiate(spawnVFX, spawnPos, Quaternion.identity);

        UI_Manager.instance.RemoveKeyObject(spawnerID);
        _spawnReady = false;
        Destroy(vfx, 3.0f);
    }

    private bool ClickedSpawner()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, spawnerLayer))
        {
            Debug.Log(hit.collider.name);
            return true;
        }
        else
        {
            Debug.Log("Hit Nothing");
            return false;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && _spawnReady) SpawnTile();
    }

    private void OnDrawGizmos()
    {
    }
}
