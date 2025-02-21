using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Michsky.UI.ModernUIPack;

public class DummyStarter : MonoBehaviour
{
    public ProgressBar myBar;
    public GameObject LoginPanel;
    public GameObject ProgressPanel;

    public GameObject TosWindow;



    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f);

        if (!PlayerPrefs.HasKey("Tos"))
        {
            TosWindow.SetActive(true);
            ProgressPanel.SetActive(false);
        }
        else
        {
            TosWindow.SetActive(false);
            ProgressPanel.SetActive(true);
        }


    }
    public void TosAgree()
    {
        PlayerPrefs.SetString("Tos", "Tos");
        TosWindow.SetActive(false);
        ProgressPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(myBar.currentPercent == 100)
        {
            LoginPanel.SetActive(true);
            ProgressPanel.SetActive(false);
        }
    }
}
