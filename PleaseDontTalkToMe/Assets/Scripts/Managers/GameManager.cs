using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public int currentLevel = 1;
    public int attemptsCount = 1;

    public float restartCw = 5f;
    public float restartInternalCw;

    private float gameTime;

    private float seconds, minutes;
    public TextMeshProUGUI gameTimeText;

    public Level[] levels;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        gameTimeText.text = "00:00";
    }

    // Update is called once per frame
    void Update()
    {
        if(restartInternalCw >= 0)
        {
            restartInternalCw -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.R) && restartInternalCw <= 0) RetryLevel();
        if (Input.GetKeyDown(KeyCode.H))
        {
            currentLevel++;
            UI_Manager.instance.UnloadEveryKey();
            GetComponent<LoadingScreen>().LoadScene(currentLevel);
        }

        Timer();
    }

    public LevelStartInputs GetCurrentLevelInputInfo()
    {
        return levels[currentLevel-1].levelInputInfo;
    }

    private void Timer()
    {
        gameTime += Time.deltaTime;
        seconds = (int)(gameTime % 60);
        minutes = (int)(gameTime / 60 % 60);
        gameTimeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    private void RetryLevel()
    {
        attemptsCount++;
        restartInternalCw = restartCw;
        UI_Manager.instance.UnloadEveryKey();
        GetComponent<LoadingScreen>().LoadScene(currentLevel);
    }
}

[Serializable]
public struct Level
{
    public int id;
    public string name;
    public float maxTime;
    public LevelStartInputs levelInputInfo;
}