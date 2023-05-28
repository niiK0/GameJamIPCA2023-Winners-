using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager instance;

    public Image[] keyUISlots;
    public KeyObject[] keyObjects;
    public int occupiedSlots = 0;
    public int lastOccupiedSlot = -1;
    public Image currentSelectedKey;

    public TextMeshProUGUI phoneCharges;

    private InputHolder playerInputHolder;

    private void Awake()
    {

        if (instance != null) Destroy(gameObject);
        else instance = this;
        DontDestroyOnLoad(gameObject);

        keyObjects = new KeyObject[keyUISlots.Length];
        playerInputHolder = FindObjectOfType<InputHolder>();
    }

    public void AddKeyObjectToUi(KeyObject newKey)
    {
        if (occupiedSlots >= keyUISlots.Length) return;

        keyObjects[occupiedSlots] = newKey;
        UpdateUIView();
        occupiedSlots++;
    }

    //private void UpdateUIView()
    //{
    //    keyUISlots[occupiedSlots].enabled = true;
    //    keyUISlots[occupiedSlots].sprite = keyObjects[occupiedSlots].uiView;
    //    keyUISlots[occupiedSlots].transform.GetComponent<KeyTileSpawner>().tileToSpawn = keyObjects[occupiedSlots].keyTile;
    //    keyUISlots[occupiedSlots].transform.GetComponent<KeyTileSpawner>().spawnerID = occupiedSlots;
    //    keyUISlots[occupiedSlots].transform.GetComponent<RectTransform>().sizeDelta = keyObjects[occupiedSlots].resolution;
    //}

    private void UpdateUIView()
    {
        for (int i = 0; i < keyUISlots.Length; i++)
        {
            if (keyObjects[i].imNull) keyUISlots[i].enabled = false;
            else
            {
                keyUISlots[i].enabled = true;
                keyUISlots[i].sprite = keyObjects[i].uiView;
                keyUISlots[i].transform.GetComponent<KeyTileSpawner>().tileToSpawn = keyObjects[i].keyTile;
                keyUISlots[i].transform.GetComponent<KeyTileSpawner>().spawnerID = i;
                keyUISlots[i].transform.GetComponent<RectTransform>().sizeDelta = keyObjects[i].resolution;
            }
        }
    }

    public void UnloadEveryKey()
    {
        for (int i = 0; i < keyObjects.Length; i++)
        {
            if (keyObjects[i].imNull) continue;

            playerInputHolder.RemoveInputObject(keyObjects[i]);
            keyObjects[i].imNull = true;
            keyUISlots[i].enabled = false;
            SendNullsToTheEnd();
        }

        occupiedSlots = 0;
        phoneCharges.text = 0.ToString();
        UpdateUIView();
    }

    public void RemoveKeyObject(int keyToRemove)
    {
        occupiedSlots--;
        RemoveSelectedKey();
        playerInputHolder.RemoveInputObject(keyObjects[keyToRemove]);
        keyObjects[keyToRemove].imNull = true;
        keyUISlots[keyToRemove].enabled = false;
        SendNullsToTheEnd();
        UpdateUIView();
    }

    private void SendNullsToTheEnd()
    {
        int insertIndex = 0;

        for (int i = 0; i < keyObjects.Length; i++)
        {
            if (!keyObjects[i].imNull)
            {
                // Move non-null elements to the front
                keyObjects[insertIndex] = keyObjects[i];
                insertIndex++;
            }
        }

        for (int i = insertIndex; i < keyObjects.Length; i++)
        {
            // Set elements with imNull to true at the end
            keyObjects[i] = new KeyObject() { imNull = true };
        }
    }

    public void ReorderArrays()
    {
        Image[] rearangedSlots = new Image[keyUISlots.Length];
        KeyObject[] rearangedKeys = new KeyObject[keyUISlots.Length];

        List<Image> nullImages = new List<Image>();
        List<KeyObject> nullKeys = new List<KeyObject>();
        int successfulIndex = 0;

        //Fill with the ones still available
        for (int i = 0; i < keyUISlots.Length; i++)
        {
            if (!keyObjects[i].imNull)
            {
                rearangedKeys[successfulIndex] = keyObjects[i];
                rearangedSlots[successfulIndex] = keyUISlots[i];
                successfulIndex++;
            }
            else
            {
                nullKeys.Add(keyObjects[i]);
                nullImages.Add(keyUISlots[i]);
            }
        }

        //Now position the empties
        KeyObject[] extraKeys = nullKeys.ToArray();
        Image[] extraImages = nullImages.ToArray();

        keyObjects = rearangedKeys.Concat(nullKeys).ToArray();
        keyUISlots = rearangedSlots.Concat(nullImages).ToArray();

        UpdateUIView();
        occupiedSlots = successfulIndex;
    }


    //public void AddKeyObjectToUi(KeyObject newKey)
    //{
    //    if (occupiedSlots >= keyUISlots.Length) return;

    //    Image keySlot = keyUISlots[lastOccupiedSlot + 1];
    //    RectTransform slotTransform = keySlot.GetComponent<RectTransform>();

    //    keySlot.sprite = newKey.uiView;
    //    Color tempColor = keySlot.color;
    //    keySlot.color = new Color(tempColor.r, tempColor.g, tempColor.b, 1);

    //    slotTransform.sizeDelta = newKey.resolution;
    //    keySlot.GetComponent<KeyTileSpawner>().tileToSpawn = newKey.keyTile;

    //    occupiedSlots++;
    //    lastOccupiedSlot++;
    //}



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
        //Deselect current selected key if != null
        if (currentSelectedKey != null) RemoveSelectedKey();

        currentSelectedKey = keyUISlots[slotID];

        currentSelectedKey.GetComponent<KeyTileSpawner>()._spawnReady = true;

        currentSelectedKey.GetComponent<Animator>().SetBool("Selected", true);

        //Transform keySlot = keyUISlots[slotID].transform;
        //keySlot.GetComponent<KeyTileSpawner>()._spawnReady = true;

        //keySlot.GetComponent<Animator>().SetBool("Selected", true);

    }

    public void RemoveSelectedKey()
    {
        if (currentSelectedKey == null) return;
        currentSelectedKey.GetComponent<KeyTileSpawner>()._spawnReady = false;
        currentSelectedKey.GetComponent<Animator>().SetBool("Selected", false);
        currentSelectedKey = null;
    }
}
