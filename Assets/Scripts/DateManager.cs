using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DateManager : MonoBehaviour
{
    public Window_Graph WG_Day;
    public Window_Graph WG_Week;
    public Window_Graph WG_Month;
    public Window_Graph WG_Year;

    public void PreviousBtn()
    {
        if (WG_Day.gameObject.activeSelf)
        {
            WG_Day.PreviousDay();
        }
        if (WG_Week.gameObject.activeSelf)
        {
            WG_Week.PreviousWeek();
        }
        if (WG_Month.gameObject.activeSelf)
        {
            WG_Month.PreviousoMonth();
        }
        if (WG_Year.gameObject.activeSelf)
        {
            WG_Year.PreviousoYear();
        }
    }
    public void NextBtn()
    {
        if (WG_Day.gameObject.activeSelf)
        {
            WG_Day.NextDay();
        }
        if (WG_Week.gameObject.activeSelf)
        {
            WG_Week.NextWeek();
        }
        if (WG_Month.gameObject.activeSelf)
        {
            WG_Month.NextMonth();
        }
        if (WG_Year.gameObject.activeSelf)
        {
            WG_Year.NextYear();
        }
    }

}
