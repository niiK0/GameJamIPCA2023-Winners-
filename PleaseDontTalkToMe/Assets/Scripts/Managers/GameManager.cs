using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public int currentLevel = 1;

    public float restartCw = 5f;
    public float restartInternalCw;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(restartInternalCw >= 0)
        {
            restartInternalCw -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.R) && restartInternalCw <= 0) RetryLevel();
    }

    private void RetryLevel()
    {
        restartInternalCw = restartCw;
        UI_Manager.instance.UnloadEveryKey();
        GetComponent<LoadingScreen>().LoadScene(currentLevel);
    }
}