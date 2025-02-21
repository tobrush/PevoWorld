using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    bool ChangeScreenMode;
    
    
    public void ChangeScreen()
    {
        if(!ChangeScreenMode)
        {
            ChangeScreenMode = true;
            HorizontalScreen();
        }
        else
        {
            ChangeScreenMode = false;
            VerticalScreen();
        }
    }

    public void HorizontalScreen() //가로
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    public void VerticalScreen() //세로
    {
        Screen.orientation = ScreenOrientation.Portrait;

    }
}
