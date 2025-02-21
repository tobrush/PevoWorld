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

public class MyFoodManager1 : MonoBehaviour
{
    public Sprite iconTagImg01, iconTagImg02, iconTagImg03;

    public List<FoodData> FoodDatas = new List<FoodData>();

    public Transform FoodList1;
    public Transform FoodList2;
    public Transform FoodList3;
    public GameObject FoodItem;


    public GameObject emptyItem;

    public void Start()
    {
        ResetFoodList();
    }

    public void ResetFoodList()
    {
        StartCoroutine(UpdateFoodList1(OtherUserData.instance.OhterDog1_Food));
        if(OtherUserData.instance.OhterDog2_Food != "")
        {
            StartCoroutine(UpdateFoodList2(OtherUserData.instance.OhterDog2_Food));
        }
        if (OtherUserData.instance.OhterDog3_Food != "")
        {
            StartCoroutine(UpdateFoodList3(OtherUserData.instance.OhterDog3_Food));
        }
        
    }

    IEnumerator UpdateFoodList1(string result)
    {
        // 기존리스트 삭제
        for (int i = 0; i < FoodList1.childCount; i++)
        {
            Destroy(FoodList1.GetChild(i).gameObject);
        }

        // 새로 생성
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
            GameObject EmptyItem = Instantiate(emptyItem, FoodList1); // 비엇다면 이걸로 대체
        }
        yield return new WaitForSeconds(0.1f);
        FoodList1.GetComponent<ContentSizeFitter>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        FoodList1.GetComponent<ContentSizeFitter>().enabled = true;
    }

    IEnumerator UpdateFoodList2(string result)
    {
        yield return new WaitForSeconds(0.1f);
        // 기존리스트 삭제
        for (int i = 0; i < FoodList2.childCount; i++)
        {
            Destroy(FoodList2.GetChild(i).gameObject);
        }

        // 새로 생성
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
            GameObject EmptyItem = Instantiate(emptyItem, FoodList2); // 비엇다면 이걸로 대체
        }
        yield return new WaitForSeconds(0.1f);
        FoodList2.GetComponent<ContentSizeFitter>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        FoodList2.GetComponent<ContentSizeFitter>().enabled = true;

    }
    IEnumerator UpdateFoodList3(string result)
    {
        yield return new WaitForSeconds(0.1f);
        // 기존리스트 삭제
        for (int i = 0; i < FoodList3.childCount; i++)
        {
            Destroy(FoodList3.GetChild(i).gameObject);
        }

        // 새로 생성
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
            GameObject EmptyItem = Instantiate(emptyItem, FoodList3); // 비엇다면 이걸로 대체
        }
        yield return new WaitForSeconds(0.1f);
        FoodList3.GetComponent<ContentSizeFitter>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        FoodList3.GetComponent<ContentSizeFitter>().enabled = true;
    }

}