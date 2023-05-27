using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;

    public Image[] keyUISlots;
    public int occupiedSlots = 0;
    public int lastOccupiedSlot = -1;
    public Image currentSelectedKey;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    public void AddKeyObjectToUi(KeyObject newKey)
    {
        if (occupiedSlots >= keyUISlots.Length) return;

        Image keySlot = keyUISlots[lastOccupiedSlot + 1];
        RectTransform slotTransform = keySlot.GetComponent<RectTransform>();

        keySlot.sprite = newKey.uiView;
        Color tempColor = keySlot.color;
        keySlot.color = new Color(tempColor.r, tempColor.g, tempColor.b, 1);

        slotTransform.sizeDelta = newKey.resolution;
        keySlot.GetComponent<KeyTileSpawner>().tileToSpawn = newKey.keyTile;

        occupiedSlots++;
        lastOccupiedSlot++;
    }



    //public void RemoveSelectedKey()
    //{
    //    for (int i = 0; i < keyUISlots.Length; i++)
    //    {
    //        if (currentSelectedKey == keyUISlots[i])
    //        {
    //            keyUISlots[i] = null;
    //            currentSelectedKey = null;
    //        }
    //    }
    //}

    public void SelectKey(int slotID)
    {
        currentSelectedKey = keyUISlots[slotID];

        currentSelectedKey.GetComponent<KeyTileSpawner>()._spawnReady = true;

        currentSelectedKey.GetComponent<Animator>().SetBool("Selected", true);

        //Transform keySlot = keyUISlots[slotID].transform;
        //keySlot.GetComponent<KeyTileSpawner>()._spawnReady = true;

        //keySlot.GetComponent<Animator>().SetBool("Selected", true);

    }
}
