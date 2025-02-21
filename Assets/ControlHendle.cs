using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlHendle : MonoBehaviour
{
    public bool ScrollbarOn;
    public bool barOn;
    public float currentTime;
    public float limitTime = 3;

    public GameObject[] gameobjects;


    public void ScrollbarChangeOn()
    {
        currentTime = 0;
        barOn = true;
        ScrollbarOn = true;

        gameobjects[0].GetComponent<Image>().color = Color.white;
        gameobjects[1].GetComponent<Image>().color = Color.white;
        gameobjects[2].GetComponent<Image>().color = Color.white;
    }

    public void Update()
    {
        if (barOn)
        {
            if (ScrollbarOn)
            {
                if (Input.touchCount == 0)
                {
                    currentTime += Time.fixedDeltaTime;

                    if (currentTime >= limitTime)
                    {

                        StartCoroutine(ScrollbarChangeOff());
                        currentTime = 0;
                        ScrollbarOn = false;
                    }
                }
            }
        }
    }

    IEnumerator ScrollbarChangeOff()
    {
        for (float t = 0f; t <= 1f; t += Time.fixedDeltaTime)
        {
            Color color = this.GetComponent<Image>().color;
            color.a = Mathf.Lerp(1, 0, t);
            gameobjects[0].GetComponent<Image>().color = color;
            gameobjects[1].GetComponent<Image>().color = color;
            gameobjects[2].GetComponent<Image>().color = color;

            yield return null;
        }
        barOn = false ;
    }
}
