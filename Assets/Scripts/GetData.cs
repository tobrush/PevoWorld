using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.Json;
using PlayFab.ClientModels;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GetData : MonoBehaviour
{
    const string URL = "https://script.google.com/macros/s/AKfycbytD52hrz0NZFjRXXimm-rCPasTlvZ_c4wlcTURJwuV5sHLBTRqNzot4IFsn5WTVKY_/exec";



    public FriendsManager friendManager;
    public MailBoxManager mailBoxManager;
    public ConfigManager configManager;
    public InventoryManager inventoryManager;

    public Image dog1_icon, dog2_icon, dog3_icon, dog1_image, dog2_image, dog3_image, player_image, player_dog1, player_dog2, player_dog3, petCard1, petCard2, petCard3, SignAdd;

    public TMP_Text TitleUserNameText;
    public TMP_Text PlayerWinBtnText1, PlayerWinBtnText2, PlayerWinBtnText3;
    public TMP_Text TitleDog1_LevelText;
    public Image TitleDog1_Exp;
    public TMP_Text TitleDog2_LevelText;
    public Image TitleDog2_Exp;
    public TMP_Text TitleDog3_LevelText;
    public Image TitleDog3_Exp;

    public TMP_Text FriendsCount;
    public TMP_Text LikeCount;

    public TMP_Text PlayerName, PlayerID, PlayerLocal, PlayerIntro;
    public TMP_Text PlayerDog1_Name, PlayerDog1_Level, PlayerDog1_Style, PlayerDog1_age, PlayerDog1_Gender, PlayerDog1_Neuter;
    public TMP_Text PlayerDog2_Name, PlayerDog2_Level, PlayerDog2_Style, PlayerDog2_age, PlayerDog2_Gender, PlayerDog2_Neuter;
    public TMP_Text PlayerDog3_Name, PlayerDog3_Level, PlayerDog3_Style, PlayerDog3_age, PlayerDog3_Gender, PlayerDog3_Neuter;


    public TMP_Text dog1_Name, dog2_Name, dog3_Name;
    public TMP_Text dog1_Style, dog2_Style, dog3_Style;
    public TMP_Text dog1_Level, dog2_Level, dog3_Level;
    public Slider dog1_ExpBar, dog2_ExpBar, dog3_ExpBar;
    public TMP_Text dog1_Local, dog2_Local, dog3_Local;
    public TMP_Text dog1_Age, dog2_Age, dog3_Age;
    public TMP_Text dog1_Sex, dog2_Sex, dog3_Sex;
    public TMP_Text dog1_Weight, dog2_Weight, dog3_Weight;

    public GameObject PlayerDogShotWin1, ShitSlash1, PlayerDogShotWin2, ShitSlash2, PlayerDogShotWin3;
    public GameObject DogSlot01, DogSlot02, DogSlot03;


    public Slider dog1_HungryBar, dog2_HungryBar, dog3_HungryBar;
    public Slider dog1_HealthBar, dog2_HealthBar, dog3_HealthBar;
    public Slider dog1_HappyBar, dog2_HappyBar, dog3_HappyBar;


    [Header("Edit")]

    public GameObject EditPanel;
    public GameObject EditCheckPanel;
    public GameObject EditResultPanel;
    //private UserData NewUserdata = new UserData();

    public TMP_InputField NewUserName;
    public TMP_InputField NewLocal;
    public TMP_InputField NewHello;


    public Image ProfilePhoto1;
    public Image ProfilePhoto2;


    public Slider HealthBar;
    public Slider HungryBar;
    public Slider TirstyBar;
    public Slider CleanBar;
    public Slider HappyBar;

    public static int stat_Min = 0;
    public static int stat_Max = 99;


    public TMP_InputField Edit_userNameInput;
    public TMP_Text edit_userNameGray;
    public TMP_InputField Edit_userHelloInput;
    public TMP_Text edit_userHelloGray;

    public GoogleData GD;
    public TMP_InputField AddressInput;
    string address;
    public GameObject NewLocalEdit ,itemLocalCard, tooManyResult;
    public Button LocalNextBtn;
    public Transform LocalList;


    public GameObject PetCardPage, cardList, toggleList, dog2Card, dog2Toggle, dog3Card, dog3Toggle;
    public Image card_Dog1pic, card_Dog2pic, card_Dog3pic;
    public TMP_Text card_Dog1name, card_Dog1id, card_Dog1style, card_Dog1birthday, card_Dog1petid, card_Dog1adopted, card_Dog1gender, card_Dog1weight;
    public TMP_Text card_Dog2name, card_Dog2id, card_Dog2style, card_Dog2birthday, card_Dog2petid, card_Dog2adopted, card_Dog2gender, card_Dog2weight;
    public TMP_Text card_Dog3name, card_Dog3id, card_Dog3style, card_Dog3birthday, card_Dog3petid, card_Dog3adopted, card_Dog3gender, card_Dog3weight;
    public GameObject Dog1_PetRgtBtn, Dog2_PetRgtBtn, Dog3_PetRgtBtn;

    /*
    public void SetNewData()
    { 
        var reqeust = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
              {"User_Name", NewUserdata.User_Name}, {"User_Local", NewUserdata.User_Local}, {"User_Hello", NewUserdata.User_Hello}
            },
            Permission = UserDataPermission.Public

        };
        PlayFabClientAPI.UpdateUserData(reqeust, (result) => { print("User ?????? ???? ????"); SetDisplayName(); MainLobbyData(); ResetProfile(); }, (error) => FailedEditProfile());

    }
      */

    public void SetDisplayName(string NewName)
    {
        var request = new UpdateUserTitleDisplayNameRequest { DisplayName = NewName };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, (result) => { print("DisplayName ?????? ???? ????"); }, (error) => print("DisplayName ?????? ???? ????"));
    }

    public void GetLocal()
    {
        LocalNextBtn.interactable = false;
        LocalNextBtn.transform.GetChild(0).gameObject.SetActive(false);
        LocalNextBtn.transform.GetChild(1).gameObject.SetActive(true);

        Transform[] childList = LocalList.GetComponentsInChildren<Transform>();

        if (childList != null)
        {
            for (int i = 1; i < childList.Length; i++)
            {
                if (childList[i] != transform)
                {
                    Destroy(childList[i].gameObject);
                }
            }
        }

        address = AddressInput.text.Trim();

        WWWForm form = new WWWForm();
        form.AddField("order", "localSearch");
        form.AddField("address", address);

        StartCoroutine(Post(form));
    }


    public void closeEditLocal()
    {
        address = "";
        AddressInput.text = "";

        Transform[] childList = LocalList.GetComponentsInChildren<Transform>();
        if (childList != null)
        {
            for (int i = 1; i < childList.Length; i++)
            {
                if (childList[i] != transform)
                {
                    Destroy(childList[i].gameObject);
                }
            }
        }
        NewLocalEdit.SetActive(false);
    }

    public void EditMyLocal()
    {
        var reqeust = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
              {"User_Local", UserData.instance.editDummyData}
            },
            Permission = UserDataPermission.Public

        };
        PlayFabClientAPI.UpdateUserData(reqeust, (result) =>
        {
            UserData.instance.User_Local = UserData.instance.editDummyData;
            PlayerLocal.text = UserData.instance.editDummyData;
            dog1_Local.text = UserData.instance.editDummyData;
            dog2_Local.text = UserData.instance.editDummyData;
            dog3_Local.text = UserData.instance.editDummyData;
            address = "";
            AddressInput.text = "";

            Transform[] childList = LocalList.GetComponentsInChildren<Transform>();
            if (childList != null)
            {
                for (int i = 1; i < childList.Length; i++)
                {
                    if (childList[i] != transform)
                    {
                        Destroy(childList[i].gameObject);
                    }
                }
            }

            //local lat, lon add
            NewLocalEdit.SetActive(false);
        }, (error) => FailedEditProfile());

        WWWForm form = new WWWForm();
        form.AddField("order", "editLocal");
        form.AddField("editData", UserData.instance.editDummyData);
        form.AddField("pass", UserData.instance.User_MyID);
        StartCoroutine(Post(form));

    }

    

    public void EditMyProfile()
    {
        var reqeust = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
               {"User_Hello", Edit_userHelloInput.text}       //{"User_Name", Edit_userNameInput.text},
            },
            Permission = UserDataPermission.Public

        };
        PlayFabClientAPI.UpdateUserData(reqeust, (result) =>
        {
            //UserData.instance.User_Name = Edit_userNameInput.text;
            UserData.instance.User_Hello = Edit_userHelloInput.text;
            PlayerIntro.text = Edit_userHelloInput.text;
            //SetDisplayName(Edit_userNameInput.text);
            EmptyInfo();

           // TitleUserNameText.text = Edit_userNameInput.text;
           // PlayerWinBtnText1.text = Edit_userNameInput.text + "님의 정보";
           // PlayerWinBtnText2.text = Edit_userNameInput.text + "님의 정보";
           // PlayerWinBtnText3.text = Edit_userNameInput.text + "님의 정보";
           // edit_userNameGray.text = Edit_userNameInput.text;
            edit_userHelloGray.text = Edit_userHelloInput.text;
            // PlayerName.text = Edit_userNameInput.text;
            //  friendManager.MyDisplayName = Edit_userNameInput.text;
            // configManager.MyName.text = Edit_userNameInput.text;

        }, (error) => FailedEditProfile());

        // - change sheet my nickname
      //  WWWForm form = new WWWForm();
      //  form.AddField("order", "editNickName");
      //  form.AddField("editData", Edit_userNameInput.text);
      //  form.AddField("pass", UserData.instance.User_MyID);
      //  StartCoroutine(Post(form));

    }

    IEnumerator Post(WWWForm form)
    {
        using (UnityWebRequest www = UnityWebRequest.Post(URL, form)) 
        {
            yield return www.SendWebRequest();

            if (www.isDone) Response(www.downloadHandler.text);
            else print("???? ?????? ????????.");
        }
    }

    void Response(string json)
    {
        if (string.IsNullOrEmpty(json)) return;

        print(json);

        GD = JsonConvert.DeserializeObject<GoogleData>(json);
        //GD = JsonUtility.FromJson<GoogleData>(json);

        if (GD.result == "Found")
        {
            LocalNextBtn.transform.GetChild(0).gameObject.SetActive(true);
            LocalNextBtn.transform.GetChild(1).gameObject.SetActive(false);

            if (GD.localData.Count < 200)
            {

                //print(GD.localData.Count);
                for (int i = 0; i < GD.localData.Count; i++)
                {
                    GameObject itemCard = Instantiate(itemLocalCard, LocalList);
                    string ResultAddress = GD.localData[i][0] + " " + GD.localData[i][1] + " " + GD.localData[i][2] + " " + GD.localData[i][3] + " " + GD.localData[i][4];

                    itemCard.transform.GetChild(1).GetComponent<TMP_Text>().text = ResultAddress;

                }
            }
            else
            {
                GameObject itemCard = Instantiate(tooManyResult, LocalList);
            }


            return;
        }


    }

    public void EmptyInfo()
    {
        Edit_userNameInput.text = "";
        Edit_userHelloInput.text = "";
        edit_userNameGray.text = UserData.instance.User_Name;
        edit_userHelloGray.text = UserData.instance.User_Hello;
    }


    /*public void ResetProfile()
    {
        EmptyInfo();

        EditCheckPanel.SetActive(false);
        EditPanel.SetActive(false);
        EditResultPanel.SetActive(true);
        EditResultPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "?????????? ??????????????.";
    }
    */
    public void FailedEditProfile()
    {
        EmptyInfo();

        //EditCheckPanel.SetActive(false);
        //EditPanel.SetActive(false);
       // EditResultPanel.SetActive(true);
       // EditResultPanel.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "?????? ?????? ????????????. ?????????? ???? ????????????";
    }
    

    /*
    public void EditProfile()
    {
       
        if(NewUserName.text != "")
        {
            NewUserdata.User_Name = NewUserName.text;
        }
        else
        {
            NewUserdata.User_Name = UserData.instance.User_Name;
        }
        
        if(NewLocal.text != "")
        {
            NewUserdata.User_Local = NewLocal.text;
        }
        else
        {
            NewUserdata.User_Local = UserData.instance.User_Local;
        }

        if(NewHello.text != "")
        {
            NewUserdata.User_Hello = NewHello.text;
        }
        else
        {
            NewUserdata.User_Hello = UserData.instance.User_Hello;
        }


        SetNewData();

    }
    */

    public void Start()
    {
       
        PhotoManager.instance.OUD = OtherUserData.instance;
        
        MainLobbyData();
    }


    public void Update()
    {
        dog1_HungryBar.value = (UserData.instance.Dog1_Hungry + UserData.instance.Dog1_Tirsty) / 200f;   // changeing
        dog1_HealthBar.value = UserData.instance.Dog1_Health / 100f;  // changeing
        dog1_HappyBar.value = UserData.instance.Dog1_Happy / 100f;  // changeing
    }


    public void basicFoodSet()
    {
     
        if (UserData.instance.Dog1_basicFood_Name != "")
        {
            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
            {
                FunctionName = "MyFoodAdd1",
                FunctionParameter = new { IconTag = 0, FoodName = UserData.instance.Dog1_basicFood_Name, Brand = UserData.instance.Dog1_basicFood_Brand, Kcal = UserData.instance.Dog1_basicFood_cal, Open = 1 },
                GeneratePlayStreamEvent = true,  // ???????? ???????? ????
            }, (result) => OnSendMyFoodSuccess(result), (error) => print("????"));
        }
    }

    public void OnSendMyFoodSuccess(ExecuteCloudScriptResult result)
    {
        //???????????? ???????????? ??????

        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = UserData.instance.User_MyID
        }, result => {
            UserData.instance.Dog1_Food = result.Data["Dog1_Food"].Value;

            this.gameObject.GetComponent<MyFoodManager>().ResetFoodList();
        }, error => {
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public void MainLobbyData()
    {
        basicFoodSet();


        DateTime lastLoginTime = DateTimeOffset.Parse(UserData.instance.User_SaveTime).UtcDateTime;
        DateTime nowTime = DateTime.Now;

        TimeSpan dateDiff = nowTime - lastLoginTime;

        int diffDay = dateDiff.Days;
        int diffHour = dateDiff.Hours;
        int diffMinute = dateDiff.Minutes;
        int diffSecond = dateDiff.Seconds;

        string day = diffDay + "일";
        string hour = diffHour + "시간";
        string min = diffMinute + "분";

        //Edit_userNameInput.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = UserData.instance.User_Name;
        Edit_userHelloInput.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>().text = UserData.instance.User_Hello;

        Debug.Log(day + " " + hour + " " + min + " ??");

        UserData.instance.Dog1_Health = UserData.instance.Dog1_Health - (((diffDay * 24) + diffHour) * 0.3f);  // ?????? 0.3?? ????
        UserData.instance.Dog1_Hungry = UserData.instance.Dog1_Hungry - ((((diffDay * 24) + diffHour) * 6) + diffMinute / 6) * 0.2f; // 10???? 0.2 ???? (?????? 1.2?? ????)
        UserData.instance.Dog1_Tirsty = UserData.instance.Dog1_Tirsty - ((((diffDay * 24) + diffHour) * 6) + diffMinute / 6) * 0.3f; // 10???? 0.3???? (?????? 1.8?? ????)
        UserData.instance.Dog1_Clean = UserData.instance.Dog1_Clean - ((((diffDay * 24) + diffHour) * 6) + diffMinute / 6) * 0.1f; // 10???? 0.1 ???? (?????? 0.6????) 
        UserData.instance.Dog1_Happy = UserData.instance.Dog1_Happy - ((diffDay * 24) + diffHour) * 2;  // ?????? 2 ????

        UserData.instance.Dog1_Health = UserData.instance.Dog1_Health < 0 ? 0 : UserData.instance.Dog1_Health;
        UserData.instance.Dog1_Hungry = UserData.instance.Dog1_Hungry < 0 ? 0 : UserData.instance.Dog1_Hungry;
        UserData.instance.Dog1_Tirsty = UserData.instance.Dog1_Tirsty < 0 ? 0 : UserData.instance.Dog1_Tirsty;
        UserData.instance.Dog1_Clean = UserData.instance.Dog1_Clean < 0 ? 0 : UserData.instance.Dog1_Clean;
        UserData.instance.Dog1_Happy = UserData.instance.Dog1_Happy < 0 ? 0 : UserData.instance.Dog1_Happy;

        


        // Main Display UI 
        TitleUserNameText.text = UserData.instance.User_Name;
        Debug.Log("nickName crack");

        PlayerWinBtnText1.text = UserData.instance.User_Name + "님의 정보";
        PlayerWinBtnText2.text = UserData.instance.User_Name + "님의 정보";
        PlayerWinBtnText3.text = UserData.instance.User_Name + "님의 정보";

        FriendsCount.text = UserData.instance.User_FriendsCount.ToString();
        LikeCount.text = UserData.instance.User_LikeCount.ToString();

        TitleDog1_LevelText.text = UserData.instance.Dog1_Level.ToString();
        TitleDog1_Exp.fillAmount = (UserData.instance.Dog1_Exp - UserData.instance.expTable[UserData.instance.Dog1_Level - 1] / UserData.instance.expTable[UserData.instance.Dog1_Level]);

        PlayerName.text = UserData.instance.User_Name;
        PlayerID.text = UserData.instance.User_MyID;
        PlayerLocal.text = UserData.instance.User_Local;
        PlayerIntro.text = UserData.instance.User_Hello;


    // my Payge UI

    dog1_Name.text = UserData.instance.Dog1_Name;
        dog1_Style.text = UserData.instance.Dog1_Style;
        dog1_Level.text = "LEVEL " + UserData.instance.Dog1_Level.ToString();
        dog1_Local.text = UserData.instance.User_Local;
        dog1_ExpBar.value = (UserData.instance.Dog1_Exp - UserData.instance.expTable[UserData.instance.Dog1_Level - 1] / UserData.instance.expTable[UserData.instance.Dog1_Level]);

        dog1_HungryBar.value = (UserData.instance.Dog1_Hungry + UserData.instance.Dog1_Tirsty) / 200f;   // changeing
        dog1_HealthBar.value = UserData.instance.Dog1_Health / 100f;  // changeing
        dog1_HappyBar.value = UserData.instance.Dog1_Happy / 100f;  // changeing


        DateTime dog1_BirthDate = DateTime.Parse(UserData.instance.Dog1_Age);
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

        dog1_Age.text = dog1_AgeYear + "살 " + dog1_AgeMonth + "개월";
        PlayerDog1_age.text = dog1_AgeYear + "살 " + dog1_AgeMonth + "개월";

        if (UserData.instance.Dog1_Sex == "M")
        {
            PlayerDog1_Gender.text = "남아";

            if (UserData.instance.Dog1_Neuter == "T")
            {
                dog1_Sex.text = "♂ / O";
                card_Dog1gender.text = "남아 / 중성화 O";
                PlayerDog1_Neuter.text = "중성화 O";
            }
            else
            {
                dog1_Sex.text = "♂ / X";
                card_Dog1gender.text = "남아 / 중성화 X";
                PlayerDog1_Neuter.text = "중성화 X";
            }

        }
        else
        {
            PlayerDog1_Gender.text = "여아";

            if (UserData.instance.Dog1_Neuter == "T")
            {
                dog1_Sex.text = "♀ / O";
                card_Dog1gender.text = "여아 / 중성화 O";
                PlayerDog1_Neuter.text = "중성화 O";

            }
            else
            {
                dog1_Sex.text = "♀ / X";
                card_Dog1gender.text = "여아 / 중성화 X";
                PlayerDog1_Neuter.text = "중성화 X";
            }
        }

        dog1_Weight.text = UserData.instance.Dog1_Weight +"kg";

        // Get Photo File
        PhotoManager.instance.getData = this;
        PhotoManager.instance.LoadAllFiles();


        //Get Pevo DogData
        //Get MyFood, MyPlace / Json DogData


        //Player Page
        PlayerDog1_Name.text = UserData.instance.Dog1_Name;
        PlayerDog1_Level.text = "Level " + UserData.instance.Dog1_Level;
        PlayerDog1_Style.text = UserData.instance.Dog1_Style;

        //Pet Card

        card_Dog1name.text = UserData.instance.Dog1_Name;
        card_Dog1id.text = UserData.instance.Dog1_PevoNumber;
        card_Dog1style.text = UserData.instance.Dog1_Style;
        card_Dog1birthday.text = dog1_BirthYear + "년 " + dog1_BirthMonth + "월 " + dog1_BirthDay + "일" + "\n" + "(" + dog1_AgeYear + "살 " + dog1_AgeMonth + "개월" + ")";

        if (UserData.instance.Dog1_PetRgtNo != null || UserData.instance.Dog1_PetRgtNo != "")
        {
            card_Dog1petid.text = UserData.instance.Dog1_PetRgtNo;
            Dog1_PetRgtBtn.SetActive(false);
        }
        else
        {
            Dog1_PetRgtBtn.SetActive(true);
        }



        DateTime dog1_AdoptedDate = DateTime.Parse(UserData.instance.Dog1_Adopted);
        int dog1_AdoptedYear = dog1_AdoptedDate.Year;
        int dog1_AdoptedMonth = dog1_AdoptedDate.Month;
        int dog1_AdoptedDay = dog1_AdoptedDate.Day;

        card_Dog1adopted.text = dog1_AdoptedYear + "년 " + dog1_AdoptedMonth + "월 " + dog1_AdoptedDay + "일";
        card_Dog1weight.text = UserData.instance.Dog1_Weight + "kg";

        if (UserData.instance.User_DogCount == 1)
        {
            ShitSlash1.SetActive(false);
            PlayerDogShotWin2.SetActive(false);
            ShitSlash2.SetActive(false);
            PlayerDogShotWin3.SetActive(false);
            DogSlot02.SetActive(false);
            DogSlot03.SetActive(false);

            //UserData.instance.Dog1_Food
            //UserData.instance.Dog1_Place

            dog2Card.SetActive(false);
            dog2Toggle.SetActive(false);
            dog3Card.SetActive(false);
            dog3Toggle.SetActive(false);
        }


        if (UserData.instance.User_DogCount > 1)
        {
 
            ShitSlash1.SetActive(true);
            PlayerDogShotWin2.SetActive(true);
            ShitSlash2.SetActive(false);
            PlayerDogShotWin3.SetActive(false);
            DogSlot02.SetActive(true);
            DogSlot03.SetActive(false);

            TitleDog2_LevelText.text = UserData.instance.Dog2_Level.ToString();
            TitleDog2_Exp.fillAmount = (UserData.instance.Dog2_Exp - UserData.instance.expTable[UserData.instance.Dog2_Level - 1] / UserData.instance.expTable[UserData.instance.Dog2_Level]);

            UserData.instance.Dog2_Health = UserData.instance.Dog2_Health - (((diffDay * 24) + diffHour) * 0.3f);  // ?????? 0.3?? ????
            UserData.instance.Dog2_Hungry = UserData.instance.Dog2_Hungry - ((((diffDay * 24) + diffHour) * 6) + diffMinute / 6) * 0.2f; // 10???? 0.2 ???? (?????? 1.2?? ????)
            UserData.instance.Dog2_Tirsty = UserData.instance.Dog2_Tirsty - ((((diffDay * 24) + diffHour) * 6) + diffMinute / 6) * 0.3f; // 10???? 0.3???? (?????? 1.8?? ????)
            UserData.instance.Dog2_Clean = UserData.instance.Dog2_Clean - ((((diffDay * 24) + diffHour) * 6) + diffMinute / 6) * 0.1f; // 10???? 0.1 ???? (?????? 0.6????) 
            UserData.instance.Dog2_Happy = UserData.instance.Dog2_Happy - ((diffDay * 24) + diffHour) * 2;  // ?????? 2 ????

            UserData.instance.Dog2_Health = UserData.instance.Dog2_Health < 0 ? 0 : UserData.instance.Dog2_Health;
            UserData.instance.Dog2_Hungry = UserData.instance.Dog2_Hungry < 0 ? 0 : UserData.instance.Dog2_Hungry;
            UserData.instance.Dog2_Tirsty = UserData.instance.Dog2_Tirsty < 0 ? 0 : UserData.instance.Dog2_Tirsty;
            UserData.instance.Dog2_Clean = UserData.instance.Dog2_Clean < 0 ? 0 : UserData.instance.Dog2_Clean;
            UserData.instance.Dog2_Happy = UserData.instance.Dog2_Happy < 0 ? 0 : UserData.instance.Dog2_Happy;

            dog2_Name.text = UserData.instance.Dog2_Name;
            dog2_Style.text = UserData.instance.Dog2_Style;
            dog2_Level.text = "LEVEL " + UserData.instance.Dog2_Level.ToString();
            dog2_Local.text = UserData.instance.User_Local;
            dog2_ExpBar.value = (UserData.instance.Dog2_Exp - UserData.instance.expTable[UserData.instance.Dog2_Level - 1] / UserData.instance.expTable[UserData.instance.Dog2_Level]);

            dog2_HungryBar.value = (UserData.instance.Dog2_Hungry + UserData.instance.Dog2_Tirsty) / 200f;   // changeing
            dog2_HealthBar.value = UserData.instance.Dog2_Health / 100f;  // changeing
            dog2_HappyBar.value = UserData.instance.Dog2_Happy / 100f;  // changeing


            DateTime dog2_BirthDate = DateTime.Parse(UserData.instance.Dog2_Age);
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

            dog2_Age.text = dog2_AgeYear + "살 " + dog2_AgeMonth + "개월";
            PlayerDog2_age.text = dog1_AgeYear + "살 " + dog1_AgeMonth + "개월";


            if (UserData.instance.Dog2_Sex == "M")
            {
                PlayerDog2_Gender.text = "남아";
                if (UserData.instance.Dog2_Neuter == "T")
                {
                    dog2_Sex.text = "♂ / O";
                    card_Dog2gender.text = "남아 / 중성화 O";
                    PlayerDog2_Neuter.text = "중성화 O";
                }
                else
                {
                    dog2_Sex.text = "♂ / X";
                    card_Dog2gender.text = "남아 / 중성화 X";
                    PlayerDog2_Neuter.text = "중성화 X";
                }

            }
            else
            {
                PlayerDog2_Gender.text = "여아";
                if (UserData.instance.Dog2_Neuter == "T")
                {
                    dog2_Sex.text = "♀ / O";
                    card_Dog2gender.text = "여아 / 중성화 O";
                    PlayerDog2_Neuter.text = "중성화 O";
                }
                else
                {
                    dog2_Sex.text = "♀ / X";
                    card_Dog2gender.text = "여아 / 중성화 X";
                    PlayerDog2_Neuter.text = "중성화 X";
                }
            }

            dog2_Weight.text = UserData.instance.Dog2_Weight + "kg";

            PlayerDog2_Name.text = UserData.instance.Dog2_Name;
            PlayerDog2_Level.text = "Level " + UserData.instance.Dog2_Level;
            PlayerDog2_Style.text = UserData.instance.Dog2_Style;


            dog2Card.transform.SetParent(cardList.transform);
            dog2Toggle.transform.SetParent(toggleList.transform);
            dog2Card.SetActive(true);
            dog2Toggle.SetActive(true);
            dog3Card.SetActive(false);
            dog3Toggle.SetActive(false);

            card_Dog2name.text = UserData.instance.Dog2_Name;
            card_Dog2id.text = UserData.instance.Dog2_PevoNumber;
            card_Dog2style.text = UserData.instance.Dog2_Style;
            card_Dog2birthday.text = dog1_BirthYear + "년 " + dog2_BirthMonth + "월 " + dog2_BirthDay + "일" + "\n" + "(" + dog2_AgeYear + "살 " + dog2_AgeMonth + "개월" + ")";

            if (UserData.instance.Dog2_PetRgtNo != null || UserData.instance.Dog2_PetRgtNo != "")
            {
                card_Dog2petid.text = UserData.instance.Dog2_PetRgtNo;
                Dog2_PetRgtBtn.SetActive(false);
            }
            else
            {
                Dog2_PetRgtBtn.SetActive(true);
            }



            DateTime dog2_AdoptedDate = DateTime.Parse(UserData.instance.Dog2_Adopted);
            int dog2_AdoptedYear = dog2_AdoptedDate.Year;
            int dog2_AdoptedMonth = dog2_AdoptedDate.Month;
            int dog2_AdoptedDay = dog2_AdoptedDate.Day;

            card_Dog2adopted.text = dog2_AdoptedYear + "년 " + dog2_AdoptedMonth + "월 " + dog2_AdoptedDay + "일";
            card_Dog2weight.text = UserData.instance.Dog2_Weight + "kg";

        }

        if (UserData.instance.User_DogCount > 2)
        {

            ShitSlash1.SetActive(true);
            PlayerDogShotWin2.SetActive(true);
            ShitSlash2.SetActive(true);
            PlayerDogShotWin3.SetActive(true);
            DogSlot02.SetActive(true);
            DogSlot03.SetActive(true);

            TitleDog3_LevelText.text = UserData.instance.Dog3_Level.ToString();
            TitleDog3_Exp.fillAmount = (UserData.instance.Dog3_Exp - UserData.instance.expTable[UserData.instance.Dog3_Level - 1] / UserData.instance.expTable[UserData.instance.Dog3_Level]);


            UserData.instance.Dog3_Health = UserData.instance.Dog3_Health - (((diffDay * 24) + diffHour) * 0.3f);  // ?????? 0.3?? ????
            UserData.instance.Dog3_Hungry = UserData.instance.Dog3_Hungry - ((((diffDay * 24) + diffHour) * 6) + diffMinute / 6) * 0.2f; // 10???? 0.2 ???? (?????? 1.2?? ????)
            UserData.instance.Dog3_Tirsty = UserData.instance.Dog3_Tirsty - ((((diffDay * 24) + diffHour) * 6) + diffMinute / 6) * 0.3f; // 10???? 0.3???? (?????? 1.8?? ????)
            UserData.instance.Dog3_Clean = UserData.instance.Dog3_Clean - ((((diffDay * 24) + diffHour) * 6) + diffMinute / 6) * 0.1f; // 10???? 0.1 ???? (?????? 0.6????) 
            UserData.instance.Dog3_Happy = UserData.instance.Dog3_Happy - ((diffDay * 24) + diffHour) * 2;  // ?????? 2 ????

            UserData.instance.Dog3_Health = UserData.instance.Dog3_Health < 0 ? 0 : UserData.instance.Dog3_Health;
            UserData.instance.Dog3_Hungry = UserData.instance.Dog3_Hungry < 0 ? 0 : UserData.instance.Dog3_Hungry;
            UserData.instance.Dog3_Tirsty = UserData.instance.Dog3_Tirsty < 0 ? 0 : UserData.instance.Dog3_Tirsty;
            UserData.instance.Dog3_Clean = UserData.instance.Dog3_Clean < 0 ? 0 : UserData.instance.Dog3_Clean;
            UserData.instance.Dog3_Happy = UserData.instance.Dog3_Happy < 0 ? 0 : UserData.instance.Dog3_Happy;


            dog3_Name.text = UserData.instance.Dog3_Name;
            dog3_Style.text = UserData.instance.Dog3_Style;
            dog3_Level.text = "LEVEL " + UserData.instance.Dog3_Level.ToString();
            dog3_Local.text = UserData.instance.User_Local;
            dog3_ExpBar.value = (UserData.instance.Dog3_Exp - UserData.instance.expTable[UserData.instance.Dog3_Level - 1] / UserData.instance.expTable[UserData.instance.Dog3_Level]);

            dog3_HungryBar.value = (UserData.instance.Dog3_Hungry + UserData.instance.Dog3_Tirsty) / 200f;   // changeing
            dog3_HealthBar.value = UserData.instance.Dog3_Health / 100f;  // changeing
            dog3_HappyBar.value = UserData.instance.Dog3_Happy / 100f;  // changeing


            DateTime dog3_BirthDate = DateTime.Parse(UserData.instance.Dog3_Age);
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

            dog3_Age.text = dog3_AgeYear + "살 " + dog3_AgeMonth + "개월";
            PlayerDog3_age.text = dog1_AgeYear + "살 " + dog1_AgeMonth + "개월";

            if (UserData.instance.Dog3_Sex == "M")
            {
                PlayerDog3_Gender.text = "남아";
                if (UserData.instance.Dog3_Neuter == "T")
                {
                    dog3_Sex.text = "♂ / O";
                    card_Dog3gender.text = "남아 / 중성화 O";
                    PlayerDog3_Neuter.text = "중성화 O";
                }
                else
                {
                    dog3_Sex.text = "♂ / X";
                    card_Dog3gender.text = "남아 / 중성화 X";
                    PlayerDog3_Neuter.text = "중성화 X";
                }

            }
            else
            {
                PlayerDog3_Gender.text = "여아";
                if (UserData.instance.Dog3_Neuter == "T")
                {
                    dog3_Sex.text = "♀ / O";
                    card_Dog3gender.text = "여아 / 중성화 O";
                    PlayerDog3_Neuter.text = "중성화 O";
                }
                else
                {
                    dog3_Sex.text = "♀ / X";
                    card_Dog3gender.text = "여아 / 중성화 X";
                    PlayerDog3_Neuter.text = "중성화 X";
                }
            }

            dog3_Weight.text = UserData.instance.Dog3_Weight + "kg";

            PlayerDog3_Name.text = UserData.instance.Dog3_Name;
            PlayerDog3_Level.text = "Level " + UserData.instance.Dog3_Level;
            PlayerDog3_Style.text = UserData.instance.Dog3_Style;

            dog3Card.transform.SetParent(cardList.transform);
            dog3Toggle.transform.SetParent(toggleList.transform);
            dog2Card.SetActive(true);
            dog2Toggle.SetActive(true);
            dog3Card.SetActive(true);
            dog3Toggle.SetActive(true);

            card_Dog3name.text = UserData.instance.Dog3_Name;
            card_Dog3id.text = UserData.instance.Dog3_PevoNumber;
            card_Dog3style.text = UserData.instance.Dog3_Style;
            card_Dog3birthday.text = dog3_BirthYear + "년 " + dog3_BirthMonth + "월 " + dog3_BirthDay + "일" + "\n" + "(" + dog3_AgeYear + "살 " + dog3_AgeMonth + "개월" + ")";

            if (UserData.instance.Dog3_PetRgtNo != null || UserData.instance.Dog3_PetRgtNo != "")
            {
                card_Dog3petid.text = UserData.instance.Dog3_PetRgtNo;
                Dog3_PetRgtBtn.SetActive(false);
            }
            else
            {
                Dog3_PetRgtBtn.SetActive(true);
            }



            DateTime dog3_AdoptedDate = DateTime.Parse(UserData.instance.Dog3_Adopted);
            int dog3_AdoptedYear = dog3_AdoptedDate.Year;
            int dog3_AdoptedMonth = dog3_AdoptedDate.Month;
            int dog3_AdoptedDay = dog3_AdoptedDate.Day;

            card_Dog3adopted.text = dog3_AdoptedYear + "년 " + dog3_AdoptedMonth + "월 " + dog3_AdoptedDay + "일";
            card_Dog3weight.text = UserData.instance.Dog3_Weight + "kg";
        }

        OnlineCheck();
        SetDogData0();

        



        // Connect Manager;
        friendManager.MyDisplayName = UserData.instance.User_Name;  
        friendManager.MyID = UserData.instance.User_MyID;   
        configManager.MyName.text = UserData.instance.User_Name; 
        configManager.myID.text = UserData.instance.User_MyID;

        //view InboxList;
        mailBoxManager.UpdateInboxListToClient(UserData.instance.User_Inbox.ToString()); 

    }

    public void SetDogData0()
    {
        var reqeust = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                {"Dog1_Health", UserData.instance.Dog1_Health.ToString() }, {"Dog1_Hungry", UserData.instance.Dog1_Hungry.ToString()}, {"Dog1_Tirsty", UserData.instance.Dog1_Tirsty.ToString()}, {"Dog1_Clean",  UserData.instance.Dog1_Clean.ToString()}, {"Dog1_Happy",  UserData.instance.Dog1_Happy.ToString()},
                {"Dog2_Health", UserData.instance.Dog2_Health.ToString() }, {"Dog2_Hungry", UserData.instance.Dog2_Hungry.ToString()}, {"Dog2_Tirsty", UserData.instance.Dog2_Tirsty.ToString()}, {"Dog2_Clean",  UserData.instance.Dog2_Clean.ToString()}, {"Dog2_Happy",  UserData.instance.Dog2_Happy.ToString()}
               
            },
            Permission = UserDataPermission.Public

        };
        PlayFabClientAPI.UpdateUserData(reqeust, (result) => { print("User ?????? ???? ????"); if (UserData.instance.User_DogCount > 2) { SetDogData1(); } }, (error) => { });

    }
    public void SetDogData1()
    {
        var reqeust = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                {"Dog3_Health", UserData.instance.Dog3_Health.ToString() }, {"Dog3_Hungry", UserData.instance.Dog3_Hungry.ToString()}, {"Dog3_Tirsty", UserData.instance.Dog3_Tirsty.ToString()}, {"Dog3_Clean",  UserData.instance.Dog3_Clean.ToString()}, {"Dog3_Happy",  UserData.instance.Dog3_Happy.ToString()}
            },
            Permission = UserDataPermission.Public

        };
        PlayFabClientAPI.UpdateUserData(reqeust, (result) => { print("User ?????? ???? ????"); }, (error) => { });

    }
    public void OnlineCheck()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "OnlineRequest",
            FunctionParameter = new { onlineState = "On" },
            GeneratePlayStreamEvent = true,
        }, (result) => { print("?????? ???? ????"); }, (error) => print("?????? ???? ????"));
    }


    public void EditDog1Photo()
    {
        PhotoManager.instance.Dog1PhotoSelect();
    }

    public void EditDog2Photo()
    {
        PhotoManager.instance.Dog2PhotoSelect();
    }
    public void EditDog3Photo()
    {
        PhotoManager.instance.Dog3PhotoSelect();
    }
    public void EditPlayerPhoto()
    {
        PhotoManager.instance.PlayerPhotoSelect();
    }

    public void DetailViewOnPevoApp()
    {
        //test
        string AppUrl;
#if UNITY_EDITOR
        AppUrl = "https://play.google.com/store/apps/details?id=com.ddcares.pevo";
#elif UNITY_IPHONE
        AppUrl = "https://apps.apple.com/kr/app/%ED%8E%98%EB%B3%B4/id1502216647";
#elif UNITY_ANDROID
        AppUrl = "https://play.google.com/store/apps/details?id=com.ddcares.pevo";
#endif
        Application.OpenURL(AppUrl);
    }
}
