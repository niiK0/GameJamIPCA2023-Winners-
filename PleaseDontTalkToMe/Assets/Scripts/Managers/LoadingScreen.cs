using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingScreen;
    public Image loadingBarFill;
    public float speed;
    public BlackScreenFader blackScreen;

    public void LoadScene(int sceneId)
    {
        blackScreen.gameObject.SetActive(true);
        blackScreen.TriggerFadeOut();
        StartCoroutine(WaitForFadeOut(sceneId));
    }

    IEnumerator WaitForFadeOut(int sceneId)
    {
        yield return new WaitForSeconds(1f);

        SceneManager.LoadSceneAsync(sceneId);

        blackScreen.gameObject.SetActive(true);
        blackScreen.TriggerFadeIn();

        yield return new WaitForSeconds(1f);

        blackScreen.gameObject.SetActive(false);

        //StartCoroutine(LoadSceneAsync(sceneId));
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / speed);
            loadingBarFill.fillAmount = progressValue;

            yield return null;
        }

        loadingScreen.SetActive(false);
        blackScreen.TriggerFadeOut();
    }
}