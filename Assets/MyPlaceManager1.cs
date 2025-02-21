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


public class MyPlaceManager1 : MonoBehaviour
{
    public Sprite iconTagImg01, iconTagImg02, iconTagImg03, iconTagImg04;

    public List<PlaceData> PlaceDatas = new List<PlaceData>();

    public Transform PlaceList1;
    public Transform PlaceList2;
    public Transform PlaceList3;
    public GameObject PlaceItem;

    public GameObject emptyItem;


    void Start()
    {
        
        ResetPlaceList();
    }

    public void ResetPlaceList()
    {
        StartCoroutine(UpdatePlaceList1(OtherUserData.instance.OhterDog1_Place));

        if (OtherUserData.instance.OhterDog2_Place != "")
        {
            StartCoroutine(UpdatePlaceList2(OtherUserData.instance.OhterDog2_Place));
        }
        if (OtherUserData.instance.OhterDog3_Place != "")
        {
            StartCoroutine(UpdatePlaceList3(OtherUserData.instance.OhterDog3_Place));
        }

    }

    IEnumerator UpdatePlaceList1(string result)
    {
        // 기존리스트 삭제
        for (int i = 0; i < PlaceList1.childCount; i++)
        {
            Destroy(PlaceList1.GetChild(i).gameObject);
        }

        // 새로 생성
        if (result != "" && result != null)
        {
            PlaceDatas = JsonConvert.DeserializeObject<List<PlaceData>>(result);

            for (int i = 0; i < PlaceDatas.Count; i++)
            {
                PlaceDatas[i].Index = i;

                GameObject item = Instantiate(PlaceItem, PlaceList1);
                item.GetComponent<PlaceItem>().Index = i;
                item.GetComponent<PlaceItem>().IconTag = PlaceDatas[i].IconTag;
                item.GetComponent<PlaceItem>().PlaceName = PlaceDatas[i].PlaceName.ToString();
                item.GetComponent<PlaceItem>().Address = PlaceDatas[i].Address.ToString();
                item.GetComponent<PlaceItem>().Detail = PlaceDatas[i].Detail.ToString();
                item.GetComponent<PlaceItem>().LocalLat = PlaceDatas[i].LocalLat.ToString();
                item.GetComponent<PlaceItem>().LocalLon = PlaceDatas[i].LocalLon.ToString();
                item.GetComponent<PlaceItem>().Open = PlaceDatas[i].Open;

                if (PlaceDatas[i].IconTag == 0)
                {
                    item.transform.GetChild(0).GetComponent<Image>().sprite = iconTagImg01;
                }
                else if (PlaceDatas[i].IconTag == 1)
                {
                    item.transform.GetChild(0).GetComponent<Image>().sprite = iconTagImg02;
                }
                else if (PlaceDatas[i].IconTag == 2)
                {
                    item.transform.GetChild(0).GetComponent<Image>().sprite = iconTagImg03;
                }
                else
                {
                    item.transform.GetChild(0).GetComponent<Image>().sprite = iconTagImg04;
                }
                item.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = PlaceDatas[i].PlaceName.ToString();
                item.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = PlaceDatas[i].Address.ToString() + " " + PlaceDatas[i].Detail.ToString();
                //버튼을 누르면 <위도경도 기반인>지도로 보기


                if (PlaceDatas[i].Open == 1)
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
            GameObject EmptyItem = Instantiate(emptyItem, PlaceList1); // 비엇다면 이걸로 대체
        }
        yield return new WaitForSeconds(0.1f);
        PlaceList1.GetComponent<ContentSizeFitter>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        PlaceList1.GetComponent<ContentSizeFitter>().enabled = true;
    }

    IEnumerator UpdatePlaceList2(string result)
    {
        yield return new WaitForSeconds(0.1f);
        // 기존리스트 삭제
        for (int i = 0; i < PlaceList2.childCount; i++)
        {
            Destroy(PlaceList2.GetChild(i).gameObject);
        }

        // 새로 생성
        if (result != "" && result != null)
        {
            PlaceDatas = JsonConvert.DeserializeObject<List<PlaceData>>(result);

            for (int i = 0; i < PlaceDatas.Count; i++)
            {
                PlaceDatas[i].Index = i;

                GameObject item = Instantiate(PlaceItem, PlaceList2);
                item.GetComponent<PlaceItem>().Index = i;
                item.GetComponent<PlaceItem>().IconTag = PlaceDatas[i].IconTag;
                item.GetComponent<PlaceItem>().PlaceName = PlaceDatas[i].PlaceName.ToString();
                item.GetComponent<PlaceItem>().Address = PlaceDatas[i].Address.ToString();
                item.GetComponent<PlaceItem>().Detail = PlaceDatas[i].Detail.ToString();
                item.GetComponent<PlaceItem>().LocalLat = PlaceDatas[i].LocalLat.ToString();
                item.GetComponent<PlaceItem>().LocalLon = PlaceDatas[i].LocalLon.ToString();
                item.GetComponent<PlaceItem>().Open = PlaceDatas[i].Open;


                if (PlaceDatas[i].IconTag == 0)
                {
                    item.transform.GetChild(0).GetComponent<Image>().sprite = iconTagImg01;
                }
                else if (PlaceDatas[i].IconTag == 1)
                {
                    item.transform.GetChild(0).GetComponent<Image>().sprite = iconTagImg02;
                }
                else if (PlaceDatas[i].IconTag == 2)
                {
                    item.transform.GetChild(0).GetComponent<Image>().sprite = iconTagImg03;
                }
                else
                {
                    item.transform.GetChild(0).GetComponent<Image>().sprite = iconTagImg04;
                }
                item.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = PlaceDatas[i].PlaceName.ToString();
                item.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = PlaceDatas[i].Address.ToString() + " " + PlaceDatas[i].Detail.ToString();
                //버튼을 누르면 <위도경도 기반인>지도로 보기


                if (PlaceDatas[i].Open == 1)
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
            GameObject EmptyItem = Instantiate(emptyItem, PlaceList2); // 비엇다면 이걸로 대체
        }
        yield return new WaitForSeconds(0.1f);
        PlaceList2.GetComponent<ContentSizeFitter>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        PlaceList2.GetComponent<ContentSizeFitter>().enabled = true;

    }
    IEnumerator UpdatePlaceList3(string result)
    {
        yield return new WaitForSeconds(0.1f);
        // 기존리스트 삭제
        for (int i = 0; i < PlaceList3.childCount; i++)
        {
            Destroy(PlaceList3.GetChild(i).gameObject);
        }

        // 새로 생성
        if (result != "" && result != null)
        {
            PlaceDatas = JsonConvert.DeserializeObject<List<PlaceData>>(result);

            for (int i = 0; i < PlaceDatas.Count; i++)
            {
                PlaceDatas[i].Index = i;

                GameObject item = Instantiate(PlaceItem, PlaceList3);
                item.GetComponent<PlaceItem>().Index = i;
                item.GetComponent<PlaceItem>().IconTag = PlaceDatas[i].IconTag;
                item.GetComponent<PlaceItem>().PlaceName = PlaceDatas[i].PlaceName.ToString();
                item.GetComponent<PlaceItem>().Address = PlaceDatas[i].Address.ToString();
                item.GetComponent<PlaceItem>().Detail = PlaceDatas[i].Detail.ToString();
                item.GetComponent<PlaceItem>().LocalLat = PlaceDatas[i].LocalLat.ToString();
                item.GetComponent<PlaceItem>().LocalLon = PlaceDatas[i].LocalLon.ToString();
                item.GetComponent<PlaceItem>().Open = PlaceDatas[i].Open;
                if (PlaceDatas[i].IconTag == 0)
                {
                    item.transform.GetChild(0).GetComponent<Image>().sprite = iconTagImg01;
                }
                else if (PlaceDatas[i].IconTag == 1)
                {
                    item.transform.GetChild(0).GetComponent<Image>().sprite = iconTagImg02;
                }
                else if (PlaceDatas[i].IconTag == 2)
                {
                    item.transform.GetChild(0).GetComponent<Image>().sprite = iconTagImg03;
                }
                else
                {
                    item.transform.GetChild(0).GetComponent<Image>().sprite = iconTagImg04;
                }
                item.transform.GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = PlaceDatas[i].PlaceName.ToString();
                item.transform.GetChild(1).GetChild(1).GetComponent<TMP_Text>().text = PlaceDatas[i].Address.ToString() + " " + PlaceDatas[i].Detail.ToString();
                //버튼을 누르면 <위도경도 기반인>지도로 보기


                if (PlaceDatas[i].Open == 1)
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
            GameObject EmptyItem = Instantiate(emptyItem, PlaceList3); // 비엇다면 이걸로 대체
        }
        yield return new WaitForSeconds(0.1f);
        PlaceList3.GetComponent<ContentSizeFitter>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        PlaceList3.GetComponent<ContentSizeFitter>().enabled = true;
    }
}
