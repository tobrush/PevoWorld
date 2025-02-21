using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class FoodItem : MonoBehaviour
{
    public MyFoodManager MFM;
    public GameObject MyFoodEditPanel;
    public GameObject SearchFood;
    public int Index;
    public int IconTag;
    public string FoodName;
    public string Brand;
    public string Kcal;
    public int Open;

    public ClickManager CM;
    public string functionName;

    public void Start()
    {
        CM = GameObject.Find("ClickManager").GetComponent<ClickManager>();

        MFM = GameObject.Find("DataManager").GetComponent<MyFoodManager>();

        MyFoodEditPanel = GameObject.Find("MyFoodEditPanel");
    }

    public void EditFood()
    { //눌렀을때 표시
        

        MyFoodEditPanel.transform.GetChild(2).GetChild(3).GetChild(IconTag).GetComponent<Toggle>().isOn = true;
        MyFoodEditPanel.transform.GetChild(2).GetChild(4).GetComponent<TMP_InputField>().text = FoodName;
        
        GameObject searchFood = Instantiate(SearchFood, MyFoodEditPanel.transform.GetChild(2).GetChild(5).GetChild(0).GetChild(0).transform);
        //searchFood text change

        MyFoodEditPanel.SetActive(true);


    }

    public void RemoveFood()
    {
        if (CM.nowPage == 1)
        {
            functionName = "MyFoodRemove1";
        }
        else if (CM.nowPage == 2)
        {
            functionName = "MyFoodRemove2";
        }
        else if (CM.nowPage == 3)
        {
            functionName = "MyFoodRemove3";
        }


        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = functionName,
            FunctionParameter = new { Index = Index }, // FJsonKeeper(JsonObject)
            GeneratePlayStreamEvent = true,
        }, OnMyFoodRefreshSuccess, (error) => print("실패"));
    }

    public void OnMyFoodRefreshSuccess(ExecuteCloudScriptResult result)
    {
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
