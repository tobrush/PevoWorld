using Newtonsoft.Json;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PevoDataManager1 : MonoBehaviour
{
    public Window_Graph Dog1_graph;
    public Window_Graph Dog2_graph;
    public Window_Graph Dog3_graph;

    public int[] Now_act0_Graph; // 무활동
    public int[] Now_act1_Graph; // 휴식
    public int[] Now_act2_Graph; // 가벼운 활동
    public int[] Now_act3_Graph; // 활발한 활동
    public int[] Now_actAll_Graph;

    public int[] Prev_act0_Graph; // 무활동
    public int[] Prev_act1_Graph; // 휴식
    public int[] Prev_act2_Graph; // 가벼운 활동
    public int[] Prev_act3_Graph; // 활발한 활동

    public float TodayAllAct;
    public float PrevAllAct;
    public float CompareAct;
    public string CompareMore;

    public Text TodayAllText;
    public Text PrevAllText;
    public Text CompareText;


    public string Serch_pet_id;
    public string DateType; // D : 일, W : 주, M : 월, Y : 년
    public string NowTime; // yyyy-MM-dd 00:00:00 

    public void StartScene()
    {
        Day_ActData(1);
        Dog1_graph.DisplayDay();

        if (OtherUserData.instance.OhterUser_DogCount > 1)
        {
            Day_ActData(2);
            Dog2_graph.DisplayDay();
        }
        if (OtherUserData.instance.OhterUser_DogCount > 2)
        {
            Day_ActData(3);
            Dog3_graph.DisplayDay();
        }
    }

    public void NextBtn(int DogNumber)
    {
        if (DogNumber == 1)
        {
            if (DateType == "D")
            {
                Dog1_graph.NextDay();
            }
            if (DateType == "W")
            {
                Dog1_graph.NextWeek();
            }
            if (DateType == "M")
            {
                Dog1_graph.NextMonth();
            }
            if (DateType == "Y")
            {
                Dog1_graph.NextYear();
            }

            NowTime = Dog1_graph.SearchDay + " 00:00:00"; // 일단 오늘날짜 가져오기
            print(NowTime);
            StartCoroutine(ActData_Coroutine());
        }
        if (DogNumber == 2)
        {
            if (DateType == "D")
            {
                Dog2_graph.NextDay();
            }
            if (DateType == "W")
            {
                Dog2_graph.NextWeek();
            }
            if (DateType == "M")
            {
                Dog2_graph.NextMonth();
            }
            if (DateType == "Y")
            {
                Dog2_graph.NextYear();
            }

            NowTime = Dog2_graph.SearchDay + " 00:00:00"; // 일단 오늘날짜 가져오기
            print(NowTime);
            StartCoroutine(ActData_Coroutine());
        }
        if (DogNumber == 3)
        {
            if (DateType == "D")
            {
                Dog3_graph.NextDay();
            }
            if (DateType == "W")
            {
                Dog3_graph.NextWeek();
            }
            if (DateType == "M")
            {
                Dog3_graph.NextMonth();
            }
            if (DateType == "Y")
            {
                Dog3_graph.NextYear();
            }

            NowTime = Dog3_graph.SearchDay + " 00:00:00"; // 일단 오늘날짜 가져오기
            print(NowTime);
            StartCoroutine(ActData_Coroutine());
        }
    }

    public void PreviousBtn(int DogNumber)
    {
        if (DogNumber == 1)
        {
            if (DateType == "D")
            {
                Dog1_graph.PreviousDay();
            }
            if (DateType == "W")
            {
                Dog1_graph.PreviousWeek();
            }
            if (DateType == "M")
            {
                Dog1_graph.PreviousoMonth();
            }
            if (DateType == "Y")
            {
                Dog1_graph.PreviousoYear();
            }

            NowTime = Dog1_graph.SearchDay + " 00:00:00"; // 일단 오늘날짜 가져오기
            print(NowTime);
            StartCoroutine(ActData_Coroutine());
        }
        if (DogNumber == 2)
        {
            if (DateType == "D")
            {
                Dog2_graph.PreviousDay();
            }
            if (DateType == "W")
            {
                Dog2_graph.PreviousWeek();
            }
            if (DateType == "M")
            {
                Dog2_graph.PreviousoMonth();
            }
            if (DateType == "Y")
            {
                Dog2_graph.PreviousoYear();
            }
            NowTime = Dog2_graph.SearchDay + " 00:00:00"; // 일단 오늘날짜 가져오기
            print(NowTime);
            StartCoroutine(ActData_Coroutine());
        }
        if (DogNumber == 3)
        {
            if (DateType == "D")
            {
                Dog3_graph.PreviousDay();
            }
            if (DateType == "W")
            {
                Dog3_graph.PreviousWeek();
            }
            if (DateType == "M")
            {
                Dog3_graph.PreviousoMonth();
            }
            if (DateType == "Y")
            {
                Dog3_graph.PreviousoYear();
            }
            NowTime = Dog3_graph.SearchDay + " 00:00:00"; // 일단 오늘날짜 가져오기
            print(NowTime);
            StartCoroutine(ActData_Coroutine());
        }

    }


    public void Day_ActData(int DogNumber)
    {
        if(DogNumber == 1)
        {
            Serch_pet_id = OtherUserData.instance.OhterDog1_PevoNumber;
        }
        if (DogNumber == 2)
        {
            Serch_pet_id = OtherUserData.instance.OhterDog2_PevoNumber;
        }
        if (DogNumber == 3)
        {
            Serch_pet_id = OtherUserData.instance.OhterDog3_PevoNumber;
        }

        DateType = "D";
        NowTime = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00"; // 일단 오늘날짜 가져오기

        StartCoroutine(ActData_Coroutine());
    }

    public void Week_ActData(int DogNumber)
    {
        if (DogNumber == 1)
        {
            Serch_pet_id = OtherUserData.instance.OhterDog1_PevoNumber;
        }
        if (DogNumber == 2)
        {
            Serch_pet_id = OtherUserData.instance.OhterDog2_PevoNumber;
        }
        if (DogNumber == 3)
        {
            Serch_pet_id = OtherUserData.instance.OhterDog3_PevoNumber;
        }

        DateType = "W";
        NowTime = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00"; // 일단 오늘날짜 가져오기
        StartCoroutine(ActData_Coroutine());
    }

    public void Month_ActData(int DogNumber)
    {
        if (DogNumber == 1)
        {
            Serch_pet_id = OtherUserData.instance.OhterDog1_PevoNumber;
        }
        if (DogNumber == 2)
        {
            Serch_pet_id = OtherUserData.instance.OhterDog2_PevoNumber;
        }
        if (DogNumber == 3)
        {
            Serch_pet_id = OtherUserData.instance.OhterDog3_PevoNumber;
        }

        DateType = "M";
        NowTime = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00"; // 일단 오늘날짜 가져오기
        StartCoroutine(ActData_Coroutine());
    }

    public void Year_ActData(int DogNumber)
    {
        if (DogNumber == 1)
        {
            Serch_pet_id = OtherUserData.instance.OhterDog1_PevoNumber;
        }
        if (DogNumber == 2)
        {
            Serch_pet_id = OtherUserData.instance.OhterDog2_PevoNumber;
        }
        if (DogNumber == 3)
        {

            Serch_pet_id = OtherUserData.instance.OhterDog3_PevoNumber;


        }

        DateType = "Y";
        NowTime = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00"; // 일단 오늘날짜 가져오기
        StartCoroutine(ActData_Coroutine());
    }


    IEnumerator ActData_Coroutine()
    {
        yield return new WaitForSeconds(0.1f);
        //outputArea.text = "Loading...";

        string uri = "https://api.pevo.care/v4/pevo_world/act_data";
        WWWForm form = new WWWForm();
        form.AddField("pet_id", Serch_pet_id);
        form.AddField("date_type", DateType);
        form.AddField("now_time", NowTime);
        using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                //outputArea.text = request.downloadHandler.text; // 없는걸 가져온 경우
                string data = request.downloadHandler.text;
                Act act = JsonConvert.DeserializeObject<Act>(data);
                //outputArea.text = act.resultMsg;
            }
            else
            {
                //outputArea.text = request.downloadHandler.text; // 옳게 입력한 경우

                string data = request.downloadHandler.text;
                Act act = JsonConvert.DeserializeObject<Act>(data);
                act.pet_id = Serch_pet_id;


                for (int i = 0; i < 31; i++)
                {
                    Now_act0_Graph[i] = 0;
                    Now_act1_Graph[i] = 0;
                    Now_act2_Graph[i] = 0;
                    Now_act3_Graph[i] = 0;
                    Prev_act0_Graph[i] = 0;
                    Prev_act1_Graph[i] = 0;
                    Prev_act2_Graph[i] = 0;
                    Prev_act3_Graph[i] = 0;

                   

                }
                TodayAllAct = 0;
                PrevAllAct = 0;

                for (int i = 0; i < act.result.now_date.Count; i++)
                {

                    if (DateType == "D")
                    {
                        int numberTime = int.Parse(act.result.now_date[i].action_time[11].ToString() + act.result.now_date[i].action_time[12].ToString()); // 24개의 시 중 몇시인지

                        //print(numberTime);
                        Now_act0_Graph[numberTime] = int.Parse(act.result.now_date[i].act0);
                        Now_act1_Graph[numberTime] = int.Parse(act.result.now_date[i].act1);
                        Now_act2_Graph[numberTime] = int.Parse(act.result.now_date[i].act2);
                        Now_act3_Graph[numberTime] = int.Parse(act.result.now_date[i].act3);

                        TodayAllAct = TodayAllAct + int.Parse(act.result.now_date[i].act2) + int.Parse(act.result.now_date[i].act3);
                    }

                    else if (DateType == "W")
                    {
                        DateTime dateTime = Convert.ToDateTime(act.result.now_date[i].action_time.Substring(0, 10)); // yyyy-MM-dd
                        //print(dateTime.DayOfWeek);
                        int numberTime;

                        if (dateTime.DayOfWeek == DayOfWeek.Sunday)
                        {
                            numberTime = 0;
                        }
                        else if (dateTime.DayOfWeek == DayOfWeek.Monday)
                        {
                            numberTime = 1;
                        }
                        else if (dateTime.DayOfWeek == DayOfWeek.Tuesday)
                        {
                            numberTime = 2;
                        }
                        else if (dateTime.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            numberTime = 3;
                        }
                        else if (dateTime.DayOfWeek == DayOfWeek.Thursday)
                        {
                            numberTime = 4;
                        }
                        else if (dateTime.DayOfWeek == DayOfWeek.Friday)
                        {
                            numberTime = 5;
                        }
                        else
                        {
                            numberTime = 6;
                        }

                        Now_act0_Graph[numberTime] = int.Parse(act.result.now_date[i].act0);
                        Now_act1_Graph[numberTime] = int.Parse(act.result.now_date[i].act1);
                        Now_act2_Graph[numberTime] = int.Parse(act.result.now_date[i].act2);
                        Now_act3_Graph[numberTime] = int.Parse(act.result.now_date[i].act3);

                        TodayAllAct = TodayAllAct + int.Parse(act.result.now_date[i].act2) + int.Parse(act.result.now_date[i].act3);

                    }
                    else if (DateType == "M")
                    {
                        int numberTime = int.Parse(act.result.now_date[i].action_time[8].ToString() + act.result.now_date[i].action_time[9].ToString()); // 31개의 일 중 몇일인지
                        //print(numberTime);
                        Now_act0_Graph[numberTime -1] = int.Parse(act.result.now_date[i].act0);
                        Now_act1_Graph[numberTime -1] = int.Parse(act.result.now_date[i].act1);
                        Now_act2_Graph[numberTime -1] = int.Parse(act.result.now_date[i].act2);
                        Now_act3_Graph[numberTime -1] = int.Parse(act.result.now_date[i].act3);

                        TodayAllAct = TodayAllAct + int.Parse(act.result.now_date[i].act2) + int.Parse(act.result.now_date[i].act3);
                    }
                    else if (DateType == "Y")
                    {
                        int numberTime = int.Parse(act.result.now_date[i].action_time[5].ToString() + act.result.now_date[i].action_time[6].ToString()); // 12개의 월 중 몇월인지
                        //print(numberTime);

                        Now_act0_Graph[numberTime -1] = int.Parse(act.result.now_date[i].act0);
                        Now_act1_Graph[numberTime -1] = int.Parse(act.result.now_date[i].act1);
                        Now_act2_Graph[numberTime -1] = int.Parse(act.result.now_date[i].act2);
                        Now_act3_Graph[numberTime -1] = int.Parse(act.result.now_date[i].act3);

                        TodayAllAct = TodayAllAct + int.Parse(act.result.now_date[i].act2) + int.Parse(act.result.now_date[i].act3);
                    }
                    else
                    {
                        print("Error");
                    }

                }

                for (int i = 0; i < act.result.prev_date.Count; i++)
                {
                    if (DateType == "D")
                    {
                        int numberTime = int.Parse(act.result.prev_date[i].action_time[11].ToString() + act.result.prev_date[i].action_time[12].ToString());

                        Prev_act0_Graph[numberTime] = int.Parse(act.result.prev_date[i].act0);
                        Prev_act1_Graph[numberTime] = int.Parse(act.result.prev_date[i].act1);
                        Prev_act2_Graph[numberTime] = int.Parse(act.result.prev_date[i].act2);
                        Prev_act3_Graph[numberTime] = int.Parse(act.result.prev_date[i].act3);

                        PrevAllAct = PrevAllAct + int.Parse(act.result.prev_date[i].act2) + int.Parse(act.result.prev_date[i].act3);
                    }
                    else if (DateType == "W")
                    {
                        DateTime dateTime = Convert.ToDateTime(act.result.prev_date[i].action_time.Substring(0, 10)); // yyyy-MM-dd
                        //print(dateTime.DayOfWeek);
                        int numberTime;

                        if (dateTime.DayOfWeek == DayOfWeek.Sunday)
                        {
                            numberTime = 0;
                        }
                        else if (dateTime.DayOfWeek == DayOfWeek.Monday)
                        {
                            numberTime = 1;
                        }
                        else if (dateTime.DayOfWeek == DayOfWeek.Tuesday)
                        {
                            numberTime = 2;
                        }
                        else if (dateTime.DayOfWeek == DayOfWeek.Wednesday)
                        {
                            numberTime = 3;
                        }
                        else if (dateTime.DayOfWeek == DayOfWeek.Thursday)
                        {
                            numberTime = 4;
                        }
                        else if (dateTime.DayOfWeek == DayOfWeek.Friday)
                        {
                            numberTime = 5;
                        }
                        else
                        {
                            numberTime = 6;
                        }

                        Prev_act0_Graph[numberTime] = int.Parse(act.result.prev_date[i].act0);
                        Prev_act1_Graph[numberTime] = int.Parse(act.result.prev_date[i].act1);
                        Prev_act2_Graph[numberTime] = int.Parse(act.result.prev_date[i].act2);
                        Prev_act3_Graph[numberTime] = int.Parse(act.result.prev_date[i].act3);

                        PrevAllAct = PrevAllAct + int.Parse(act.result.prev_date[i].act2) + int.Parse(act.result.prev_date[i].act3);
                    }
                    else if (DateType == "M")
                    {
                        int numberTime = int.Parse(act.result.prev_date[i].action_time[8].ToString() + act.result.prev_date[i].action_time[9].ToString()); // 31개의 일 중 몇일인지
                        //print(numberTime);
                        Prev_act0_Graph[numberTime -1] = int.Parse(act.result.prev_date[i].act0);
                        Prev_act1_Graph[numberTime -1] = int.Parse(act.result.prev_date[i].act1);
                        Prev_act2_Graph[numberTime -1] = int.Parse(act.result.prev_date[i].act2);
                        Prev_act3_Graph[numberTime -1] = int.Parse(act.result.prev_date[i].act3);

                        PrevAllAct = PrevAllAct + int.Parse(act.result.prev_date[i].act2) + int.Parse(act.result.prev_date[i].act3);
                    }
                    else if (DateType == "Y")
                    {
                        int numberTime = int.Parse(act.result.now_date[i].action_time[5].ToString() + act.result.now_date[i].action_time[6].ToString()); // 12개의 월 중 몇월인지
                        //print(numberTime);

                        Prev_act0_Graph[numberTime -1] = int.Parse(act.result.prev_date[i].act0);
                        Prev_act1_Graph[numberTime -1] = int.Parse(act.result.prev_date[i].act1);
                        Prev_act2_Graph[numberTime -1] = int.Parse(act.result.prev_date[i].act2);
                        Prev_act3_Graph[numberTime -1] = int.Parse(act.result.prev_date[i].act3);

                        PrevAllAct = PrevAllAct + int.Parse(act.result.prev_date[i].act2) + int.Parse(act.result.prev_date[i].act3);
                    }
                    else
                    {
                        print("Error");
                    }
                }
                
               
                if( DateType != "D")
                {
                    if (act.result.now_date.Count != 0)
                    { 
                        TodayAllAct = Mathf.Round(TodayAllAct / act.result.now_date.Count); 
                        
                    }
                    if(act.result.prev_date.Count != 0)
                    {
                        PrevAllAct = Mathf.Round(PrevAllAct / act.result.prev_date.Count);
                    }
                   
                    
                }

            

                if (TodayAllAct >= PrevAllAct)
                {
                    CompareAct = TodayAllAct - PrevAllAct;
                    CompareMore = " ▼";
                }
                else
                {
                    CompareAct = PrevAllAct - TodayAllAct;
                    CompareMore = " ▲";
                }

                
                for (int i = 0; i < 31; i++)
                {
                    Now_actAll_Graph[i] = Now_act2_Graph[i] + Now_act3_Graph[i];
                }
                yield return new WaitForSeconds(0.1f);
                if (DateType == "D")
                {
                    if(Dog1_graph.gameObject.activeSelf)
                    {
                        Dog1_graph.DayNumCountObj.SetActive(true);
                        Dog1_graph.WeekNumCountObj.SetActive(false);
                        Dog1_graph.MonthNumCountObj.SetActive(false);
                        Dog1_graph.YearNumCountObj.SetActive(false);

                        Dog1_graph.xSize = 35f;
                        Dog1_graph.valueList.Clear();
                        Dog1_graph.stickList.Clear();
                        Dog1_graph.valueList = Now_actAll_Graph.ToList<int>();
                        Dog1_graph.stickList = Now_actAll_Graph.ToList<int>();
                        for (int i = 0; i < 7; i++)
                        {
                            Dog1_graph.valueList.RemoveAt(Dog1_graph.valueList.Count - 1);
                            Dog1_graph.stickList.RemoveAt(Dog1_graph.stickList.Count - 1);
                        }
                        Dog1_graph.GraphReset();
                    }

                    if (Dog2_graph.gameObject.activeSelf)
                    {
                        Dog2_graph.DayNumCountObj.SetActive(true);
                        Dog2_graph.WeekNumCountObj.SetActive(false);
                        Dog2_graph.MonthNumCountObj.SetActive(false);
                        Dog2_graph.YearNumCountObj.SetActive(false);

                        Dog2_graph.xSize = 35f;
                        Dog2_graph.valueList.Clear();
                        Dog2_graph.stickList.Clear();
                        Dog2_graph.valueList = Now_actAll_Graph.ToList<int>();
                        Dog2_graph.stickList = Now_actAll_Graph.ToList<int>();
                        for (int i = 0; i < 7; i++)
                        {
                            Dog2_graph.valueList.RemoveAt(Dog2_graph.valueList.Count - 1);
                            Dog2_graph.stickList.RemoveAt(Dog2_graph.stickList.Count - 1);
                        }
                        Dog2_graph.GraphReset();
                    }
                    if (Dog3_graph.gameObject.activeSelf)
                    {
                        Dog3_graph.DayNumCountObj.SetActive(true);
                        Dog3_graph.WeekNumCountObj.SetActive(false);
                        Dog3_graph.MonthNumCountObj.SetActive(false);
                        Dog3_graph.YearNumCountObj.SetActive(false);

                        Dog3_graph.xSize = 35f;
                        Dog3_graph.valueList.Clear();
                        Dog3_graph.stickList.Clear();
                        Dog3_graph.valueList = Now_actAll_Graph.ToList<int>();
                        Dog3_graph.stickList = Now_actAll_Graph.ToList<int>();
                        for (int i = 0; i < 7; i++)
                        {
                            Dog3_graph.valueList.RemoveAt(Dog3_graph.valueList.Count - 1);
                            Dog3_graph.stickList.RemoveAt(Dog3_graph.stickList.Count - 1);
                        }
                        Dog3_graph.GraphReset();
                    }

                    TodayAllText.text = "<color=grey>어제 총 활동</color>" + "\n" + "<b><size=39>" + TodayAllAct.ToString() + " 분</size></b>";
                    PrevAllText.text = "<color=grey>오늘 총 활동</color>" + "\n" + "<b><size=39>" + PrevAllAct.ToString() + " 분</size></b>"; 
                    CompareText.text = "<color=grey>어제 대비</color>" + "\n" + "<b><size=39>" + CompareAct.ToString() + CompareMore + "</size></b>"; 
                }
                if (DateType == "W")
                {
                    if (Dog1_graph.gameObject.activeSelf)
                    {
                        Dog1_graph.DayNumCountObj.SetActive(false);
                        Dog1_graph.WeekNumCountObj.SetActive(true);
                        Dog1_graph.MonthNumCountObj.SetActive(false);
                        Dog1_graph.YearNumCountObj.SetActive(false);
                        Dog1_graph.xSize = 120f;
                        Dog1_graph.valueList.Clear();
                        Dog1_graph.stickList.Clear();
                        Dog1_graph.valueList = Now_actAll_Graph.ToList<int>();
                        Dog1_graph.stickList = Now_actAll_Graph.ToList<int>();
                        for (int i = 0; i < 24; i++)
                        {
                            Dog1_graph.valueList.RemoveAt(Dog1_graph.valueList.Count - 1);
                            Dog1_graph.stickList.RemoveAt(Dog1_graph.stickList.Count - 1);
                        }
                        Dog1_graph.GraphReset();
                    }

                    if (Dog2_graph.gameObject.activeSelf)
                    {
                        Dog2_graph.DayNumCountObj.SetActive(false);
                        Dog2_graph.WeekNumCountObj.SetActive(true);
                        Dog2_graph.MonthNumCountObj.SetActive(false);
                        Dog2_graph.YearNumCountObj.SetActive(false);
                        Dog2_graph.xSize = 120f;
                        Dog2_graph.valueList.Clear();
                        Dog2_graph.stickList.Clear();
                        Dog2_graph.valueList = Now_actAll_Graph.ToList<int>();
                        Dog2_graph.stickList = Now_actAll_Graph.ToList<int>();
                        for (int i = 0; i < 24; i++)
                        {
                            Dog2_graph.valueList.RemoveAt(Dog2_graph.valueList.Count - 1);
                            Dog2_graph.stickList.RemoveAt(Dog2_graph.stickList.Count - 1);
                        }
                        Dog2_graph.GraphReset();
                    }

                    if (Dog3_graph.gameObject.activeSelf)
                    {
                        Dog3_graph.DayNumCountObj.SetActive(false);
                        Dog3_graph.WeekNumCountObj.SetActive(true);
                        Dog3_graph.MonthNumCountObj.SetActive(false);
                        Dog3_graph.YearNumCountObj.SetActive(false);

                        Dog3_graph.xSize = 120f;
                        Dog3_graph.valueList.Clear();
                        Dog3_graph.stickList.Clear();
                        Dog3_graph.valueList = Now_actAll_Graph.ToList<int>();
                        Dog3_graph.stickList = Now_actAll_Graph.ToList<int>();
                        for (int i = 0; i < 24; i++)
                        {
                            Dog3_graph.valueList.RemoveAt(Dog3_graph.valueList.Count - 1);
                            Dog3_graph.stickList.RemoveAt(Dog3_graph.stickList.Count - 1);
                        }
                        Dog3_graph.GraphReset();
                    }

                    TodayAllText.text = "<color=grey>지난 주 평균</color>" + "\n" + "<b><size=39>" + TodayAllAct.ToString() + " 분</size></b>";
                    PrevAllText.text = "<color=grey>이번 주 평균</color>" + "\n" + "<b><size=39>" + PrevAllAct.ToString() + " 분</size></b>";
                    CompareText.text = "<color=grey>지난 주 대비</color>" + "\n" + "<b><size=39>" + CompareAct.ToString() + CompareMore + "</size></b>";
                }
                if (DateType == "M")
                {
                    if(Dog1_graph.gameObject.activeSelf)
                    {
                        Dog1_graph.DayNumCountObj.SetActive(false);
                        Dog1_graph.WeekNumCountObj.SetActive(false);
                        Dog1_graph.MonthNumCountObj.SetActive(true);
                        Dog1_graph.YearNumCountObj.SetActive(false);
                        Dog1_graph.xSize = 28f;
                        Dog1_graph.valueList.Clear();
                        Dog1_graph.stickList.Clear();
                        
                        

                        Dog1_graph.valueList = Now_actAll_Graph.ToList<int>();
                        Dog1_graph.stickList = Now_actAll_Graph.ToList<int>();

                        //Month date count
                        DateTime MonthTime = DateTime.Parse(NowTime);
                        int MonthDay = DateTime.DaysInMonth(MonthTime.Year, MonthTime.Month);

                        for (int i = 0; i < 31-MonthDay; i++)
                        {
                            Dog1_graph.valueList.RemoveAt(Dog1_graph.valueList.Count - 1);
                            Dog1_graph.stickList.RemoveAt(Dog1_graph.stickList.Count - 1);
                        }
                        Dog1_graph.GraphReset();

                    }
                    if (Dog2_graph.gameObject.activeSelf)
                    {
                        Dog2_graph.DayNumCountObj.SetActive(false);
                        Dog2_graph.WeekNumCountObj.SetActive(false);
                        Dog2_graph.MonthNumCountObj.SetActive(true);
                        Dog2_graph.YearNumCountObj.SetActive(false);
                        Dog2_graph.xSize = 28f;
                        Dog2_graph.valueList.Clear();
                        Dog2_graph.stickList.Clear();
                        Dog2_graph.valueList = Now_actAll_Graph.ToList<int>();
                        Dog2_graph.stickList = Now_actAll_Graph.ToList<int>();
                        //Month date count
                        DateTime MonthTime = DateTime.Parse(NowTime);
                        int MonthDay = DateTime.DaysInMonth(MonthTime.Year, MonthTime.Month);

                        for (int i = 0; i < 31 - MonthDay; i++)
                        {
                            Dog2_graph.valueList.RemoveAt(Dog2_graph.valueList.Count - 1);
                            Dog2_graph.stickList.RemoveAt(Dog2_graph.stickList.Count - 1);
                        }
                        Dog2_graph.GraphReset();
                    }
                    if (Dog3_graph.gameObject.activeSelf)
                    {
                        Dog3_graph.DayNumCountObj.SetActive(false);
                        Dog3_graph.WeekNumCountObj.SetActive(false);
                        Dog3_graph.MonthNumCountObj.SetActive(true);
                        Dog3_graph.YearNumCountObj.SetActive(false);

                        Dog3_graph.xSize = 28f;
                        Dog3_graph.valueList.Clear();
                        Dog3_graph.stickList.Clear();
                        Dog3_graph.valueList = Now_actAll_Graph.ToList<int>();
                        Dog3_graph.stickList = Now_actAll_Graph.ToList<int>();
                        //Month date count
                        DateTime MonthTime = DateTime.Parse(NowTime);
                        int MonthDay = DateTime.DaysInMonth(MonthTime.Year, MonthTime.Month);

                        for (int i = 0; i < 31 - MonthDay; i++)
                        {
                            Dog3_graph.valueList.RemoveAt(Dog3_graph.valueList.Count - 1);
                            Dog3_graph.stickList.RemoveAt(Dog3_graph.stickList.Count - 1);
                        }
                        Dog3_graph.GraphReset();
                    }


                    TodayAllText.text = "<color=grey>지난 달 평균</color>" + "\n" + "<b><size=39>" + TodayAllAct.ToString() + " 분</size></b>";
                    PrevAllText.text = "<color=grey>이번 달 평균</color>" + "\n" + "<b><size=39>" + PrevAllAct.ToString() + " 분</size></b>";
                    CompareText.text = "<color=grey>지난 달 대비</color>" + "\n" + "<b><size=39>" + CompareAct.ToString() + CompareMore + "</size></b>";
                }
                if (DateType == "Y")
                {
                    if (Dog1_graph.gameObject.activeSelf)
                    {
                        Dog1_graph.DayNumCountObj.SetActive(false);
                        Dog1_graph.WeekNumCountObj.SetActive(false);
                        Dog1_graph.MonthNumCountObj.SetActive(false);
                        Dog1_graph.YearNumCountObj.SetActive(true);

                        Dog1_graph.xSize = 70f;
                        Dog1_graph.valueList.Clear();
                        Dog1_graph.stickList.Clear();
                        Dog1_graph.valueList = Now_actAll_Graph.ToList<int>();
                        Dog1_graph.stickList = Now_actAll_Graph.ToList<int>();
                        for (int i = 0; i < 19; i++)
                        {
                            Dog1_graph.valueList.RemoveAt(Dog1_graph.valueList.Count - 1);
                            Dog1_graph.stickList.RemoveAt(Dog1_graph.stickList.Count - 1);
                        }

                        Dog1_graph.GraphReset();
                    }
                    if (Dog2_graph.gameObject.activeSelf)
                    {
                        Dog2_graph.DayNumCountObj.SetActive(false);
                        Dog2_graph.WeekNumCountObj.SetActive(false);
                        Dog2_graph.MonthNumCountObj.SetActive(false);
                        Dog2_graph.YearNumCountObj.SetActive(true);
                        Dog2_graph.xSize = 70f;
                        Dog2_graph.valueList.Clear();
                        Dog2_graph.stickList.Clear();
                        Dog2_graph.valueList = Now_actAll_Graph.ToList<int>();
                        Dog2_graph.stickList = Now_actAll_Graph.ToList<int>();
                        for (int i = 0; i < 19; i++)
                        {
                            Dog2_graph.valueList.RemoveAt(Dog2_graph.valueList.Count - 1);
                            Dog2_graph.stickList.RemoveAt(Dog2_graph.stickList.Count - 1);
                        }
                        Dog2_graph.GraphReset();
                    }
                    if (Dog3_graph.gameObject.activeSelf)
                    {
                        Dog3_graph.DayNumCountObj.SetActive(false);
                        Dog3_graph.WeekNumCountObj.SetActive(false);
                        Dog3_graph.MonthNumCountObj.SetActive(false);
                        Dog3_graph.YearNumCountObj.SetActive(true);
                        Dog3_graph.xSize = 70f;
                        Dog3_graph.valueList.Clear();
                        Dog3_graph.stickList.Clear();
                        Dog3_graph.valueList = Now_actAll_Graph.ToList<int>();
                        Dog3_graph.stickList = Now_actAll_Graph.ToList<int>();
                        for (int i = 0; i < 19; i++)
                        {
                            Dog3_graph.valueList.RemoveAt(Dog3_graph.valueList.Count - 1);
                            Dog3_graph.stickList.RemoveAt(Dog3_graph.stickList.Count - 1);
                        }

                        Dog3_graph.GraphReset();
                    }

                    TodayAllText.text = "<color=grey>지난 년도 평균</color>" + "\n" + "<b><size=39>" + TodayAllAct.ToString() + " 분</size></b>";
                    PrevAllText.text = "<color=grey>이번 년도 평균</color>" + "\n" + "<b><size=39>" + PrevAllAct.ToString() + " 분</size></b>";
                    CompareText.text = "<color=grey>지난 년도 대비</color>" + "\n" + "<b><size=39>" + CompareAct.ToString() + CompareMore + "</size></b>";
                }


            }
        }
    }
}
