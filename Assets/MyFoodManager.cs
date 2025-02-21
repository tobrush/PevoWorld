using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;
using System;
using UnityEngine.Networking;
using TMPro;

[Serializable]
public class FoodData
{
    public int Index;
    public int IconTag;
    public string FoodName;
    public string Brand;
    public string Kcal;
    public int Open;
}

public class MyFoodManager : MonoBehaviour
{
    public Sprite iconTagImg01, iconTagImg02, iconTagImg03;

    public List<FoodData> FoodDatas = new List<FoodData>();

    public Transform FoodList1;
    public Transform FoodList2;
    public Transform FoodList3;
    public GameObject FoodItem;


    public GameObject SearchFoodCard;
    public Transform CardSpawnList;
    public TMP_InputField SearchNameField;

    public GameObject emptyItem;

    public void Start()
    {
        ResetFoodList();
    }

    public void ResetFoodList()
    {
        StartCoroutine(UpdateFoodList1(UserData.instance.Dog1_Food));
        if(UserData.instance.Dog2_Food != "")
        {
            StartCoroutine(UpdateFoodList2(UserData.instance.Dog2_Food));
        }
        if (UserData.instance.Dog3_Food != "")
        {
            StartCoroutine(UpdateFoodList3(UserData.instance.Dog3_Food));
        }
        
    }

    IEnumerator UpdateFoodList1(string result)
    {
        // ?????????? ????
        for (int i = 0; i < FoodList1.childCount; i++)
        {
            Destroy(FoodList1.GetChild(i).gameObject);
        }

        // ???? ????
        if (result != "" && result != null)
        {
            FoodDatas = JsonConvert.DeserializeObject<List<FoodData>>(result);

            for (int i = 0; i < FoodDatas.Count; i++)
            {
                FoodDatas[i].Index = i;

                GameObject item = Instantiate(FoodItem, FoodList1);
                item.GetComponent<FoodItem>().Index = i;
                item.GetComponent<FoodItem>().IconTag = FoodDatas[i].IconTag;
                item.GetComponent<FoodItem>().FoodName = FoodDatas[i].FoodName.ToString();
                item.GetComponent<FoodItem>().Brand = FoodDatas[i].Brand.ToString();
                item.GetComponent<FoodItem>().Kcal = FoodDatas[i].Kcal.ToString();
                item.GetComponent<FoodItem>().Open = FoodDatas[i].Open;

                if (FoodDatas[i].IconTag == 0)
                {
                    item.transform.GetChild(0).GetComponent<Image>().sprite = iconTagImg01;
                }
                else if (FoodDatas[i].IconTag == 1)
                {
                    item.transform.GetChild(0).GetComponent<Image>().sprite = iconTagImg02;
                }
                else
                {
                    item.transform.GetChild(0).GetComponent<Image>().sprite = iconTagImg03;
                }


                item.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = FoodDatas[i].FoodName.ToString();
                item.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = FoodDatas[i].Brand.ToString() + " (" + FoodDatas[i].Kcal + "cal)";


                if(FoodDatas[i].Open == 1)
                {
                    item.transform.GetChild(2).GetChild(0).GetComponent<Toggle>().isOn = true;
                }
                else
                {
                    item.transform.GetChild(2).GetChild(0).GetComponent<Toggle>().isOn = false;
                }

                   
            }
        }
        else
        {
            GameObject EmptyItem = Instantiate(emptyItem, FoodList1); // ???????? ?????? ????
        }
        yield return new WaitForSeconds(0.1f);
        FoodList1.GetComponent<ContentSizeFitter>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        FoodList1.GetComponent<ContentSizeFitter>().enabled = true;
    }

    IEnumerator UpdateFoodList2(string result)
    {
        yield return new WaitForSeconds(0.1f);
        // ?????????? ????
        for (int i = 0; i < FoodList2.childCount; i++)
        {
            Destroy(FoodList2.GetChild(i).gameObject);
        }

        // ???? ????
        if (result != "" && result != null)
        {
            FoodDatas = JsonConvert.DeserializeObject<List<FoodData>>(result);

            for (int i = 0; i < FoodDatas.Count; i++)
            {
                FoodDatas[i].Index = i;

                GameObject item = Instantiate(FoodItem, FoodList2);
                item.GetComponent<FoodItem>().Index = i;
                item.GetComponent<FoodItem>().IconTag = FoodDatas[i].IconTag;
                item.GetComponent<FoodItem>().FoodName = FoodDatas[i].FoodName.ToString();
                item.GetComponent<FoodItem>().Brand = FoodDatas[i].Brand.ToString();
                item.GetComponent<FoodItem>().Kcal = FoodDatas[i].Kcal;
                item.GetComponent<FoodItem>().Open = FoodDatas[i].Open;

                if (FoodDatas[i].IconTag == 0)
                {
                    item.transform.GetChild(0).GetComponent<Image>().sprite = iconTagImg01;
                }
                else if (FoodDatas[i].IconTag == 1)
                {
                    item.transform.GetChild(0).GetComponent<Image>().sprite = iconTagImg02;
                }
                else
                {
                    item.transform.GetChild(0).GetComponent<Image>().sprite = iconTagImg03;
                }

                item.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = FoodDatas[i].FoodName.ToString();
                item.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = FoodDatas[i].Brand.ToString() + " (" + FoodDatas[i].Kcal + "cal)";

                if (FoodDatas[i].Open == 1)
                {
                    item.transform.GetChild(2).GetChild(0).GetComponent<Toggle>().isOn = true;
                }
                else
                {
                    item.transform.GetChild(2).GetChild(0).GetComponent<Toggle>().isOn = false;
                }
            }
        }
        else
        {
            GameObject EmptyItem = Instantiate(emptyItem, FoodList2); // ???????? ?????? ????
        }
        yield return new WaitForSeconds(0.1f);
        FoodList2.GetComponent<ContentSizeFitter>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        FoodList2.GetComponent<ContentSizeFitter>().enabled = true;

    }
    IEnumerator UpdateFoodList3(string result)
    {
        yield return new WaitForSeconds(0.1f);
        // ?????????? ????
        for (int i = 0; i < FoodList3.childCount; i++)
        {
            Destroy(FoodList3.GetChild(i).gameObject);
        }

        // ???? ????
        if (result != "" && result != null)
        {
            FoodDatas = JsonConvert.DeserializeObject<List<FoodData>>(result);

            for (int i = 0; i < FoodDatas.Count; i++)
            {
                FoodDatas[i].Index = i;

                GameObject item = Instantiate(FoodItem, FoodList3);
                item.GetComponent<FoodItem>().Index = i;
                item.GetComponent<FoodItem>().IconTag = FoodDatas[i].IconTag;
                item.GetComponent<FoodItem>().FoodName = FoodDatas[i].FoodName.ToString();
                item.GetComponent<FoodItem>().Brand = FoodDatas[i].Brand.ToString();
                item.GetComponent<FoodItem>().Kcal = FoodDatas[i].Kcal;
                item.GetComponent<FoodItem>().Open = FoodDatas[i].Open;

                if (FoodDatas[i].IconTag == 0)
                {
                    item.transform.GetChild(0).GetComponent<Image>().sprite = iconTagImg01;
                }
                else if (FoodDatas[i].IconTag == 1)
                {
                    item.transform.GetChild(0).GetComponent<Image>().sprite = iconTagImg02;
                }
                else
                {
                    item.transform.GetChild(0).GetComponent<Image>().sprite = iconTagImg03;
                }

                item.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = FoodDatas[i].FoodName.ToString();
                item.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = FoodDatas[i].Brand.ToString() + " (" + FoodDatas[i].Kcal + "cal)";

                if (FoodDatas[i].Open == 1)
                {
                    item.transform.GetChild(2).GetChild(0).GetComponent<Toggle>().isOn = true;
                }
                else
                {
                    item.transform.GetChild(2).GetChild(0).GetComponent<Toggle>().isOn = false;
                }
            }
        }
        else
        {
            GameObject EmptyItem = Instantiate(emptyItem, FoodList3); // ???????? ?????? ????
        }
        yield return new WaitForSeconds(0.1f);
        FoodList3.GetComponent<ContentSizeFitter>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        FoodList3.GetComponent<ContentSizeFitter>().enabled = true;
    }

    public void SearchFoodData()
    {
        StartCoroutine(FoodData_Coroutine());
    }

    IEnumerator FoodData_Coroutine()
    {
        //outputArea.text = "Loading...";

        string uri = "https://api.pevo.care/v4/pevo_world/pet_diet";
        WWWForm form = new WWWForm();
        form.AddField("diet_name", SearchNameField.text);
        using (UnityWebRequest request = UnityWebRequest.Post(uri, form))
        {
            yield return request.SendWebRequest();
            if (request.isNetworkError || request.isHttpError)
            {
                //outputArea.text = request.downloadHandler.text; // ?????? ?????? ????
                string data = request.downloadHandler.text;
                Food food = JsonConvert.DeserializeObject<Food>(data);
                //outputArea.text = Food.resultMsg;
            }

           
            else
            {
                //outputArea.text = request.downloadHandler.text; // ???? ?????? ????

                string data = request.downloadHandler.text;
                Food food = JsonConvert.DeserializeObject<Food>(data);


                for (int i = 0; i < CardSpawnList.childCount; i++)
                {
                    Destroy(CardSpawnList.GetChild(i).gameObject);
                }
                CardSpawnList.DetachChildren();




                for (int i = 0; i < food.result_diet.Count; i++)
                {
                    GameObject instance = Instantiate(SearchFoodCard, CardSpawnList);

                    instance.transform.GetChild(1).GetComponent<TMP_Text>().text = food.result_diet[i].name;
                    instance.transform.GetChild(2).GetComponent<TMP_Text>().text = food.result_diet[i].me;

                    if (food.result_diet[i].title == "???????? ??????") // ??
                    {
                        instance.transform.GetChild(0).GetComponent<TMP_Text>().text = food.result_diet[i].title + "??";
                    }
                    else if (food.result_diet[i].title == "????????????????") // ??
                    {
                        instance.transform.GetChild(0).GetComponent<TMP_Text>().text = food.result_diet[i].title + "??";
                    }
                    else if (food.result_diet[i].title == "?????????? ????") // ??????
                    {
                        instance.transform.GetChild(0).GetComponent<TMP_Text>().text = food.result_diet[i].title + "??????";
                    }
                    else if (food.result_diet[i].title == "???? ??????????") //??????
                    {
                        instance.transform.GetChild(0).GetComponent<TMP_Text>().text = food.result_diet[i].title + "??????";
                    }
                    else if (food.result_diet[i].title == "???? ??????????") //?? ????????
                    {
                        instance.transform.GetChild(0).GetComponent<TMP_Text>().text = food.result_diet[i].title + "?? ????????";
                    }
                    else if (food.result_diet[i].title == "???????? ??????") //??
                    {
                        instance.transform.GetChild(0).GetComponent<TMP_Text>().text = food.result_diet[i].title + "??";
                    }
                    else
                    {
                        instance.transform.GetChild(0).GetComponent<TMP_Text>().text = food.result_diet[i].title;
                    }

                }

            }

        }
    }
}