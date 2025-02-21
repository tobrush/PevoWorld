using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class PlaceItem : MonoBehaviour
{
    public UIManager uiManager;
    public MyPlaceManager MPM;
    public GameObject MyPlaceEditPanel;
    public GameObject SearchPlace;
    public int Index;
    public int IconTag;
    public string PlaceName;
    public string Address;
    public string Detail;
    public string LocalLat;
    public string LocalLon;
    public int Open;

    public ClickManager CM;
    public string functionName;
    public void Start()
    {
        uiManager = GameObject.Find("GoogleMapManager").GetComponent<UIManager>();

        CM = GameObject.Find("ClickManager").GetComponent<ClickManager>();

        MPM = GameObject.Find("DataManager").GetComponent<MyPlaceManager>();

        MyPlaceEditPanel = GameObject.Find("MyPlaceEditPanel");
    }

    public void RemovePlace()
    {
        if (CM.nowPage == 1)
        {
            functionName = "MyPlaceRemove1";
        }
        else if (CM.nowPage == 2)
        {
            functionName = "MyPlaceRemove2";
        }
        else if (CM.nowPage == 3)
        {
            functionName = "MyPlaceRemove3";
        }


        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = functionName,
            FunctionParameter = new { Index = Index }, // FJsonKeeper(JsonObject)
            GeneratePlayStreamEvent = true,
        }, OnMyPlaceRefreshSuccess, (error) => print("½ÇÆÐ"));
    }
    public void OnMyPlaceRefreshSuccess(ExecuteCloudScriptResult result)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = UserData.instance.User_MyID
        }, result =>
        {
            UserData.instance.Dog1_Place = result.Data["Dog1_Place"].Value;
            UserData.instance.Dog2_Place = result.Data["Dog2_Place"].Value;
            UserData.instance.Dog3_Place = result.Data["Dog3_Place"].Value;

            MPM.ResetPlaceList();
        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }


    public void viewGoogleMap()
    {
        uiManager.ViewMap(16, LocalLat, LocalLon, PlaceName, Address + " " +Detail);

    }

}
