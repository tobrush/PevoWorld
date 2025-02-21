using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FramerateCounter : MonoBehaviour
{
    public TMP_Text fpsMeter;

    private float updateTime = .5f;
    private float nextUpdate = 0;
    private float frames = 0;


    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void Update()
    {
        nextUpdate -= Time.deltaTime;
        frames++;

        if(nextUpdate <= 0)
        {
            fpsMeter.text = ((int)frames * (1 / updateTime)) + " FPS";
            frames = 0;
            nextUpdate = updateTime;
        }
    }
}
