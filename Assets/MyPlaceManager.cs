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
public class PlaceData
{
    public int Index;
    public int IconTag;
    public string PlaceName;
    public string Address;
    public string Detail;
    public string LocalLat;
    public string LocalLon;
    public int Open;
}

public class MyPlaceManager : MonoBehaviour
{
    public Sprite iconTagImg01, iconTagImg02, iconTagImg03, iconTagImg04;

    public List<PlaceData> PlaceDatas = new List<PlaceData>();

    public Transform PlaceList1;
    public Transform PlaceList2;
    public Transform PlaceList3;
    public GameObject PlaceItem;

    public Toggle Icon01, Icon02, Icon03, Icon04;
    public GameObject emptyItem;
    public string functionName;
    public ClickManager CM;
    public GameObject MyPlaceEditPanel;

    public Button AddBtn;
    public GameObject SearchResult;
    public TMP_InputField SearchAddress;
    public TMP_InputField placeName;
    public TMP_Text address;
    public TMP_InputField detail;

    public GoogleGeocodingAPI GGA;

    void Start()
    {
        
        ResetPlaceList();
    }

    public void MyPlaceAdd()
    {
        int iconTag = 0;

        if (Icon01.isOn)
        {
            iconTag = 0;
        }
        else if (Icon02.isOn)
        {
            iconTag = 1;
        }
        else if(Icon03.isOn)
        {
            iconTag = 2;
        }
        else
        {
            iconTag = 3;
        }

        string localLat = GGA.Lat;
        string localLon = GGA.Lng;

        if (CM.nowPage == 1)
        {
            functionName = "MyPlaceAdd1";
        }
        else if (CM.nowPage == 2)
        {
            functionName = "MyPlaceAdd2";
        }
        else if (CM.nowPage == 3)
        {
            functionName = "MyPlaceAdd3";
        }
        AddBtn.interactable = false;


        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = functionName,
            FunctionParameter = new { IconTag = iconTag, PlaceName = placeName.text, Address = address.text, Detail = detail.text, LocalLat = localLat, LocalLon = localLon, Open = 1 },
            GeneratePlayStreamEvent = true,  // 선택사항 서버로그 표시
        }, (result) => OnSendMyPlaceSuccess(result), (error) => print("실패"));
    }
    public void OnSendMyPlaceSuccess(ExecuteCloudScriptResult result)
    {
        //정리필요
        Icon01.GetComponent<Toggle>().Select();
        SearchAddress.text = "";
        
        MyPlaceEditPanel.gameObject.SetActive(false);

        //플레이팹에서 다시불러와서 초기화

        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = UserData.instance.User_MyID
        }, result => {
            UserData.instance.Dog1_Place = result.Data["Dog1_Place"].Value;
            UserData.instance.Dog2_Place = result.Data["Dog2_Place"].Value;
            UserData.instance.Dog3_Place = result.Data["Dog3_Place"].Value;

            ResetPlaceList();
        }, error => {
            Debug.Log(error.GenerateErrorReport());
        });
        SearchResult.SetActive(false);
        AddBtn.interactable = true;
    }





    public void ResetPlaceList()
    {
        StartCoroutine(UpdatePlaceList1(UserData.instance.Dog1_Place));

        if (UserData.instance.Dog2_Place != "")
        {
            StartCoroutine(UpdatePlaceList2(UserData.instance.Dog2_Place));
        }
        if (UserData.instance.Dog3_Place != "")
        {
            StartCoroutine(UpdatePlaceList3(UserData.instance.Dog3_Place));
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
