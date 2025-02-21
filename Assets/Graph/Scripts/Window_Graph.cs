using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;
using TMPro;

public class Window_Graph : MonoBehaviour {

    public GameObject DayNumCountObj;
    public GameObject WeekNumCountObj;
    public GameObject MonthNumCountObj;
    public GameObject Num31;
    public GameObject Num30;
    public GameObject Num29;
    public GameObject YearNumCountObj;

    //public Text DisplayText;
    public String DisplayText;

    public Text dayText;

    public String SearchDay;

    [SerializeField] private Sprite circleSprite;
    [SerializeField] private Sprite stickSprite;

    public RectTransform graphContainer;
    public RectTransform stickContainer;

   // public List<int> valueList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 0, 20, 36, 45, 30, 22, 17, 0, 13, 17, 25, 0 };
   // public List<int> stickList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 10, 50, 30, 20, 0, 10, 10, 0, 0, 0 };
    public List<int> valueList;
    public List<int> stickList;

    public float xSize = 30f;
    public float yMaxSize = 30f;
    // day = 35f, valueCount = 24
    // week = 120f, valueCount = 7
    // month = 28f, valueCount = 31
    // year = 70f,  valueCount = 12

    public bool StickView;
    public int ViewDay = 0;


    public TMP_Text YmaxText;
    public TMP_Text YhalfText;

    private void Awake()
    {
        DisplayDay();
    }

    private void Start()
    {
        GraphReset();
    }
    public void GraphReset() {

        if(StickView)
        {
            ShowStick(stickList);
        }
        else
        {
            ShowGraph(valueList);
        }

        
    }



    private GameObject CreateCircle(Vector2 anchoredPosition) {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    public void DisplayDay()
    {
        ViewDay = 0;
        DisplayText = "오늘";

        dayText.text = System.DateTime.Now.ToString("yyyy.MM.dd") + " (" + DisplayText + ")";

    }

    public void PreviousDay()
    {
        
        ViewDay = ViewDay - 1;
        DateTime yesterday = DateTime.Today.AddDays(ViewDay);

        

        if (ViewDay == 0 )
        {
            DisplayText = "오늘";
        }
        if (ViewDay == -1)
        {
            DisplayText = "어제";
        }
        if (ViewDay < -1)
        {
            DisplayText = "이전";
        }

        dayText.text = yesterday.ToString("yyyy.MM.dd") + " (" + DisplayText + ")";
        SearchDay = yesterday.ToString("yyyy-MM-dd");
        // yesterday의 데이터(12개의 숫자(0~60)리스트)+어제총활동분+오늘총활동분(어제대비는 비교연산) 서버에서 호출 -> 받아오면 GraphReset();
    }

    public void NextDay()
    {
        
        ViewDay = ViewDay + 1;
        DateTime tomorrow = DateTime.Today.AddDays(ViewDay);

        

        if (DateTime.Now >= tomorrow)
        {
            dayText.text = tomorrow.ToString("yyyy.MM.dd") + " (" + DisplayText + ")";
            SearchDay = tomorrow.ToString("yyyy-MM-dd");
        }
        else
        {
            ViewDay = ViewDay - 1;
            tomorrow = DateTime.Today.AddDays(ViewDay);
            SearchDay = tomorrow.ToString("yyyy-MM-dd");
        }

        

        if (ViewDay == 0)
        {
            DisplayText = "오늘";
            dayText.text = tomorrow.ToString("yyyy.MM.dd") + " (" + DisplayText + ")";
        }
        if (ViewDay == -1)
        {
            DisplayText= "어제";
            dayText.text = tomorrow.ToString("yyyy.MM.dd") + " (" + DisplayText + ")";
 
        }
        if (ViewDay < -1)
        {
            DisplayText= "이전";
            dayText.text = tomorrow.ToString("yyyy.MM.dd") + " (" + DisplayText + ")";
        }


        

    }


    public void ViewClear()
    {
        ViewDay = 0;
    }

    public void DisplayWeek()
    {
        
        DateTime nowDt = DateTime.Now;


        DisplayText= "주간 평균";


    

        if (nowDt.DayOfWeek == DayOfWeek.Monday)
        {
            int WeekStart = ViewDay - 1;
            int WeekEnd = ViewDay + 5;
            DateTime WeekStartDay = DateTime.Today.AddDays(WeekStart);
            DateTime WeekEndDay = DateTime.Today.AddDays(WeekEnd);
            dayText.text = WeekStartDay.ToString("yyyy.MM.dd") + " ~ " + WeekEndDay.ToString("yyyy.MM.dd") + " (" + DisplayText + ")";
            // peve data 호출 = WeekEnd ~ WeekEndDay 사이 데이터  -> 받아오면 GraphReset();
            SearchDay = WeekStartDay.ToString("yyyy-MM-dd");
        }

        else if (nowDt.DayOfWeek == DayOfWeek.Tuesday)
        {
            int WeekStart = ViewDay - 2;
            int WeekEnd = ViewDay + 4;
            DateTime WeekStartDay = DateTime.Today.AddDays(WeekStart);
            DateTime WeekEndDay = DateTime.Today.AddDays(WeekEnd);
            dayText.text = WeekStartDay.ToString("yyyy.MM.dd") + " ~ " + WeekEndDay.ToString("yyyy.MM.dd") + " (" + DisplayText + ")";
            SearchDay = WeekStartDay.ToString("yyyy-MM-dd");
        }

        else if (nowDt.DayOfWeek == DayOfWeek.Wednesday)
        {
            int WeekStart = ViewDay - 3;
            int WeekEnd = ViewDay + 3;
            DateTime WeekStartDay = DateTime.Today.AddDays(WeekStart);
            DateTime WeekEndDay = DateTime.Today.AddDays(WeekEnd);
            dayText.text = WeekStartDay.ToString("yyyy.MM.dd") + " ~ " + WeekEndDay.ToString("yyyy.MM.dd") + " (" + DisplayText + ")";
            SearchDay = WeekStartDay.ToString("yyyy-MM-dd");
        }

        else if (nowDt.DayOfWeek == DayOfWeek.Thursday)
        {
            int WeekStart = ViewDay - 4;
            int WeekEnd = ViewDay + 2;
            DateTime WeekStartDay = DateTime.Today.AddDays(WeekStart);
            DateTime WeekEndDay = DateTime.Today.AddDays(WeekEnd);
            dayText.text = WeekStartDay.ToString("yyyy.MM.dd") + " ~ " + WeekEndDay.ToString("yyyy.MM.dd") + " (" + DisplayText + ")";
            SearchDay = WeekStartDay.ToString("yyyy-MM-dd");
        }

        else if (nowDt.DayOfWeek == DayOfWeek.Friday)
        {
            int WeekStart = ViewDay - 5;
            int WeekEnd = ViewDay + 1;
            DateTime WeekStartDay = DateTime.Today.AddDays(WeekStart);
            DateTime WeekEndDay = DateTime.Today.AddDays(WeekEnd);
            dayText.text = WeekStartDay.ToString("yyyy.MM.dd") + " ~ " + WeekEndDay.ToString("yyyy.MM.dd") + " (" + DisplayText + ")";
            SearchDay = WeekStartDay.ToString("yyyy-MM-dd");
        }

        else if (nowDt.DayOfWeek == DayOfWeek.Saturday)
        {
            int WeekStart = ViewDay - 6;
            int WeekEnd = ViewDay + 0;
            DateTime WeekStartDay = DateTime.Today.AddDays(WeekStart);
            DateTime WeekEndDay = DateTime.Today.AddDays(WeekEnd);
            dayText.text = WeekStartDay.ToString("yyyy.MM.dd") + " ~ " + WeekEndDay.ToString("yyyy.MM.dd") + " (" + DisplayText + ")";
            SearchDay = WeekStartDay.ToString("yyyy-MM-dd");
        }

        else if (nowDt.DayOfWeek == DayOfWeek.Sunday)
        {
            int WeekStart = ViewDay - 0;
            int WeekEnd = ViewDay + 6;
            DateTime WeekStartDay = DateTime.Today.AddDays(WeekStart);
            DateTime WeekEndDay = DateTime.Today.AddDays(WeekEnd);
            dayText.text = WeekStartDay.ToString("yyyy.MM.dd") + " ~ " + WeekEndDay.ToString("yyyy.MM.dd") + " (" + DisplayText + ")";
            SearchDay = WeekStartDay.ToString("yyyy-MM-dd");
        }
       

    }


    public void PreviousWeek()
    {
        ViewDay = ViewDay - 7;

        DisplayWeek();
    }


    public void NextWeek()
    {
        ViewDay = ViewDay + 7;
        
        DateTime NextWeekDay = DateTime.Today.AddDays(ViewDay);
        
        if (DateTime.Now >= NextWeekDay)
        {
     
            DisplayWeek();
        }
        else
        {
            ViewDay = ViewDay - 7;
            NextWeekDay = DateTime.Today.AddDays(ViewDay);
            SearchDay = NextWeekDay.ToString("yyyy-MM-dd");
        }
    

    }


    public void DisplayMonth()
    {

        DateTime nowDt = DateTime.Now;

        int MonthDay = DateTime.DaysInMonth(nowDt.Year, nowDt.Month);
        if(MonthDay > valueList.Count)
        {
            int AddDay = MonthDay - valueList.Count;

            for(int i = 0; i < AddDay; i++)
            {
                valueList.Add(0);
                stickList.Add(0);
            }

        } 
        else if(MonthDay < valueList.Count)
        {
            int MinusDay = valueList.Count - MonthDay;

            for (int i = 0; i < MinusDay; i++)
            {
                valueList.RemoveAt(valueList.Count - 1);
                stickList.RemoveAt(valueList.Count - 1);
            }

            
        }
        GraphReset();
        ResetMonthDay(MonthDay);


        DisplayText = "월간 평균";

        dayText.text = System.DateTime.Now.ToString("yyyy"+"년 " +"MM" + "월") + " (" + DisplayText + ")";

        SearchDay = nowDt.ToString("yyyy-MM-dd");

    }

    void ResetMonthDay(int MonthDay)
    {

        if (MonthDay == 31)
        {
            Num29.SetActive(true);
            Num30.SetActive(true);
            Num31.SetActive(true);
            MonthNumCountObj.GetComponent<HorizontalLayoutGroup>().padding.left = 17;
            MonthNumCountObj.GetComponent<HorizontalLayoutGroup>().padding.right = 17;
        }
        else if (MonthDay == 30)
        {
            Num29.SetActive(true);
            Num30.SetActive(true);
            Num31.SetActive(false);
            MonthNumCountObj.GetComponent<HorizontalLayoutGroup>().padding.left = 31;
            MonthNumCountObj.GetComponent<HorizontalLayoutGroup>().padding.right = 31;
        }
        else if (MonthDay == 29)
        {
            Num29.SetActive(true);
            Num30.SetActive(false);
            Num31.SetActive(false);
            MonthNumCountObj.GetComponent<HorizontalLayoutGroup>().padding.left = 45;
            MonthNumCountObj.GetComponent<HorizontalLayoutGroup>().padding.right = 45;
        }
        else if (MonthDay == 28)
        {
            Num29.SetActive(false);
            Num30.SetActive(false);
            Num31.SetActive(false);
            MonthNumCountObj.GetComponent<HorizontalLayoutGroup>().padding.left = 58;
            MonthNumCountObj.GetComponent<HorizontalLayoutGroup>().padding.right = 58;
        }

    }

    public void PreviousoMonth()
    {

           ViewDay = ViewDay - 1;
        DateTime lastMonth = DateTime.Today.AddMonths(ViewDay);
        dayText.text = lastMonth.ToString("yyyy" + "년 " + "MM" + "월") + " (" + DisplayText + ")";
        SearchDay = lastMonth.ToString("yyyy-MM-dd");

        int MonthDay = DateTime.DaysInMonth(lastMonth.Year, lastMonth.Month);
        if(MonthDay > valueList.Count)
        {
            int AddDay = MonthDay - valueList.Count;

            for(int i = 0; i < AddDay; i++)
            {
                valueList.Add(0);
                stickList.Add(0);
            }
        } 
        else if(MonthDay < valueList.Count)
        {
            int MinusDay = valueList.Count - MonthDay;

            for (int i = 0; i < MinusDay; i++)
            {
                valueList.RemoveAt(valueList.Count - 1);
                stickList.RemoveAt(valueList.Count - 1);
            }
        }
        GraphReset();
        ResetMonthDay(MonthDay);

       
    }


    public void NextMonth()
    {
        ViewDay = ViewDay + 1;
        DateTime nextMonth = DateTime.Today.AddMonths(ViewDay);

        if (DateTime.Now >= nextMonth)
        {
            dayText.text = nextMonth.ToString("yyyy" + "년 " + "MM" + "월") + " (" + DisplayText + ")";
            SearchDay = nextMonth.ToString("yyyy-MM-dd");
            int MonthDay = DateTime.DaysInMonth(nextMonth.Year, nextMonth.Month);
            if (MonthDay > valueList.Count)
            {
                int AddDay = MonthDay - valueList.Count;

                for (int i = 0; i < AddDay; i++)
                {
                    valueList.Add(0);
                    stickList.Add(0);
                  
                }
            }
            else if (MonthDay < valueList.Count)
            {
                int MinusDay = valueList.Count - MonthDay;

                for (int i = 0; i < MinusDay; i++)
                {
                    valueList.RemoveAt(valueList.Count - 1);
                    stickList.RemoveAt(valueList.Count - 1);
                }
            }
            GraphReset();
            ResetMonthDay(MonthDay);
        }
        else
        {
            ViewDay = ViewDay - 1;
            nextMonth = DateTime.Today.AddMonths(ViewDay);
            SearchDay = nextMonth.ToString("yyyy-MM-dd");
        }

                 

    }

    public void DisplayYear()
    {
        DateTime nowDt = DateTime.Now;

        DisplayText = "연간 평균";

        dayText.text = System.DateTime.Now.ToString("yyyy" + "년") + " (" + DisplayText + ")";

        SearchDay = nowDt.ToString("yyyy-MM-dd");
    }

    public void PreviousoYear()
    {
        ViewDay = ViewDay - 1;
        DateTime lastYear = DateTime.Today.AddYears(ViewDay);
        dayText.text = lastYear.ToString("yyyy" + "년") + " (" + DisplayText + ")";

         SearchDay = lastYear.ToString("yyyy-MM-dd");

    }

    public void NextYear()
    {
        ViewDay = ViewDay + 1;
        DateTime nextYear = DateTime.Today.AddYears(ViewDay);

        if (DateTime.Now >= nextYear)
        {
            dayText.text = nextYear.ToString("yyyy" + "년") + " (" + DisplayText + ")";
            SearchDay = nextYear.ToString("yyyy-MM-dd");
        }
        else
        {
            ViewDay = ViewDay - 1;
            nextYear = DateTime.Today.AddYears(ViewDay);
            SearchDay = nextYear.ToString("yyyy-MM-dd");

        }
        
    }

    private GameObject CreateStick(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("stick", typeof(Image));
        gameObject.transform.SetParent(stickContainer, false);
        gameObject.GetComponent<Image>().sprite = stickSprite;
       

        gameObject.GetComponent<Image>().type = Image.Type.Sliced;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(anchoredPosition.x , -5);
        rectTransform.sizeDelta = new Vector2(13, anchoredPosition.y);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(0.5f, 0);
        
        if(rectTransform.sizeDelta.y == 12)
        {
            gameObject.GetComponent<Image>().color = new Color(0, 0, 0, 0.2f);
        }

            return gameObject;
           
    }

    private void ShowStick(List<int> stickList)
    {
        for (int i = 0; i < stickContainer.childCount; i++)
        {
            Destroy(stickContainer.GetChild(i).gameObject);
        }


       int maximum = 60;

        for(int i =0; i< stickList.Count; i++)
        {
            if(maximum < stickList[i])
            {
                maximum = stickList[i];
            }
        }

        float graphHeight = stickContainer.sizeDelta.y;
        float yMaximum = maximum;

        YmaxText.text = (yMaximum).ToString("N0") + "m -";
        YhalfText.text = (yMaximum * 0.5f).ToString("N0") + "m -";


        float ShowWidth = xSize * stickList.Count;
        stickContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(ShowWidth, 200);
        stickContainer.GetComponent<RectTransform>().localPosition = new Vector3 (- xSize/2 , 10, 0);

        GameObject lastStickGameObject = null;

        for (int i = 0; i < stickList.Count; i++)
        {
            
            float xPosition = xSize + i * xSize;
            float ySize = 12+((stickList[i] / yMaximum)* graphHeight);
           
            
            GameObject StickGameObject = CreateStick(new Vector2(xPosition, ySize));

            lastStickGameObject = StickGameObject;

            

        }
    }
    private void ShowGraph(List<int> valueList) {

        for (int i = 0; i < graphContainer.childCount; i++)
        {
            Destroy(graphContainer.GetChild(i).gameObject);
        }


        float graphHeight = graphContainer.sizeDelta.y;
        float yMaximum = 60f;

        float ShowWidth = xSize * valueList.Count;
        graphContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(ShowWidth, 200);
        graphContainer.GetComponent<RectTransform>().localPosition = new Vector3(-xSize / 2, 10, 0);


        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++) {
            float xPosition = xSize + i * xSize;
            float yPosition = (valueList[i] / yMaximum) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            if (lastCircleGameObject != null) {
                CreateDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;
        }
    }

    private void CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB) {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = Color.black; //new Color(1,1,1, 0.5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
    }

}
