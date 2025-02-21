using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class OtherUserManager : MonoBehaviour
{
    public Sprite[] chaimages;

    public string PlayfabID;
    public int Relationship = 0;

    public GameObject OhterUserPanel;
    public GameObject SendBox;
    public GameObject CheckBox;
    public TMP_InputField TitleMessege;
    public TMP_InputField DescMessege;

    public GameObject CheckRemove;

    public TMP_Text OtherUser_ID; // ?? ??????
    public TMP_Text OtherUser_Name; // ????????
    public TMP_Text OtherUser_Local; // ????
    public TMP_Text OtherUser_Hello; // ??????
    
    public TMP_Text OtherUser_LikeCount; // ?????? ??
    public TMP_Text OtherUser_FriendsCount; // ???? ??
    public string[] OtherUser_LikeIDs;
    public List<string> OtherUser_FriendsIDs;


    public TMP_Text OtherUser_SaveTime; // ???????????? ?? ????????
    public string OtherUser_DogCount; // ?????? ??
    public string OtherUser_PevoOne;


    public TMP_Text Dog1_Name; // ??????
    public TMP_Text Dog1_Style; // ????
    public TMP_Text Dog1_Age; // ??????
    public TMP_Text Dog1_Sex; // ????
    public TMP_Text Dog1_Neuter; // ??????
    public TMP_Text Dog1_Level; //????

    public GameObject Dog2Slot;
    public GameObject Dog2Line;
    public TMP_Text Dog2_Name; // ??????
    public TMP_Text Dog2_Style; // ????
    public TMP_Text Dog2_Age; // ??????
    public TMP_Text Dog2_Sex; // ????
    public TMP_Text Dog2_Neuter; // ??????
    public TMP_Text Dog2_Level;

    public GameObject Dog3Slot;
    public GameObject Dog3Line;
    public TMP_Text Dog3_Name; // ??????
    public TMP_Text Dog3_Style; // ????
    public TMP_Text Dog3_Age; // ??????
    public TMP_Text Dog3_Sex; // ????
    public TMP_Text Dog3_Neuter; // ??????
    public TMP_Text Dog3_Level; //????

    public Image PlayerPhoto_Multi;
    public Image Dog1Photo_Multi;
    public Image Dog2Photo_Multi;
    public Image Dog3Photo_Multi;

    public GameObject AddFriendBtn;
    public GameObject GoHomeBtn;

    public void Start()
    {
        PhotoManager.instance.otherUserManager = this;
    }
    public void LikeAdd()
    {

    }

    public void FriendAdd()
    {

    }
    public void SendMessage()
    {
        SendBox.transform.GetChild(0).gameObject.SetActive(true);
        SendBox.transform.GetChild(1).gameObject.SetActive(false);

        SendBox.transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<TMPro.TMP_Text>().text = UserData.instance.User_Name;
        SendBox.transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<TMPro.TMP_Text>().text = OtherUser_Name.text;

        SendBox.SetActive(true);
    }
    public void StartSendMessageRequest()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "SendMessageRequest",
            FunctionParameter = new { FriendPlayFabId = OtherUser_ID.text, To = UserData.instance.User_Name, MailName = TitleMessege.text, MailDesc = DescMessege.text, ExpireHours = 9 },
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

    public void FriendRequst()
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
    }
    public void ToYourHome()
    {
        UserData.instance.gameObject.GetComponent<OtherUserData>().OhterUser_ID = OtherUser_ID.text;

        Transitioner.Instance.TransitionToScene("OtherUserHome");
    }




    public void UserInfomationOpen(string _playfabID)
    {

        OhterUserPanel.SetActive(true);
        OhterUserPanel.transform.GetChild(0).gameObject.SetActive(true);
        OhterUserPanel.transform.GetChild(1).gameObject.SetActive(false);

        PlayfabID = _playfabID;

        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = PlayfabID
        }, result =>
        {
            //FM.OtherUserName = result.Data["User_Name"].Value;

            PhotoManager.instance.OtherUserLoadAllFiles(result.Data["User_EntityID"].Value);

            OtherUser_Name.text = result.Data["User_Name"].Value;
            OtherUser_ID.text = PlayfabID;
            OtherUser_Local.text = result.Data["User_Local"].Value;
            OtherUser_Hello.text = result.Data["User_Hello"].Value;

            OtherUser_LikeCount.text = result.Data["User_LikeCount"].Value;

            // OUDV.OtherUser_LikeIDs = JsonConvert.DeserializeObject<string>(result.Data["User_LikeIDs"].Value);

            OtherUser_FriendsCount.text = result.Data["User_FriendsCount"].Value;
            // {"",""}
            // OUDV.OtherUser_FriendsIDs = JsonConvert.DeserializeObject<string>(result.Data["User_FriendsIDs"].Value);

            OtherUser_PevoOne = result.Data["User_PevoOne"].Value;

            //check Friend icon / Like icon

            if (Relationship != 0)
            {
                AddFriendBtn.SetActive(false);
                GoHomeBtn.SetActive(true);
            }
            else
            {
                AddFriendBtn.SetActive(true);
                GoHomeBtn.SetActive(false);
            }

            Dog1_Name.text = result.Data["Dog1_Name"].Value;
            Dog1_Style.text = result.Data["Dog1_Style"].Value;

            DateTime dog1_BirthDate = DateTime.Parse(result.Data["Dog1_Age"].Value);
            int dog1_BirthYear = dog1_BirthDate.Year;
            int dog1_BirthMonth = dog1_BirthDate.Month;
            int dog1_BirthDay = dog1_BirthDate.Day;

            DateTime dog1_nowDate = DateTime.Now;
            int dog1_NowYear = dog1_nowDate.Year;
            int dog1_NowMonth = dog1_nowDate.Month;
            int dog1_NowDay = dog1_nowDate.Day;

            int dog1_AgeYear;
            int dog1_AgeMonth;
            int dog1_AgeDay;

            if (dog1_NowDay >= dog1_BirthDay)
            {
                dog1_AgeDay = dog1_NowDay - dog1_BirthDay;
            }
            else
            {
                dog1_NowMonth -= 1;
                dog1_AgeDay = DateTime.DaysInMonth(dog1_NowYear, dog1_NowMonth) + dog1_NowDay - dog1_BirthDay;

                if (dog1_AgeDay > 15)
                {
                    dog1_NowMonth += 1;
                }
            }


            if (dog1_NowMonth >= dog1_BirthMonth)
            {
                dog1_AgeMonth = dog1_NowMonth - dog1_BirthMonth;
            }
            else
            {
                dog1_NowYear -= 1;
                dog1_AgeMonth = 12 + dog1_NowMonth - dog1_BirthMonth;
            }

            dog1_AgeYear = dog1_NowYear - dog1_BirthYear;

            Dog1_Age.text = dog1_AgeYear + "?? " + dog1_AgeMonth + "????";

            if (result.Data["Dog1_Sex"].Value == "M")
            {
                Dog1_Sex.text = "????";
            }
            else
            {
                Dog1_Sex.text = "????";

            }

            if (result.Data["Dog1_Neuter"].Value == "T")
            {
                Dog1_Neuter.text = "?????? O";
            }
            else
            {
                Dog1_Neuter.text = "?????? X";
            }

           Dog1_Level.text = "Lv." + result.Data["Dog1_Level"].Value;
            //loading end

            Dog2Slot.SetActive(false);
            Dog3Slot.SetActive(false);
            Dog2Line.SetActive(false);
            Dog3Line.SetActive(false);

            if (int.Parse(result.Data["User_DogCount"].Value) > 1)
            {
                Dog2Slot.SetActive(true);
                Dog2Line.SetActive(true);
                Dog2_Name.text = result.Data["Dog2_Name"].Value;
                Dog2_Style.text = result.Data["Dog2_Style"].Value;

                DateTime dog2_BirthDate = DateTime.Parse(result.Data["Dog2_Age"].Value);
                int dog2_BirthYear = dog2_BirthDate.Year;
                int dog2_BirthMonth = dog2_BirthDate.Month;
                int dog2_BirthDay = dog2_BirthDate.Day;

                DateTime dog2_nowDate = DateTime.Now;
                int dog2_NowYear = dog2_nowDate.Year;
                int dog2_NowMonth = dog2_nowDate.Month;
                int dog2_NowDay = dog2_nowDate.Day;

                int dog2_AgeYear;
                int dog2_AgeMonth;
                int dog2_AgeDay;

                if (dog2_NowDay >= dog2_BirthDay)
                {
                    dog2_AgeDay = dog2_NowDay - dog2_BirthDay;
                }
                else
                {
                    dog2_NowMonth -= 1;
                    dog2_AgeDay = DateTime.DaysInMonth(dog2_NowYear, dog2_NowMonth) + dog2_NowDay - dog2_BirthDay;

                    if (dog2_AgeDay > 15)
                    {
                        dog2_NowMonth += 1;
                    }
                }


                if (dog2_NowMonth >= dog2_BirthMonth)
                {
                    dog2_AgeMonth = dog2_NowMonth - dog2_BirthMonth;
                }
                else
                {
                    dog2_NowYear -= 1;
                    dog2_AgeMonth = 12 + dog2_NowMonth - dog2_BirthMonth;
                }

                dog2_AgeYear = dog2_NowYear - dog2_BirthYear;

                Dog2_Age.text = dog2_AgeYear + "?? " + dog2_AgeMonth + "????";

                //
                if (result.Data["Dog2_Sex"].Value == "M")
                {
                    Dog2_Sex.text = "????";
                }
                else
                {
                    Dog2_Sex.text = "????";

                }

                if (result.Data["Dog2_Neuter"].Value == "T")
                {
                    Dog2_Neuter.text = "?????? O";
                }
                else
                {
                    Dog2_Neuter.text = "?????? X";
                }

                Dog2_Level.text = "Lv." + result.Data["Dog2_Level"].Value;
            }

            if (int.Parse(result.Data["User_DogCount"].Value) > 2)
            {
                Dog3Slot.SetActive(true);
                Dog3Line.SetActive(true);
                Dog3_Name.text = result.Data["Dog3_Name"].Value;
                Dog3_Style.text = result.Data["Dog3_Style"].Value;

                DateTime dog3_BirthDate = DateTime.Parse(result.Data["Dog3_Age"].Value);
                int dog3_BirthYear = dog3_BirthDate.Year;
                int dog3_BirthMonth = dog3_BirthDate.Month;
                int dog3_BirthDay = dog3_BirthDate.Day;

                DateTime dog3_nowDate = DateTime.Now;
                int dog3_NowYear = dog3_nowDate.Year;
                int dog3_NowMonth = dog3_nowDate.Month;
                int dog3_NowDay = dog3_nowDate.Day;

                int dog3_AgeYear;
                int dog3_AgeMonth;
                int dog3_AgeDay;

                if (dog3_NowDay >= dog3_BirthDay)
                {
                    dog3_AgeDay = dog3_NowDay - dog3_BirthDay;
                }
                else
                {
                    dog3_NowMonth -= 1;
                    dog3_AgeDay = DateTime.DaysInMonth(dog3_NowYear, dog3_NowMonth) + dog3_NowDay - dog3_BirthDay;

                    if (dog3_AgeDay > 15)
                    {
                        dog3_NowMonth += 1;
                    }
                }


                if (dog3_NowMonth >= dog3_BirthMonth)
                {
                    dog3_AgeMonth = dog3_NowMonth - dog3_BirthMonth;
                }
                else
                {
                    dog3_NowYear -= 1;
                    dog3_AgeMonth = 12 + dog3_NowMonth - dog3_BirthMonth;
                }

                dog3_AgeYear = dog3_NowYear - dog3_BirthYear;

                Dog3_Age.text = dog3_AgeYear + "?? " + dog3_AgeMonth + "????";
                //

                if (result.Data["Dog3_Sex"].Value == "M")
                {
                    Dog3_Sex.text = "????";
                }
                else
                {
                    Dog3_Sex.text = "????";

                }

                if (result.Data["Dog3_Neuter"].Value == "T")
                {
                    Dog3_Neuter.text = "?????? O";
                }
                else
                {
                    Dog3_Neuter.text = "?????? X";
                }
                Dog3_Level.text = "Lv." + result.Data["Dog3_Level"].Value;
            }

            OhterUserPanel.transform.GetChild(0).gameObject.SetActive(false);
            OhterUserPanel.transform.GetChild(1).gameObject.SetActive(true);

        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });

    }
}
