using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    private float gameTime;
    private float seconds, minutes;
    public TextMeshProUGUI gameTimeText;


    // Start is called before the first frame update
    void Start()
    {
        gameTimeText.text = "00:00";
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;
        seconds = (int)(gameTime % 60);
        minutes = (int)(gameTime / 60 % 60);
        gameTimeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }
}
