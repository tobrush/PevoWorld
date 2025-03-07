using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;
using System;


public class SearchFriend : MonoBehaviour
{
    public string PlayfabID;
    public OtherUserDataView OUDV;
    public int Relationship = 0;




    public void Start()
    {
        OUDV = GameObject.Find("DataManager").GetComponent<OtherUserDataView>();
    }

    public void UserInfomationOpen()
    {

        OUDV.OhterUserPanel.SetActive(true);
        OUDV.OhterUserPanel.transform.GetChild(0).gameObject.SetActive(true);
        OUDV.OhterUserPanel.transform.GetChild(1).gameObject.SetActive(false);

        

        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = PlayfabID
        }, result =>
        {
            OtherUserData.instance.OhterUser_ID = PlayfabID;
            OtherUserData.instance.OhterDog1_Character = result.Data["Dog1_Character"].Value;

            OUDV.FM.OtherUserName = result.Data["User_Name"].Value;

            PhotoManager.instance.OtherUserLoadAllFiles(result.Data["User_EntityID"].Value);

            OUDV.OtherUser_Name.text = result.Data["User_Name"].Value;
            OUDV.OtherUser_ID.text = PlayfabID;
            OUDV.OtherUser_Local.text = result.Data["User_Local"].Value;
            OUDV.OtherUser_Hello.text = result.Data["User_Hello"].Value;

            OUDV.OtherUser_LikeCount.text = result.Data["User_LikeCount"].Value;

           // OUDV.OtherUser_LikeIDs = JsonConvert.DeserializeObject<string>(result.Data["User_LikeIDs"].Value);

            OUDV.OtherUser_FriendsCount.text = result.Data["User_FriendsCount"].Value;
            // {"",""}
            // OUDV.OtherUser_FriendsIDs = JsonConvert.DeserializeObject<string>(result.Data["User_FriendsIDs"].Value);

            OUDV.OtherUser_PevoOne = result.Data["User_PevoOne"].Value;

            //check Friend icon / Like icon

            if(Relationship != 0)
            {
                OUDV.AddFriendBtn.SetActive(false);
                OUDV.GoHomeBtn.SetActive(true);
            }
            else
            {
                OUDV.AddFriendBtn.SetActive(true);
                OUDV.GoHomeBtn.SetActive(false);
            }

            OUDV.Dog1_Name.text = result.Data["Dog1_Name"].Value;
            OUDV.Dog1_Style.text = result.Data["Dog1_Style"].Value;
            
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

            OUDV.Dog1_Age.text = dog1_AgeYear + "살 " + dog1_AgeMonth + "개월";

            if (result.Data["Dog1_Sex"].Value == "M")
            {
                OUDV.Dog1_Sex.text = "남아";
            }
            else
            {
                OUDV.Dog1_Sex.text = "여아";

            }

            if (result.Data["Dog1_Neuter"].Value == "T")
            {
                OUDV.Dog1_Neuter.text = "중성화 O";
            }
            else
            {
                OUDV.Dog1_Neuter.text = "중성화 X";
            }

            OUDV.Dog1_Level.text = "Lv." + result.Data["Dog1_Level"].Value;
            //loading end

            OUDV.Dog2Slot.SetActive(false);
            OUDV.Dog3Slot.SetActive(false);
            OUDV.Dog2Line.SetActive(false);
            OUDV.Dog3Line.SetActive(false);

            


            if (int.Parse(result.Data["User_DogCount"].Value) > 1)
            {
                OUDV.Dog2Slot.SetActive(true);
                OUDV.Dog2Line.SetActive(true);
                OUDV.Dog2_Name.text = result.Data["Dog2_Name"].Value;
                OUDV.Dog2_Style.text = result.Data["Dog2_Style"].Value;

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

                OUDV.Dog2_Age.text = dog2_AgeYear + "살 " + dog2_AgeMonth + "개월";

                //
                if (result.Data["Dog2_Sex"].Value == "M")
                {
                    OUDV.Dog2_Sex.text = "남아";
                }
                else
                {
                    OUDV.Dog2_Sex.text = "여아";
                   
                }

                if (result.Data["Dog2_Neuter"].Value == "T")
                {
                    OUDV.Dog2_Neuter.text = "중성화 O";
                }
                else
                {
                    OUDV.Dog2_Neuter.text = "중성화 X";
                }

                OUDV.Dog2_Level.text = "Lv." + result.Data["Dog2_Level"].Value;
            }

            if (int.Parse(result.Data["User_DogCount"].Value) > 2)
            {
                OUDV.Dog3Slot.SetActive(true);
                OUDV.Dog3Line.SetActive(true);
                OUDV.Dog3_Name.text = result.Data["Dog3_Name"].Value;
                OUDV.Dog3_Style.text = result.Data["Dog3_Style"].Value;

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

                OUDV.Dog3_Age.text = dog3_AgeYear + "살 " + dog3_AgeMonth + "개월";
                //
                
                if (result.Data["Dog3_Sex"].Value == "M")
                {
                    OUDV.Dog3_Sex.text = "남아";
                }
                else
                {
                    OUDV.Dog3_Sex.text = "여아";

                }

                if (result.Data["Dog3_Neuter"].Value == "T")
                {
                    OUDV.Dog3_Neuter.text = "중성화 O";
                }
                else
                {
                    OUDV.Dog3_Neuter.text = "중성화 X";
                }
                OUDV.Dog3_Level.text = "Lv." + result.Data["Dog3_Level"].Value;
            }
           
            OUDV.OhterUserPanel.transform.GetChild(0).gameObject.SetActive(false);
            OUDV.OhterUserPanel.transform.GetChild(1).gameObject.SetActive(true);

        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });

    }
}
