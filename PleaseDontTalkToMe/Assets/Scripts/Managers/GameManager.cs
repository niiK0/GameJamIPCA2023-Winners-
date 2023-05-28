using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public int currentLevel = 0;
    public int attemptsCount = 1;

    public float restartCw = 5f;
    public float restartInternalCw;

    private float gameTime;

    public float finalGameTime = 0f;
    public float finalAttempts = 0f;

    public GameObject video;

    private float seconds, minutes;

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
        UI_Manager.instance.gameTimeText.text = "00:00";
        UpdateUI();
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

        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            video.gameObject.SetActive(true);
        }

        Timer();
    }

    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the Unity Editor
        Application.Quit(); // Quit the application in a built standalone executable
    }

    public LevelStartInputs GetCurrentLevelInputInfo()
    {
        return levels[currentLevel].levelInputInfo;
    }

    private void Timer()
    {
        gameTime += Time.deltaTime;
        seconds = (int)(gameTime % 60);
        minutes = (int)(gameTime / 60 % 60);
        UI_Manager.instance.gameTimeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void NextLevel()
    {
        currentLevel++;
        UI_Manager.instance.UnloadEveryKey();
        GetComponent<LoadingScreen>().LoadScene(currentLevel);
    }

    public void RetryLevel()
    {
        attemptsCount++;
        restartInternalCw = restartCw;
        UI_Manager.instance.UnloadEveryKey();
        //Destroy(FindObjectOfType<PlayerMovement>().gameObject);
        GetComponent<LoadingScreen>().LoadScene(currentLevel);
        UpdateUI();
    }

    public void UpdateUI()
    {
        if(UI_Manager.instance.attemptsText == null || UI_Manager.instance.levelText == null) return;
        UI_Manager.instance.attemptsText.text = "Attempt " + attemptsCount.ToString();
        UI_Manager.instance.levelText.text = "<b>Level " + (currentLevel + 1)+ " : </b>" + levels[currentLevel].name;
    }

    public void PlayerDied()
    {
        //maybe do smt else if needed ?
        RetryLevel();
    }

    public void RetryButtonHoverIn(RectTransform imageSize)
    {
        imageSize.sizeDelta = new Vector2(90f, 90f);
    }
    public void RetryButtonHoverOut(RectTransform imageSize)
    {
        imageSize.sizeDelta = new Vector2(80f, 80f);
    }
}

[Serializable]
public struct Level
{
    public int id;
    public string name;
    public LevelStartInputs levelInputInfo;
}