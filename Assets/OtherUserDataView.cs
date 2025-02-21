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

    public TMP_Text OtherUser_ID; // �� ���̵�
    public TMP_Text OtherUser_Name; // �����̸�
    public TMP_Text OtherUser_Local; // ����
    public TMP_Text OtherUser_Hello; // �λ縻

    public TMP_Text OtherUser_LikeCount; // ���ƿ� ��
    public TMP_Text OtherUser_FriendsCount; // ģ�� ��
    public string[] OtherUser_LikeIDs;
    public List<string> OtherUser_FriendsIDs;


    public TMP_Text OtherUser_SaveTime; // �������ӽð� �� ����ð�
    public string OtherUser_DogCount; // ������ ��
    public string OtherUser_PevoOne;


    public TMP_Text Dog1_Name; // ���̸�
    public TMP_Text Dog1_Style; // ����
    public TMP_Text Dog1_Age; // ������
    public TMP_Text Dog1_Sex; // ����
    public TMP_Text Dog1_Neuter; // �߼�ȭ
    public TMP_Text Dog1_Level; //����

    public GameObject Dog2Slot;
    public GameObject Dog2Line;
    public TMP_Text Dog2_Name; // ���̸�
    public TMP_Text Dog2_Style; // ����
    public TMP_Text Dog2_Age; // ������
    public TMP_Text Dog2_Sex; // ����
    public TMP_Text Dog2_Neuter; // �߼�ȭ
    public TMP_Text Dog2_Level;

    public GameObject Dog3Slot;
    public GameObject Dog3Line;
    public TMP_Text Dog3_Name; // ���̸�
    public TMP_Text Dog3_Style; // ����
    public TMP_Text Dog3_Age; // ������
    public TMP_Text Dog3_Sex; // ����
    public TMP_Text Dog3_Neuter; // �߼�ȭ
    public TMP_Text Dog3_Level; //����

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
            GeneratePlayStreamEvent = true,  // ���û��� �����α� ǥ��
        }, OnFriendRequest, (error) => print("����"));

    }

    public void OnFriendRequest(ExecuteCloudScriptResult result)
    {
        FM.GetFriendList();
    }


    public void SendMessage()
    {
        //1. ����ڽ� ����
        //�����ϵ�0 �Ѱ�
        //�����ϵ�1 ����
        // ����0����1����1 text = ���̸�ǥ��
        // ����0����2����1 text = ����̸�ǥ��
    }
    public void ToYourHome()
    {
        OtherUserData.instance.OhterUser_ID = OtherUser_ID.text;

        Transitioner.Instance.TransitionToScene("OtherUserHome");
    }


}
