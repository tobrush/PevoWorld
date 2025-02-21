using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DanielLochner.Assets.SimpleSideMenu;

public class TapManager : MonoBehaviour
{
    public SimpleSideMenu simpleSideMenu01, simpleSideMenu02, simpleSideMenu03, simpleSideMenu04, simpleSideMenu05;

    public bool Opened;
    public GameObject SetbarObj;

    public float speed = 10f;

    public Image YellowStick;

    public Toggle slot_1;
    public Toggle slot_2;
    public Toggle slot_3;
    public Toggle slot_4;
    public Toggle slot_5;


    public Toggle slot2_1;
    public Toggle slot2_2;
    public Toggle slot2_3;
    public Toggle slot2_4;

    public Image WorldBtn;

    public CamTarget camTaget;

    void Update()
    {
        if (slot_1.isOn)
        {
            if (0.001f <= Vector3.Distance(YellowStick.rectTransform.localPosition, new Vector3(-392f, YellowStick.rectTransform.localPosition.y, 0)))
            {
                YellowStick.rectTransform.localPosition = Vector3.Lerp(YellowStick.rectTransform.localPosition, new Vector3(-392, YellowStick.rectTransform.localPosition.y, 0), Time.deltaTime * speed);
            }
        }

        if (slot_2.isOn)
        {
            if (0.001f <= Vector3.Distance(YellowStick.rectTransform.localPosition, new Vector3(-196f, YellowStick.rectTransform.localPosition.y, 0)))
            {
                YellowStick.rectTransform.localPosition = Vector3.Lerp(YellowStick.rectTransform.localPosition, new Vector3(-196, YellowStick.rectTransform.localPosition.y, 0), Time.deltaTime * speed);
            }
        }

        if (slot_3.isOn)
        {
            if (0.001f <= Vector3.Distance(YellowStick.rectTransform.localPosition, new Vector3(0f, YellowStick.rectTransform.localPosition.y, 0)))
            {
                YellowStick.rectTransform.localPosition = Vector3.Lerp(YellowStick.rectTransform.localPosition, new Vector3(0, YellowStick.rectTransform.localPosition.y, 0), Time.deltaTime * speed);
            }
        }
        if (slot_4.isOn)
        {
            if (0.001f <= Vector3.Distance(YellowStick.rectTransform.localPosition, new Vector3(196f, YellowStick.rectTransform.localPosition.y, 0)))
            {
                YellowStick.rectTransform.localPosition = Vector3.Lerp(YellowStick.rectTransform.localPosition, new Vector3(196, YellowStick.rectTransform.localPosition.y, 0), Time.deltaTime * speed);
            }
        }

        if (slot_5.isOn)
        {
            if (0.001f <= Vector3.Distance(YellowStick.rectTransform.localPosition, new Vector3(392, YellowStick.rectTransform.localPosition.y, 0)))
            {
                YellowStick.rectTransform.localPosition = Vector3.Lerp(YellowStick.rectTransform.localPosition, new Vector3(392, YellowStick.rectTransform.localPosition.y, 0), Time.deltaTime * speed);
            }
        }
    }

    public void HidePage()
    {
        if(!Opened)
        {
            SetbarObj.SetActive(true);
            Opened = true;
        }
        else
        {
            Opened = false;
            SetbarObj.SetActive(false);
          
            slot_1.GetComponent<Text>().color = Color.gray;
            slot_2.GetComponent<Text>().color = Color.gray;
            slot_3.GetComponent<Text>().color = Color.gray;
            slot_4.GetComponent<Text>().color = Color.gray;
            slot_5.GetComponent<Text>().color = Color.gray;

            WorldBtn.GetComponent<RectTransform>().anchoredPosition = new Vector3(365, 260, 0);

            camTaget.SetTarget01Addon(); //3∏∂∏Æ¿œ∂ß πŸ≤„¡‡æﬂ«‘ 
        }
        //if(¥›«Ù¿÷¥Ÿ∏È?) // setbarµµ setactive false

    }



    public void ColorChange()
    {
        SetbarObj.SetActive(true);
        Opened = true;

        if (slot_1.isOn)
        {
            slot_1.GetComponent<Text>().color = Color.black;
            //WorldBtn.GetComponent<RectTransform>().anchoredPosition = new Vector3(365, 1000, 0);
            camTaget.SetTarget01();   //3∏∂∏Æ¿œ∂ß πŸ≤„¡‡æﬂ«‘ 
        }
        else
        {
            slot_1.GetComponent<Text>().color = Color.gray;
        }

        if (slot_2.isOn)
        {
            slot_2.GetComponent<Text>().color = Color.black;
            //WorldBtn.GetComponent<RectTransform>().anchoredPosition = new Vector3(365, 1000, 0);
            camTaget.SetTarget01();
        }
        else
        {
            slot_2.GetComponent<Text>().color = Color.gray;
        }

        if (slot_3.isOn)
        {
            slot_3.GetComponent<Text>().color = Color.black;
            //WorldBtn.GetComponent<RectTransform>().anchoredPosition = new Vector3(365, 1000, 0);
            camTaget.SetTarget01();
        }
        else
        {
            slot_3.GetComponent<Text>().color = Color.gray;
        }

        if (slot_4.isOn)
        {
            slot_4.GetComponent<Text>().color = Color.black;
            //WorldBtn.GetComponent<RectTransform>().anchoredPosition = new Vector3(365, 1000, 0);
            camTaget.SetTarget01();
        }
        else
        {
            slot_4.GetComponent<Text>().color = Color.gray;
        }

        if (slot_5.isOn)
        {
            slot_5.GetComponent<Text>().color = Color.black;
            //WorldBtn.GetComponent<RectTransform>().anchoredPosition = new Vector3(365, 1000, 0);
            camTaget.SetTarget01();
        }
        else
        {
            slot_5.GetComponent<Text>().color = Color.gray;
        }
    }

    public void ColorChange2()
    {
        if (slot2_1.isOn)
        {
            slot2_1.transform.GetChild(1).GetComponent<Text>().color = Color.white;
        }
        else
        {
            slot2_1.transform.GetChild(1).GetComponent<Text>().color = Color.gray;
        }

        if (slot2_2.isOn)
        {
            slot2_2.transform.GetChild(1).GetComponent<Text>().color = Color.white;
        }
        else
        {
            slot2_2.transform.GetChild(1).GetComponent<Text>().color = Color.gray;
        }

        if (slot2_3.isOn)
        {
            slot2_3.transform.GetChild(1).GetComponent<Text>().color = Color.white;
        }
        else
        {
            slot2_3.transform.GetChild(1).GetComponent<Text>().color = Color.gray;
        }

        if (slot2_4.isOn)
        {
            slot2_4.transform.GetChild(1).GetComponent<Text>().color = Color.white;
        }
        else
        {
            slot2_4.transform.GetChild(1).GetComponent<Text>().color = Color.gray;
        }
    }
}
