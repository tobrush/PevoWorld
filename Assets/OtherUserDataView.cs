using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;



public class OtherUserDataView : MonoBehaviour
{

    //[SerializeField]
    //public static OtherUserData instance = null;

    public FriendsManager FM;
    public GameObject OhterUserPanel;

    public TMP_Text OtherUser_ID; // 내 아이디
    public TMP_Text OtherUser_Name; // 유저이름
    public TMP_Text OtherUser_Local; // 지역
    public TMP_Text OtherUser_Hello; // 인사말

    public TMP_Text OtherUser_LikeCount; // 좋아요 수
    public TMP_Text OtherUser_FriendsCount; // 친구 수
    public string[] OtherUser_LikeIDs;
    public List<string> OtherUser_FriendsIDs;


    public TMP_Text OtherUser_SaveTime; // 최종접속시간 및 저장시간
    public string OtherUser_DogCount; // 강아지 수
    public string OtherUser_PevoOne;


    public TMP_Text Dog1_Name; // 개이름
    public TMP_Text Dog1_Style; // 견종
    public TMP_Text Dog1_Age; // 개나이
    public TMP_Text Dog1_Sex; // 성별
    public TMP_Text Dog1_Neuter; // 중성화
    public TMP_Text Dog1_Level; //레벨

    public GameObject Dog2Slot;
    public GameObject Dog2Line;
    public TMP_Text Dog2_Name; // 개이름
    public TMP_Text Dog2_Style; // 견종
    public TMP_Text Dog2_Age; // 개나이
    public TMP_Text Dog2_Sex; // 성별
    public TMP_Text Dog2_Neuter; // 중성화
    public TMP_Text Dog2_Level;

    public GameObject Dog3Slot;
    public GameObject Dog3Line;
    public TMP_Text Dog3_Name; // 개이름
    public TMP_Text Dog3_Style; // 견종
    public TMP_Text Dog3_Age; // 개나이
    public TMP_Text Dog3_Sex; // 성별
    public TMP_Text Dog3_Neuter; // 중성화
    public TMP_Text Dog3_Level; //레벨

    public Image PlayerPhoto;
    public Image Dog1Photo;
    public Image Dog2Photo;
    public Image Dog3Photo;

    public GameObject AddFriendBtn;
    public GameObject GoHomeBtn;

    public void Start()
    {
        PhotoManager.instance.OUDV = this;
    }
    public void LikeAdd()
    {

    }

    public void FriendAdd()
    {

    }

    public void FriendRequst()
    {
        FM.StartSendFriendRequest(OtherUser_ID.text);
    }

    public void FriendRemove()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "DenyFriendRequest",
            FunctionParameter = new { FriendPlayFabId = OtherUser_ID.text },
            GeneratePlayStreamEvent = true,  // 선택사항 서버로그 표시
        }, OnFriendRequest, (error) => print("실패"));

    }

    public void OnFriendRequest(ExecuteCloudScriptResult result)
    {
        FM.GetFriendList();
    }


    public void SendMessage()
    {
        //1. 샌드박스 열기
        //겟차일드0 켜고
        //겟차일드1 끄고
        // 겟차0겟차1겟차1 text = 내이름표기
        // 겟차0겟차2겟차1 text = 상대이름표기
    }
    public void ToYourHome()
    {
        OtherUserData.instance.OhterUser_ID = OtherUser_ID.text;

        Transitioner.Instance.TransitionToScene("OtherUserHome");
    }


}
