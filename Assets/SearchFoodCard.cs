using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine.UI;


public class SearchFoodCard : MonoBehaviour
{
    public string functionName;
    public MyFoodManager MFM;
    public ClickManager CM;

    public Button AddBtn;

    public void MyFoodAdd()
    {
        MFM = GameObject.Find("DataManager").GetComponent<MyFoodManager>();

        CM = GameObject.Find("ClickManager").GetComponent<ClickManager>();
        int iconTag = 0;

        if (this.gameObject.transform.parent.parent.parent.parent.transform.GetChild(4).GetChild(0).GetComponent<Toggle>().isOn)
        {
            iconTag = 0;
        }
        else if(this.gameObject.transform.parent.parent.parent.parent.transform.GetChild(4).GetChild(1).GetComponent<Toggle>().isOn)
        {
            iconTag = 1;
        }
        else
        {
            iconTag = 2;
        }

        string brand = this.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text;
        string productName = this.gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text;
        string kcal = this.gameObject.transform.GetChild(2).GetComponent<TMP_Text>().text;

        
        if (CM.nowPage == 1)
        {
            functionName = "MyFoodAdd1";
        }
        else if (CM.nowPage == 2)
        {
            functionName = "MyFoodAdd2";
        }
        else if (CM.nowPage == 3)
        {
            functionName = "MyFoodAdd3";
        }
        AddBtn.interactable = false;

        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {

            FunctionName = functionName,
            FunctionParameter = new { IconTag = iconTag, FoodName = productName, Brand = brand, Kcal = kcal, Open = 1 },
            GeneratePlayStreamEvent = true,  // 선택사항 서버로그 표시
        }, (result) => OnSendMyFoodSuccess(result), (error) => print("실패"));
    }

    public void OnSendMyFoodSuccess(ExecuteCloudScriptResult result)
    {
        this.gameObject.transform.parent.parent.parent.parent.transform.GetChild(4).GetChild(1).GetComponent<Toggle>().Select();
        this.gameObject.transform.parent.parent.parent.parent.transform.GetChild(5).GetComponent<TMP_InputField>().text = "";

        for (int i = 0; i < this.gameObject.transform.parent.childCount; i++)
        {
            Destroy(this.gameObject.transform.parent.GetChild(i).gameObject);
        }
        this.gameObject.transform.parent.parent.parent.parent.parent.gameObject.SetActive(false);

        //플레이팹에서 다시불러와서 초기화

        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = UserData.instance.User_MyID
        }, result => {
            UserData.instance.Dog1_Food = result.Data["Dog1_Food"].Value;
            UserData.instance.Dog2_Food = result.Data["Dog2_Food"].Value;
            UserData.instance.Dog3_Food = result.Data["Dog3_Food"].Value;

            MFM.ResetFoodList();
        }, error => {
            Debug.Log(error.GenerateErrorReport());
        });
    }
}
