using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class FriendsManager : MonoBehaviour
{
    public string YourID;
    
    public string MyDisplayName;
    public string MyID;
    public string OtherUserName;

    public List<FriendInfo> _friends = null;

    public Transform FriendItemList;
    public Transform WaitFriendItemList;
    public Transform MyRequstFriendItemList;
    public GameObject FriendItem;

    public List<GameObject> FriendVeiwList;

    public GameObject SendBox;
    public TMP_InputField TitleMessege;
    public TMP_InputField DescMessege;
    public GameObject CheckRemove;

    public TMP_Text FriendCount01;
    public TMP_Text FriendCount02;
    public TMP_Text FriendCount03;


    [Header("Another User Data")]
    public Text anotherUser_UserName;
    public Text anotherUser_Local;
    public Text anotherUser_Hello;
    public Text anotherUser_DogName;
    public Text anotherUser_DogStyle;
    public Text anotherUser_DogAge;
    public Text anotherUser_DogWeight;
    public Text anotherUser_DogFood;
    public Text anotherUser_DogHospital;
    public Text anotherUser_Level;

    public GameObject AnotherUserPanel;

    public GameObject OtherUserHousePanelCheck;


    public GameObject CheckBox;

    private void Start()
    {
        GetFriendList();
        
    }

    [ContextMenu("친구 신청")]
    public void StartSendFriendRequest(string PlayfabID)
    {
        
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "SendFriendRequest",
            FunctionParameter = new { FriendPlayFabId = PlayfabID },
            GeneratePlayStreamEvent = true,  // ???????? ???????? ????
        }, OnFriendRequest, (error) => print("????"));
    }

    public void OnFriendRequest(ExecuteCloudScriptResult result)
    {
        CheckBox.SetActive(true);

        // JsonObject jsonResult = (JsonObject)result.FunctionResult;
        // jsonResult.TryGetValue("messageValue", out object messageValue);
        // Debug.Log((string)messageValue);

        GetFriendList();
    }


    /// /////////////////////
    public void GetFriendList()
    {
        var request = new GetFriendsListRequest()
        {
            IncludeSteamFriends = false,
            IncludeFacebookFriends = false,
            XboxToken = null,

            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowAvatarUrl = true,
                ShowDisplayName = true,
                ShowCreated = true,
                ShowLastLogin = true,
                ShowLocations = true
             }
        };
        PlayFabClientAPI.GetFriendsList(request, (result) => { print("?????????? ???? ????");StartCoroutine(DisplayFriends(result.Friends)); }, (error) => print("?????????? ???? ????"));
    }


    public void StartSendMessageRequest()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "SendMessageRequest",
            FunctionParameter = new { FriendPlayFabId = YourID, To = MyDisplayName, MailName = TitleMessege.text, MailDesc = DescMessege.text, ExpireHours = 9 },
            GeneratePlayStreamEvent = true,  // ???????? ???????? ????
        }, (result) => OnSendMessageSuccess(result), (error) => print("????"));
    }

    public void OnSendMessageSuccess(ExecuteCloudScriptResult result)
    {
        SendBox.transform.GetChild(0).gameObject.SetActive(false);
        SendBox.transform.GetChild(1).gameObject.SetActive(true);
        TitleMessege.text = "";
        DescMessege.text = "";
    }

    public void CancleMessage()
    {

        TitleMessege.text = "";
        DescMessege.text = "";

        SendBox.SetActive(false);
        CheckRemove.SetActive(true);
    }

    public void FriendsGetAnotherUserData()
    {
        var reqeust = new GetUserDataRequest()
        {
            PlayFabId = YourID
        };
        PlayFabClientAPI.GetUserData(reqeust, (result) =>
        {
            anotherUser_UserName.text = result.Data["UserName"].Value;
            anotherUser_Local.text = result.Data["Local"].Value;
            anotherUser_Hello.text = result.Data["Hello"].Value;
            anotherUser_DogName.text = result.Data["DogName"].Value;
            anotherUser_DogStyle.text = result.Data["DogStyle"].Value;
            anotherUser_DogAge.text = result.Data["DogAge"].Value;
            anotherUser_DogWeight.text = result.Data["DogWeight"].Value;
            anotherUser_DogFood.text = result.Data["DogFood"].Value;
            anotherUser_DogHospital.text = result.Data["DogHospital"].Value;
            anotherUser_Level.text = result.Data["Level"].Value.ToString();

            AnotherUserPanel.gameObject.SetActive(true);

        }, (error) => print("?????? ???????? ????"));
    }

    public void GetOtherUserData() 
    {
        var reqeust = new GetUserDataRequest()
        {
            PlayFabId = YourID
        };
        PlayFabClientAPI.GetUserData(reqeust, (result) =>
        {
            OtherUserData.instance.OhterUser_Name = result.Data["UserName"].Value;
            OtherUserData.instance.OhterUser_Local = result.Data["Local"].Value;
            OtherUserData.instance.OhterUser_Hello = result.Data["Hello"].Value;
            OtherUserData.instance.OhterDog1_Name = result.Data["DogName"].Value;
            OtherUserData.instance.OhterDog1_Style = result.Data["DogStyle"].Value;
            OtherUserData.instance.OhterDog1_Age = result.Data["DogAge"].Value;
            OtherUserData.instance.OhterDog1_Weight = result.Data["DogWeight"].Value;
            OtherUserData.instance.OhterDog1_Food = result.Data["DogFood"].Value;
            OtherUserData.instance.OhterDog1_Place = result.Data["DogHospital"].Value;
            OtherUserData.instance.OhterDog1_Level = int.Parse(result.Data["Level"].Value);

            Transitioner.Instance.TransitionToScene("OtherUserHouse");

        }, (error) => print("?????? ???????? ????"));

    }
    public void InputMessage()
    {
        SendBox.SetActive(true);
        SendBox.transform.GetChild(0).gameObject.SetActive(true);
        SendBox.transform.GetChild(1).gameObject.SetActive(false);

        SendBox.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<TMPro.TMP_Text>().text = MyDisplayName;
        SendBox.transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<TMPro.TMP_Text>().text = OtherUserName;

    }

    void DestroyOldFriends()
    {
        for (int i = 0; i < FriendVeiwList.Count; i++)
        {
            Destroy(FriendVeiwList[i].gameObject);
        }
    }




    IEnumerator DisplayFriends(List<FriendInfo> friends)
    {
        _friends = friends;
        
        DestroyOldFriends(); //??????

        // ???????? ???????? : item?? ?????????? ?????? ??????? ?????????????? ???? ????????.

        int confirmedCount = 0;
        int requesteeCount = 0;
        int requesterCount = 0;

        for (int i = 0; i < _friends.Count; i++)
        {
            GameObject item = Instantiate(FriendItem, FriendItemList);
            item.GetComponent<FriendInfor>().YourID = _friends[i].FriendPlayFabId;
            item.GetComponent<FriendInfor>().YourDisplayName = _friends[i].TitleDisplayName;
            item.GetComponent<FriendInfor>().Location = _friends[i].Profile.Locations[0].City.ToString();
            item.GetComponent<SearchFriend>().PlayfabID = _friends[i].FriendPlayFabId;

            DateTime lastLoginTime = Convert.ToDateTime(_friends[i].Profile.LastLogin);
            lastLoginTime = lastLoginTime.AddHours(9);

            Debug.Log("Frined : " + lastLoginTime);

            DateTime nowTime = DateTime.Now;

            TimeSpan dateDiff = nowTime - lastLoginTime;

            int diffDay = dateDiff.Days;
            int diffHour = dateDiff.Hours;
            int diffMinute = dateDiff.Minutes;
            int diffSecond = dateDiff.Seconds;

            string day;
            string hour;
            string min;

            if(diffDay > 0)
            {
                day = diffDay + "일";
            }
            else
            {
                day = "";
            }

            if (diffHour > 0)
            {
                hour = diffHour + "시간";
            }
            else
            {
                hour = "";
            }

            if (diffMinute > 0)
            {
                min = diffDay + "분";
            }
            else
            {
                min = "";
            }

            item.GetComponent<FriendInfor>().LastLoginTime = day + " " + hour + " " + min + " 전";

            FriendVeiwList.Add(item);
            item.transform.GetChild(0).GetComponent<TMP_Text>().text = _friends[i].TitleDisplayName;
            item.transform.GetChild(1).GetComponent<TMP_Text>().text = _friends[i].FriendPlayFabId;
            item.transform.GetChild(2).GetComponent<TMP_Text>().text = item.GetComponent<FriendInfor>().LastLoginTime;

            if (_friends[i].Tags[0].ToString() == "confirmed")
            {
                confirmedCount = confirmedCount + 1;
                item.transform.GetChild(3).gameObject.SetActive(true);
                item.transform.GetChild(4).gameObject.SetActive(false);
                item.transform.GetChild(5).gameObject.SetActive(false);
                item.transform.GetChild(6).gameObject.SetActive(false);
                item.GetComponent<SearchFriend>().Relationship = 1;
                // Debug.Log(_friends[i].TitleDisplayName + " : confirmed");
            }
            if (_friends[i].Tags[0].ToString() == "requestee")
            {
                requesteeCount = requesteeCount + 1;
                item.transform.SetParent(MyRequstFriendItemList);
                item.transform.GetChild(3).gameObject.SetActive(false);
                item.transform.GetChild(4).gameObject.SetActive(true);
                item.transform.GetChild(5).gameObject.SetActive(false);
                item.transform.GetChild(6).gameObject.SetActive(false);
                // Debug.Log(_friends[i].TitleDisplayName + " : requestee");
            }
            if (_friends[i].Tags[0].ToString() == "requester")
            {
                requesterCount = requesterCount + 1;
                item.transform.SetParent(WaitFriendItemList);
                item.transform.GetChild(3).gameObject.SetActive(false);
                item.transform.GetChild(4).gameObject.SetActive(false);
                item.transform.GetChild(5).gameObject.SetActive(true);
                item.transform.GetChild(6).gameObject.SetActive(true);
                //Debug.Log(_friends[i].TitleDisplayName + " : requester");

            }
            
        }
        FriendCount01.text = "내 친구 " + confirmedCount + "명";
        FriendCount02.text = "받은 친구요청 (" + requesterCount + ")";
        FriendCount03.text = "내가 신청한 친구요청 (" + requesteeCount + ")";

        yield return new WaitForSeconds(0.1f);
        FriendItemList.GetComponent<ContentSizeFitter>().enabled = false;
        MyRequstFriendItemList.GetComponent<ContentSizeFitter>().enabled = false;
        WaitFriendItemList.GetComponent<ContentSizeFitter>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        FriendItemList.GetComponent<ContentSizeFitter>().enabled = true;
        MyRequstFriendItemList.GetComponent<ContentSizeFitter>().enabled = true;
        WaitFriendItemList.GetComponent<ContentSizeFitter>().enabled = true;
        yield return new WaitForSeconds(0.1f);
        FriendItemList.GetComponent<ContentSizeFitter>().enabled = false;
        MyRequstFriendItemList.GetComponent<ContentSizeFitter>().enabled = false;
        WaitFriendItemList.GetComponent<ContentSizeFitter>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        FriendItemList.GetComponent<ContentSizeFitter>().enabled = true;
        MyRequstFriendItemList.GetComponent<ContentSizeFitter>().enabled = true;
        WaitFriendItemList.GetComponent<ContentSizeFitter>().enabled = true;
    }
}
