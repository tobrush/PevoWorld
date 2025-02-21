using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;
using System;


public class now_date
{
    public string act0 { get; set; }
    public string act1 { get; set; }
    public string act2 { get; set; }
    public string act3 { get; set; }
    public string act4 { get; set; }
    public string action_time { get; set; }
}
public class now_avg
{
    public string min_act { get; set; }
    public string two_act { get; set; }
    public string three_act { get; set; }
    public string all_avg { get; set; }
    public string for_act { get; set; }
    public string five_act { get; set; }
    public string max_act { get; set; }
    public string user_act { get; set; }
}
public class prev_date
{
    public string act0 { get; set; }
    public string act1 { get; set; }
    public string act2 { get; set; }
    public string act3 { get; set; }
    public string act4 { get; set; }
    public string action_time { get; set; }
}
public class prev_avg
{
    public string min_act { get; set; }
    public string two_act { get; set; }
    public string three_act { get; set; }
    public string all_avg { get; set; }
    public string for_act { get; set; }
    public string five_act { get; set; }
    public string max_act { get; set; }
    public string user_act { get; set; }
}

public class result
{
    public List<now_date> now_date { get; set; }
    public List<now_avg> now_avg { get; set; }
    public List<prev_date> prev_date { get; set; }
    public List<prev_avg> prev_avg { get; set; }
}


public class Act
{
    public string pet_id;

    public string resultCode { get; set; }
    public string resultMsg { get; set; }
    public result result { get; set; }

}

public class Dog
{
    public string pet_id;

    public string pet_name { get; set; }
    public string breed_name { get; set; }
    public string birth { get; set; }
    public string gender { get; set; }
    public string is_neutered { get; set; }
    public string weight { get; set; }

    public string petRgtNo { get; set; }
    public string adopted_date { get; set; }

    public string diet_me { get; set; }
    public string diet_title { get; set; }
    public string diet_name { get; set; }

    public string resultCode { get; set; }
    public string resultMsg { get; set; }

}



public class PevoDogGetPostApiManager : MonoBehaviour
{

    public int[] Now_act0_Graph; // ??????
    public int[] Now_act1_Graph; // ????
    public int[] Now_act2_Graph; // ?????? ????
    public int[] Now_act3_Graph; // ?????? ????

    public int[] Prev_act0_Graph; // ??????
    public int[] Prev_act1_Graph; // ????
    public int[] Prev_act2_Graph; // ?????? ????
    public int[] Prev_act3_Graph; // ?????? ????

    public float TodayAllAct;
    public float PrevAllAct;
    public float CompareAct;
    public bool CompareMore;

    public Dog dog;

    public Act act;

    public string Serch_pet_id;
    public string DateType; // D : ??, W : ??, M : ??, Y : ??
    public string NowTime; // yyyy-MM-dd 00:00:00 

    public TMP_InputField outputArea;

    void Start()
    {
        outputArea = GameObject.Find("OutputArea").GetComponent<TMP_InputField>();
        //GameObject.Find("PostButton").GetComponent<Button>().onClick.AddListener(DogData);
    }

    public void DogData()
    {
        StartCoroutine(DogData_Coroutine());
    }
    public void ActData()
    {

        //DateType = "D";
        NowTime = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00"; // ???? ???????? ????????
        StartCoroutine(ActData_Coroutine());
    }

    IEnumerator DogData_Coroutine()
    {
        outputArea.text = "Loading...";

        string uri = "https://api.pevo.care/v4/pevo_world/pet_info";
        WWWForm form = new WWWForm();
        form.AddField("pet_id", Serch_pet_id);
        using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                //outputArea.text = request.downloadHandler.text; // ?????? ?????? ????
                string data = request.downloadHandler.text;
                Dog dog = JsonConvert.DeserializeObject<Dog>(data);
                outputArea.text = dog.resultMsg;
            }

            else
            {
                //outputArea.text = request.downloadHandler.text; // ???? ?????? ????

                string data = request.downloadHandler.text;

                Dog dog = JsonConvert.DeserializeObject<Dog>(data);
                dog.pet_id = Serch_pet_id;
                print(dog.pet_name);

            }

        }

    }



    IEnumerator ActData_Coroutine()
    {
        outputArea.text = "Loading...";

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
                //outputArea.text = request.downloadHandler.text; // ?????? ?????? ????
                string data = request.downloadHandler.text;
                Act act = JsonConvert.DeserializeObject<Act>(data);
                outputArea.text = act.resultMsg;
            }

            else
            {
                //outputArea.text = request.downloadHandler.text; // ???? ?????? ????

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
                        int numberTime = int.Parse(act.result.now_date[i].action_time[11].ToString() + act.result.now_date[i].action_time[12].ToString()); // 24???? ?? ?? ????????

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
                        int numberTime = int.Parse(act.result.now_date[i].action_time[8].ToString() + act.result.now_date[i].action_time[9].ToString()); // 31???? ?? ?? ????????
                        //print(numberTime);
                        Now_act0_Graph[numberTime -1] = int.Parse(act.result.now_date[i].act0);
                        Now_act1_Graph[numberTime -1] = int.Parse(act.result.now_date[i].act1);
                        Now_act2_Graph[numberTime -1] = int.Parse(act.result.now_date[i].act2);
                        Now_act3_Graph[numberTime -1] = int.Parse(act.result.now_date[i].act3);

                        TodayAllAct = TodayAllAct + int.Parse(act.result.now_date[i].act2) + int.Parse(act.result.now_date[i].act3);
                    }
                    else if (DateType == "Y")
                    {
                        int numberTime = int.Parse(act.result.now_date[i].action_time[5].ToString() + act.result.now_date[i].action_time[6].ToString()); // 12???? ?? ?? ????????
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
                        int numberTime = int.Parse(act.result.prev_date[i].action_time[8].ToString() + act.result.prev_date[i].action_time[9].ToString()); // 31???? ?? ?? ????????
                        //print(numberTime);
                        Prev_act0_Graph[numberTime -1] = int.Parse(act.result.prev_date[i].act0);
                        Prev_act1_Graph[numberTime -1] = int.Parse(act.result.prev_date[i].act1);
                        Prev_act2_Graph[numberTime -1] = int.Parse(act.result.prev_date[i].act2);
                        Prev_act3_Graph[numberTime -1] = int.Parse(act.result.prev_date[i].act3);

                        PrevAllAct = PrevAllAct + int.Parse(act.result.prev_date[i].act2) + int.Parse(act.result.prev_date[i].act3);
                    }
                    else if (DateType == "Y")
                    {
                        int numberTime = int.Parse(act.result.now_date[i].action_time[5].ToString() + act.result.now_date[i].action_time[6].ToString()); // 12???? ?? ?? ????????
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
                    CompareMore = true;
                }
                else
                {
                    CompareAct = PrevAllAct - TodayAllAct;
                    CompareMore = false;
                }

            }
        }
    }
}
