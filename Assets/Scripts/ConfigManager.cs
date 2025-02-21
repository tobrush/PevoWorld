using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
//using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using TMPro;

public class ConfigManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    //public PostProcessVolume ppv;

    public TMP_Text myID;
    public TMP_Text MyName;

    public GameObject ViewBloomSlider;
    public Image GraphicsWindow;

    // ???????? : PlayerPrefs.DeleteAll();

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);

        
    }

   public void SetQuality (int qulityIndex)
    {
        QualitySettings.SetQualityLevel(qulityIndex);
    }

    public void SetBloom(bool onoff)
    {
       
        //ppv.isGlobal = onoff;
        if(!onoff)
        {
            GraphicsWindow.rectTransform.sizeDelta = new Vector2(1050, 388f);
            ViewBloomSlider.SetActive(false);
            ViewBloomSlider.GetComponent<Slider>().value = 0f;
        }
        else
        {
            GraphicsWindow.rectTransform.sizeDelta = new Vector2(1050, 498f);
            ViewBloomSlider.SetActive(true);
            ViewBloomSlider.GetComponent<Slider>().value = 0.5f;
        }
    }

    public void SetBloomQuality(float weightAmount)
    {
        //ppv.weight = weightAmount;
    }
}
