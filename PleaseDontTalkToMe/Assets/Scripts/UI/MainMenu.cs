using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject OptionsPanel;
    [SerializeField] GameObject MainMenuPanel;
    public GameObject gameManager;

    private bool InOptions = false;


    Resolution[] resolutions;

    private void Start()
    {
        OptionsPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (InOptions)
            {
                OptionsPanel.SetActive(false);
                MainMenuPanel.SetActive(true);
                InOptions= false;
            }
        }
    }

    //Settings
    public void QuitSettings()
    {
        OptionsPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

    public void SetVolume(float volume)
    {
        Debug.Log(volume);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    //Main Menu
    public void PlayGame()
    {
        gameManager.GetComponent<LoadingScreen>().LoadScene(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OptiosMenu()
    {
        OptionsPanel.SetActive(true);
        MainMenuPanel.SetActive(false);
        InOptions = true;
    }
}
