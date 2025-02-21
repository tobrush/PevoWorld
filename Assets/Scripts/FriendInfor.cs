using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;
using System;

public class FriendInfor : MonoBehaviour
{
    public string YourID;
    public string YourDisplayName;
    public string Location;
    public string LastLoginTime;
 

    public FriendsManager FM;

    public void Start()
    {
        FM = GameObject.Find("FriendsManager").GetComponent<FriendsManager>();
        
    }

    public void StartAcceptFriendRequest()
    {

        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "AcceptFriendRequest",
            FunctionParameter = new { FriendPlayFabId = YourID },
            GeneratePlayStreamEvent = true,  // 선택사항 서버로그 표시
        }, OnFriendRequest, (error) => print("실패"));

    }

    public void StartDenyFriendRequest()
    {

        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "DenyFriendRequest",
            FunctionParameter = new { FriendPlayFabId = YourID },
            GeneratePlayStreamEvent = true,  // 선택사항 서버로그 표시
        }, OnFriendRequest, (error) => print("실패"));

    }

    public void OnFriendRequest(ExecuteCloudScriptResult result)
    {
        FM.GetFriendList();
    }


    public void OnFriendsInfor()
    {
        FM.YourID = YourID;

        StartCoroutine(Dealy());
        
    }

    IEnumerator Dealy()
    {

        yield return new WaitForSeconds(0.1f);
        FM.FriendsGetAnotherUserData();
    }


    public void InputMessage()
    {
        FM.SendBox.SetActive(true);

        FM.YourID = YourID;
        FM.SendBox.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<TMPro.TMP_Text>().text = FM.MyDisplayName;
        FM.SendBox.transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<TMPro.TMP_Text>().text = YourDisplayName;

        
    }

    public void GoToFriendHouse()
    {
        FM.YourID = YourID;
        FM.OtherUserHousePanelCheck.SetActive(true);
        //FM.OtherUserName.text = YourDisplayName + " 님의 집에 방문하시겠습니까?";
    }
}
