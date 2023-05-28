using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHolder : MonoBehaviour
{
    public KeyObject moveRight;
    public KeyObject moveLeft;
    public KeyObject jump;
    public KeyObject throwObject;

    public LevelStartInputs levelInputInfo;

    private void Start()
    {
        levelInputInfo = GameManager.instance.GetCurrentLevelInputInfo();
        LoadLevelInputInfo();
    }

    private void LoadLevelInputInfo()
    {
        foreach (var item in levelInputInfo.startInputs)
        {
            AddInputObject(item);
        }
    }

    public void AddInputObject(InputFunction newInput)
    {
        switch (newInput)
        {
            case InputFunction.Jump:
                UI_Manager.instance.AddKeyObjectToUi(jump);
                jump.inputCount++;
                break;
            case InputFunction.Throw:
                UI_Manager.instance.AddKeyObjectToUi(throwObject);
                throwObject.inputCount++;
                break;
            case InputFunction.MoveLeft:
                UI_Manager.instance.AddKeyObjectToUi(moveLeft);
                moveLeft.inputCount++;
                break;
            case InputFunction.MoveRight:
                UI_Manager.instance.AddKeyObjectToUi(moveRight);
                moveRight.inputCount++;
                break;
            default:
                break;
        }
        
        RefreshAvailability();
    }

    public void RemoveInputObject(KeyObject keyObject)
    {
        InputFunction temp = keyObject.function;

        switch (temp)
        {
            case InputFunction.Jump:
                jump.inputCount--;
                break;
            case InputFunction.Throw:
                throwObject.inputCount--;
                break;
            case InputFunction.MoveLeft:
                moveLeft.inputCount--;
                break;
            case InputFunction.MoveRight:
                moveRight.inputCount--;
                break;
            default:
                break;
        }
        RefreshAvailability();
    }

    public void RefreshAvailability()
    {
        if (moveRight.inputCount <= 0) moveRight.isAvailable = false;
        else moveRight.isAvailable = true;
        if (moveLeft.inputCount <= 0) moveLeft.isAvailable = false;
        else moveLeft.isAvailable = true;
        if (jump.inputCount <= 0) jump.isAvailable = false;
        else jump.isAvailable = true;
        if (throwObject.inputCount <= 0) throwObject.isAvailable = false;
        else throwObject.isAvailable = true;
    }
}

[Serializable]
public struct KeyObject
{
    public KeyCode keyCode;
    public bool isAvailable;
    public InputFunction function;
    public int inputCount;
    public Sprite uiView;
    public Vector2 resolution;
    public GameObject keyTile;
    public bool imNull;
}

public enum InputFunction
{
    Jump, Throw, MoveLeft, MoveRight
}