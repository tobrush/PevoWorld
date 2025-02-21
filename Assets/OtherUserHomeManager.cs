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


public class OtherUserHomeManager : MonoBehaviour
{
    public PevoDataManager1 PDM1;

    public FriendsManager FM;
    public OtherUserData OUD;
    public string OtherUserPlayfabID;
    public ClickManager CM;
    //
    public Image dog4_icon, dog5_icon, dog6_icon, dog4_image, dog5_image, dog6_image, otherPlayer_image, player_dog4, player_dog5, player_dog6, petCard4, petCard5, petCard6, SignAdd;

    public TMP_Text TitleUserNameText;
    public TMP_Text PlayerWinBtnText4, PlayerWinBtnText5, PlayerWinBtnText6;
    public TMP_Text TitleDog4_LevelText;
    public Image TitleDog4_Exp;
    public TMP_Text TitleDog5_LevelText;
    public Image TitleDog5_Exp;
    public TMP_Text TitleDog6_LevelText;
    public Image TitleDog6_Exp;

    public TMP_Text FriendsCount;
    public TMP_Text LikeCount;

    public TMP_Text PlayerName, PlayerID, PlayerLocal, PlayerIntro;
    public TMP_Text PlayerDog4_Name, PlayerDog4_Level, PlayerDog4_Style, PlayerDog4_age, PlayerDog4_Gender, PlayerDog4_Neuter;
    public TMP_Text PlayerDog5_Name, PlayerDog5_Level, PlayerDog5_Style, PlayerDog5_age, PlayerDog5_Gender, PlayerDog5_Neuter;
    public TMP_Text PlayerDog6_Name, PlayerDog6_Level, PlayerDog6_Style, PlayerDog6_age, PlayerDog6_Gender, PlayerDog6_Neuter;

    

    public TMP_Text dog4_Name, dog5_Name, dog6_Name;
    public TMP_Text dog4_Style, dog5_Style, dog6_Style;
    public TMP_Text dog4_Level, dog5_Level, dog6_Level;
    public Slider dog4_ExpBar, dog5_ExpBar, dog6_ExpBar;
    public TMP_Text dog4_Local, dog5_Local, dog6_Local;
    public TMP_Text dog4_Age, dog5_Age, dog6_Age;
    public TMP_Text dog4_Sex, dog5_Sex, dog6_Sex;
    public TMP_Text dog4_Weight, dog5_Weight, dog3_Weight;

    public GameObject PlayerDogShotWin4, ShitSlash4, PlayerDogShotWin5, ShitSlash5, PlayerDogShotWin6;
    public GameObject DogSlot04, DogSlot05, DogSlot06;


    public Slider dog4_HungryBar, dog5_HungryBar, dog6_HungryBar;
    public Slider dog4_HealthBar, dog5_HealthBar, dog6_HealthBar;
    public Slider dog4_HappyBar, dog5_HappyBar, dog6_HappyBar;

    public static int stat_Min = 0;
    public static int stat_Max = 99;
    private void Start()
    {
        OUD = GameObject.Find("OtherUserData").GetComponent<OtherUserData>();


        GetOtherUserData();
    }

    public void Update()
    {
        //dog4_HungryBar.value = (UserData.instance.Dog1_Hungry + UserData.instance.Dog1_Tirsty) / 200f;   // changeing
        //dog4_HealthBar.value = UserData.instance.Dog1_Health / 100f;  // changeing
        //dog4_HappyBar.value = UserData.instance.Dog1_Happy / 100f;  // changeing
    }

    public void GetOtherUserData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = OUD.OhterUser_ID

        }, result =>
        {
            print(OUD.OhterUser_ID);
            OUD.OhterUser_Name = result.Data["User_Name"].Value;
            OUD.OhterUser_Local = result.Data["User_Local"].Value;
            OUD.OhterUser_Hello = result.Data["User_Hello"].Value;
            OUD.OhterUser_LikeCount = int.Parse(result.Data["User_LikeCount"].Value);
            OUD.OhterUser_FriendsCount = int.Parse(result.Data["User_FriendsCount"].Value);
            OUD.OhterUser_DogCount = int.Parse(result.Data["User_DogCount"].Value);
            OUD.OhterUser_FriendsCount = int.Parse(result.Data["User_FriendsCount"].Value);
            OUD.OhterUser_DogCount = int.Parse(result.Data["User_DogCount"].Value);
            OUD.OhterUser_SaveTime = result.Data["SaveTime"].Value;

            OUD.OhterDog1_PevoNumber = result.Data["Dog1_PevoNumber"].Value;
            OUD.OhterDog1_PetRgtNo = result.Data["Dog1_PetRgtNo"].Value;
            OUD.OhterDog1_Character = result.Data["Dog1_Character"].Value;
            OUD.OhterDog1_Name = result.Data["Dog1_Name"].Value;
            OUD.OhterDog1_Style = result.Data["Dog1_Style"].Value;
            OUD.OhterDog1_Age = result.Data["Dog1_Age"].Value;
            OUD.OhterDog1_Adopted = result.Data["Dog1_Adopted"].Value;
            OUD.OhterDog1_Weight = result.Data["Dog1_Weight"].Value;

            OUD.OhterDog1_Sex = result.Data["Dog1_Sex"].Value;
            OUD.OhterDog1_Neuter = result.Data["Dog1_Neuter"].Value;
            OUD.OhterDog1_Food = result.Data["Dog1_Food"].Value;
            OUD.OhterDog1_Place = result.Data["Dog1_Place"].Value;

            OUD.OhterDog1_Health = float.Parse(result.Data["Dog1_Health"].Value);
            OUD.OhterDog1_Hungry = float.Parse(result.Data["Dog1_Hungry"].Value);
            OUD.OhterDog1_Tirsty = float.Parse(result.Data["Dog1_Tirsty"].Value);
            OUD.OhterDog1_Clean = float.Parse(result.Data["Dog1_Clean"].Value);
            OUD.OhterDog1_Happy = float.Parse(result.Data["Dog1_Happy"].Value);
            OUD.OhterDog1_Level = int.Parse(result.Data["Dog1_Level"].Value);
            OUD.OhterDog1_Exp = int.Parse(result.Data["Dog1_Exp"].Value);
            OUD.OhterDog1_Speed = int.Parse(result.Data["Dog1_Speed"].Value);
            OUD.OhterDog1_Power = int.Parse(result.Data["Dog1_Power"].Value);
            OUD.OhterDog1_Stamina = int.Parse(result.Data["Dog1_Stamina"].Value);
            OUD.OhterDog1_Sense = int.Parse(result.Data["Dog1_Sense"].Value);
            OUD.OhterDog1_Guts = int.Parse(result.Data["Dog1_Guts"].Value);
            OUD.OhterDog1_Lux = int.Parse(result.Data["Dog1_Lux"].Value);


            OUD.OhterDog1_item_Head = result.Data["Dog1_item_Head"].Value;
            OUD.OhterDog1_item_Eye = result.Data["Dog1_item_Eye"].Value;
            OUD.OhterDog1_item_Neck = result.Data["Dog1_item_Neck"].Value;
            OUD.OhterDog1_item_Body = result.Data["Dog1_item_Body"].Value;
            //2

            OUD.OhterDog2_PevoNumber = result.Data["Dog2_PevoNumber"].Value;
            OUD.OhterDog2_PetRgtNo = result.Data["Dog2_PetRgtNo"].Value;
            OUD.OhterDog2_Character = result.Data["Dog2_Character"].Value;
            OUD.OhterDog2_Name = result.Data["Dog2_Name"].Value;
            OUD.OhterDog2_Style = result.Data["Dog2_Style"].Value;
            OUD.OhterDog2_Age = result.Data["Dog2_Age"].Value;
            OUD.OhterDog2_Adopted = result.Data["Dog2_Adopted"].Value;
            OUD.OhterDog2_Weight = result.Data["Dog2_Weight"].Value;
            OUD.OhterDog2_Sex = result.Data["Dog2_Sex"].Value;
            OUD.OhterDog2_Neuter = result.Data["Dog2_Neuter"].Value;
            OUD.OhterDog2_Food = result.Data["Dog2_Food"].Value;
            OUD.OhterDog2_Place = result.Data["Dog2_Place"].Value;

            OUD.OhterDog2_Health = float.Parse(result.Data["Dog2_Health"].Value);
            OUD.OhterDog2_Hungry = float.Parse(result.Data["Dog2_Hungry"].Value);
            OUD.OhterDog2_Tirsty = float.Parse(result.Data["Dog2_Tirsty"].Value);
            OUD.OhterDog2_Clean = float.Parse(result.Data["Dog2_Clean"].Value);
            OUD.OhterDog2_Happy = float.Parse(result.Data["Dog2_Happy"].Value);
            OUD.OhterDog2_Level = int.Parse(result.Data["Dog2_Level"].Value);
            OUD.OhterDog2_Exp = int.Parse(result.Data["Dog2_Exp"].Value);
            OUD.OhterDog2_Speed = int.Parse(result.Data["Dog2_Speed"].Value);
            OUD.OhterDog2_Power = int.Parse(result.Data["Dog2_Power"].Value);
            OUD.OhterDog2_Stamina = int.Parse(result.Data["Dog2_Stamina"].Value);
            OUD.OhterDog2_Sense = int.Parse(result.Data["Dog2_Sense"].Value);
            OUD.OhterDog2_Guts = int.Parse(result.Data["Dog2_Guts"].Value);
            OUD.OhterDog2_Lux = int.Parse(result.Data["Dog2_Lux"].Value);


            OUD.OhterDog2_item_Head = result.Data["Dog2_item_Head"].Value;
            OUD.OhterDog2_item_Eye = result.Data["Dog2_item_Eye"].Value;
            OUD.OhterDog2_item_Neck = result.Data["Dog2_item_Neck"].Value;
            OUD.OhterDog2_item_Body = result.Data["Dog2_item_Body"].Value;
            //3

            OUD.OhterDog3_PevoNumber = result.Data["Dog3_PevoNumber"].Value;
            OUD.OhterDog3_PetRgtNo = result.Data["Dog3_PetRgtNo"].Value;
            OUD.OhterDog3_Character = result.Data["Dog3_Character"].Value;
            OUD.OhterDog3_Name = result.Data["Dog3_Name"].Value;
            OUD.OhterDog3_Style = result.Data["Dog3_Style"].Value;
            OUD.OhterDog3_Age = result.Data["Dog3_Age"].Value;
            OUD.OhterDog3_Adopted = result.Data["Dog3_Adopted"].Value;
            OUD.OhterDog3_Weight = result.Data["Dog3_Weight"].Value;

            OUD.OhterDog3_Sex = result.Data["Dog3_Sex"].Value;
            OUD.OhterDog3_Neuter = result.Data["Dog3_Neuter"].Value;
            OUD.OhterDog3_Food = result.Data["Dog3_Food"].Value;
            OUD.OhterDog3_Place = result.Data["Dog3_Place"].Value;

            OUD.OhterDog3_Health = float.Parse(result.Data["Dog3_Health"].Value);
            OUD.OhterDog3_Hungry = float.Parse(result.Data["Dog3_Hungry"].Value);
            OUD.OhterDog3_Tirsty = float.Parse(result.Data["Dog3_Tirsty"].Value);
            OUD.OhterDog3_Clean = float.Parse(result.Data["Dog3_Clean"].Value);
            OUD.OhterDog3_Happy = float.Parse(result.Data["Dog3_Happy"].Value);
            OUD.OhterDog3_Level = int.Parse(result.Data["Dog3_Level"].Value);
            OUD.OhterDog3_Exp = int.Parse(result.Data["Dog3_Exp"].Value);
            OUD.OhterDog3_Speed = int.Parse(result.Data["Dog3_Speed"].Value);
            OUD.OhterDog3_Power = int.Parse(result.Data["Dog3_Power"].Value);
            OUD.OhterDog3_Stamina = int.Parse(result.Data["Dog3_Stamina"].Value);
            OUD.OhterDog3_Sense = int.Parse(result.Data["Dog3_Sense"].Value);
            OUD.OhterDog3_Guts = int.Parse(result.Data["Dog3_Guts"].Value);
            OUD.OhterDog3_Lux = int.Parse(result.Data["Dog3_Lux"].Value);

            OUD.OhterDog3_item_Head = result.Data["Dog3_item_Head"].Value;
            OUD.OhterDog3_item_Eye = result.Data["Dog3_item_Eye"].Value;
            OUD.OhterDog3_item_Neck = result.Data["Dog3_item_Neck"].Value;
            OUD.OhterDog3_item_Body = result.Data["Dog3_item_Body"].Value;

            SetUp();
        }, error =>
        {
            Debug.Log(error.GenerateErrorReport());
        });
    }

    public void SetUp()
    {
        PDM1.StartScene();
        dog4_icon.sprite = OUD.OhterDog1Photo.sprite;
        dog4_image.sprite = OUD.OhterDog1Photo.sprite;
        player_dog4.sprite = OUD.OhterDog1Photo.sprite;
        
        if (OUD.OhterUser_DogCount > 1)
        {
            dog5_icon.sprite = OUD.OhterDog2Photo.sprite;
            dog5_image.sprite = OUD.OhterDog2Photo.sprite;
            player_dog5.sprite = OUD.OhterDog2Photo.sprite;

            if (OUD.OhterUser_DogCount > 2)
            {
                dog6_icon.sprite = OUD.OhterDog3Photo.sprite;
                dog6_image.sprite = OUD.OhterDog3Photo.sprite;
                player_dog6.sprite = OUD.OhterDog3Photo.sprite;
            }
        }

        FM.OtherUserName = OUD.OhterUser_Name;
        FM.YourID = OUD.OhterUser_ID;
        otherPlayer_image.sprite = OUD.OhterPlayerPhoto.sprite;

        DateTime lastLoginTime = DateTimeOffset.Parse(OUD.OhterUser_SaveTime).UtcDateTime;
        DateTime nowTime = DateTime.Now;

        TimeSpan dateDiff = nowTime - lastLoginTime;

        int diffDay = dateDiff.Days;
        int diffHour = dateDiff.Hours;
        int diffMinute = dateDiff.Minutes;
        int diffSecond = dateDiff.Seconds;

        string day = diffDay + "일";
        string hour = diffHour + "시간";
        string min = diffMinute + "분";

        Debug.Log(day + " " + hour + " " + min + " 전");

        OUD.OhterDog1_Health = OUD.OhterDog1_Health - (((diffDay * 24) + diffHour) * 0.3f);  // 시간당 0.3씩 감소
        OUD.OhterDog1_Hungry = OUD.OhterDog1_Hungry - ((((diffDay * 24) + diffHour) * 6) + diffMinute / 6) * 0.2f; // 10분당 0.2 감소 (시간당 1.2씩 감소)
        OUD.OhterDog1_Tirsty = OUD.OhterDog1_Tirsty - ((((diffDay * 24) + diffHour) * 6) + diffMinute / 6) * 0.3f; // 10분당 0.3감소 (시간당 1.8씩 감소)
        OUD.OhterDog1_Clean = OUD.OhterDog1_Clean - ((((diffDay * 24) + diffHour) * 6) + diffMinute / 6) * 0.1f; // 10분당 0.1 감소 (시간당 0.6감소) 
        OUD.OhterDog1_Happy = OUD.OhterDog1_Happy - ((diffDay * 24) + diffHour) * 2;  // 시간당 2 감소

        OUD.OhterDog1_Health = OUD.OhterDog1_Health < 0 ? 0 : OUD.OhterDog1_Health;
        OUD.OhterDog1_Hungry = OUD.OhterDog1_Hungry < 0 ? 0 : OUD.OhterDog1_Hungry;
        OUD.OhterDog1_Tirsty = OUD.OhterDog1_Tirsty < 0 ? 0 : OUD.OhterDog1_Tirsty;
        OUD.OhterDog1_Clean = OUD.OhterDog1_Clean < 0 ? 0 : OUD.OhterDog1_Clean;
        OUD.OhterDog1_Happy = OUD.OhterDog1_Happy < 0 ? 0 : OUD.OhterDog1_Happy;

        // Main Display UI 
        TitleUserNameText.text = OUD.OhterUser_Name;
        //Debug.Log("nickName crack");

        PlayerWinBtnText4.text = OUD.OhterUser_Name + "님의 정보";
        PlayerWinBtnText5.text = OUD.OhterUser_Name + "님의 정보";
        PlayerWinBtnText6.text = OUD.OhterUser_Name + "님의 정보";

        FriendsCount.text = OUD.OhterUser_FriendsCount.ToString();
        LikeCount.text = OUD.OhterUser_LikeCount.ToString();

        TitleDog4_LevelText.text = OUD.OhterDog1_Level.ToString();
        TitleDog4_Exp.fillAmount = (OUD.OhterDog1_Exp - UserData.instance.expTable[OUD.OhterDog1_Level - 1] / UserData.instance.expTable[OUD.OhterDog1_Level]);

        PlayerName.text = OUD.OhterUser_Name;
        PlayerID.text = OUD.OhterUser_ID;
        PlayerLocal.text = OUD.OhterUser_Local;
        PlayerIntro.text = OUD.OhterUser_Hello;

        // my Payge UI

        dog4_Name.text = OUD.OhterDog1_Name;
        dog4_Style.text = OUD.OhterDog1_Style;
        dog4_Level.text = "LEVEL " + OUD.OhterDog1_Level.ToString();
        dog4_Local.text = OUD.OhterUser_Local;
        dog4_ExpBar.value = (OUD.OhterDog1_Exp - UserData.instance.expTable[OUD.OhterDog1_Level - 1] / UserData.instance.expTable[OUD.OhterDog1_Level]);

        dog4_HungryBar.value = (OUD.OhterDog1_Hungry + OUD.OhterDog1_Tirsty) / 200f;   // changeing
        dog4_HealthBar.value = OUD.OhterDog1_Health / 100f;  // changeing
        dog4_HappyBar.value = OUD.OhterDog1_Happy / 100f;  // changeing


        DateTime dog1_BirthDate = DateTime.Parse(OUD.OhterDog1_Age);
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

        dog4_Age.text = dog1_AgeYear + "살 " + dog1_AgeMonth + "개월";
        PlayerDog4_age.text = dog1_AgeYear + "살 " + dog1_AgeMonth + "개월";

        if (OUD.OhterDog1_Sex == "M")
        {
            PlayerDog4_Gender.text = "남아";

            if (OUD.OhterDog1_Neuter == "T")
            {
                dog4_Sex.text = "♂ / O";
                PlayerDog4_Neuter.text = "중성화 O";
            }
            else
            {
                dog4_Sex.text = "♂ / X";
                PlayerDog4_Neuter.text = "중성화 X";
            }
        }
        else
        {
            PlayerDog4_Gender.text = "여아";

            if (OUD.OhterDog1_Neuter == "T")
            {
                dog4_Sex.text = "♀ / O";
                PlayerDog4_Neuter.text = "중성화 O";

            }
            else
            {
                dog4_Sex.text = "♀ / X";
                PlayerDog4_Neuter.text = "중성화 X";
            }
        }

        dog4_Weight.text = OUD.OhterDog1_Weight + "kg";

        // Get Photo File
        //PhotoManager.instance.getData = null  //////// TODO
        //PhotoManager.instance.LoadAllFiles();     //////// TODO

        //Get Pevo DogData
        //Get MyFood, MyPlace / Json DogData

        //Player Page
        PlayerDog4_Name.text = OUD.OhterDog1_Name;
        PlayerDog4_Level.text = "Level " + OUD.OhterDog1_Level;
        PlayerDog4_Style.text = OUD.OhterDog1_Style;

        if (OUD.OhterUser_DogCount == 1)
        {
            ShitSlash4.SetActive(false);
            PlayerDogShotWin5.SetActive(false);
            ShitSlash5.SetActive(false);
            PlayerDogShotWin6.SetActive(false);
            DogSlot05.SetActive(false);
            DogSlot06.SetActive(false);

            //UserData.instance.Dog1_Food
            //UserData.instance.Dog1_Place 
        }


        if (OUD.OhterUser_DogCount > 1)
        {
            ShitSlash4.SetActive(true);
            PlayerDogShotWin5.SetActive(true);
            ShitSlash5.SetActive(false);
            PlayerDogShotWin6.SetActive(false);
            DogSlot05.SetActive(true);
            DogSlot06.SetActive(false);

            TitleDog5_LevelText.text = OUD.OhterDog2_Level.ToString();
            TitleDog5_Exp.fillAmount = (OUD.OhterDog2_Exp - UserData.instance.expTable[OUD.OhterDog2_Level - 1] / UserData.instance.expTable[OUD.OhterDog2_Level]);

            OUD.OhterDog2_Health = OUD.OhterDog2_Health - (((diffDay * 24) + diffHour) * 0.3f);  // 시간당 0.3씩 감소
            OUD.OhterDog2_Hungry = OUD.OhterDog2_Hungry - ((((diffDay * 24) + diffHour) * 6) + diffMinute / 6) * 0.2f; // 10분당 0.2 감소 (시간당 1.2씩 감소)
            OUD.OhterDog2_Tirsty = OUD.OhterDog2_Tirsty - ((((diffDay * 24) + diffHour) * 6) + diffMinute / 6) * 0.3f; // 10분당 0.3감소 (시간당 1.8씩 감소)
            OUD.OhterDog2_Clean = OUD.OhterDog2_Clean - ((((diffDay * 24) + diffHour) * 6) + diffMinute / 6) * 0.1f; // 10분당 0.1 감소 (시간당 0.6감소) 
            OUD.OhterDog2_Happy = OUD.OhterDog2_Happy - ((diffDay * 24) + diffHour) * 2;  // 시간당 2 감소

            OUD.OhterDog2_Health = OUD.OhterDog2_Health < 0 ? 0 : OUD.OhterDog2_Health;
            OUD.OhterDog2_Hungry = OUD.OhterDog2_Hungry < 0 ? 0 : OUD.OhterDog2_Hungry;
            OUD.OhterDog2_Tirsty = OUD.OhterDog2_Tirsty < 0 ? 0 : OUD.OhterDog2_Tirsty;
            OUD.OhterDog2_Clean = OUD.OhterDog2_Clean < 0 ? 0 : OUD.OhterDog2_Clean;
            OUD.OhterDog2_Happy = OUD.OhterDog2_Happy < 0 ? 0 : OUD.OhterDog2_Happy;

            dog5_Name.text = OUD.OhterDog2_Name;
            dog5_Style.text = OUD.OhterDog2_Style;
            dog5_Level.text = "LEVEL " + OUD.OhterDog2_Level.ToString();
            dog5_Local.text = OUD.OhterUser_Local;
            dog5_ExpBar.value = (OUD.OhterDog2_Exp - UserData.instance.expTable[OUD.OhterDog2_Level - 1] / UserData.instance.expTable[OUD.OhterDog2_Level]);

            dog5_HungryBar.value = (OUD.OhterDog2_Hungry + OUD.OhterDog2_Tirsty) / 200f;   // changeing
            dog5_HealthBar.value = OUD.OhterDog2_Health / 100f;  // changeing
            dog5_HappyBar.value = OUD.OhterDog2_Happy / 100f;  // changeing


            DateTime dog2_BirthDate = DateTime.Parse(OUD.OhterDog2_Age);
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

            dog5_Age.text = dog2_AgeYear + "살 " + dog2_AgeMonth + "개월";
            PlayerDog5_age.text = dog1_AgeYear + "살 " + dog1_AgeMonth + "개월";


            if (OUD.OhterDog2_Sex == "M")
            {
                PlayerDog5_Gender.text = "남아";
                if (OUD.OhterDog2_Neuter == "T")
                {
                    dog5_Sex.text = "♂ / O";
                    PlayerDog5_Neuter.text = "중성화 O";
                }
                else
                {
                    dog5_Sex.text = "♂ / X";
                    PlayerDog5_Neuter.text = "중성화 X";
                }

            }
            else
            {
                PlayerDog5_Gender.text = "여아";
                if (OUD.OhterDog2_Neuter == "T")
                {
                    dog5_Sex.text = "♀ / O";
                    PlayerDog5_Neuter.text = "중성화 O";
                }
                else
                {
                    dog5_Sex.text = "♀ / X";
                    PlayerDog5_Neuter.text = "중성화 X";
                }
            }

            dog5_Weight.text = OUD.OhterDog2_Weight + "kg";

            PlayerDog5_Name.text = OUD.OhterDog2_Name;
            PlayerDog5_Level.text = "Level " + OUD.OhterDog2_Level;
            PlayerDog5_Style.text = OUD.OhterDog2_Style;

        }

        if (OUD.OhterUser_DogCount > 2)
        {
            ShitSlash4.SetActive(true);
            PlayerDogShotWin5.SetActive(true);
            ShitSlash5.SetActive(true);
            PlayerDogShotWin6.SetActive(true);
            DogSlot05.SetActive(true);
            DogSlot06.SetActive(true);

            TitleDog6_LevelText.text = OUD.OhterDog3_Level.ToString();
            TitleDog6_Exp.fillAmount = (OUD.OhterDog3_Exp - UserData.instance.expTable[OUD.OhterDog3_Level - 1] / UserData.instance.expTable[OUD.OhterDog3_Level]);


            OUD.OhterDog3_Health = OUD.OhterDog3_Health - (((diffDay * 24) + diffHour) * 0.3f);  // 시간당 0.3씩 감소
            OUD.OhterDog3_Hungry = OUD.OhterDog3_Hungry - ((((diffDay * 24) + diffHour) * 6) + diffMinute / 6) * 0.2f; // 10분당 0.2 감소 (시간당 1.2씩 감소)
            OUD.OhterDog3_Tirsty = OUD.OhterDog3_Tirsty - ((((diffDay * 24) + diffHour) * 6) + diffMinute / 6) * 0.3f; // 10분당 0.3감소 (시간당 1.8씩 감소)
            OUD.OhterDog3_Clean = OUD.OhterDog3_Clean - ((((diffDay * 24) + diffHour) * 6) + diffMinute / 6) * 0.1f; // 10분당 0.1 감소 (시간당 0.6감소) 
            OUD.OhterDog3_Happy = OUD.OhterDog3_Happy - ((diffDay * 24) + diffHour) * 2;  // 시간당 2 감소

            OUD.OhterDog3_Health = OUD.OhterDog3_Health < 0 ? 0 : OUD.OhterDog3_Health;
            OUD.OhterDog3_Hungry = OUD.OhterDog3_Hungry < 0 ? 0 : OUD.OhterDog3_Hungry;
            OUD.OhterDog3_Tirsty = OUD.OhterDog3_Tirsty < 0 ? 0 : OUD.OhterDog3_Tirsty;
            OUD.OhterDog3_Clean = OUD.OhterDog3_Clean < 0 ? 0 : OUD.OhterDog3_Clean;
            OUD.OhterDog3_Happy = OUD.OhterDog3_Happy < 0 ? 0 : OUD.OhterDog3_Happy;


            dog6_Name.text = OUD.OhterDog3_Name;
            dog6_Style.text = OUD.OhterDog3_Style;
            dog6_Level.text = "LEVEL " + OUD.OhterDog3_Level.ToString();
            dog6_Local.text = OUD.OhterUser_Local;
            dog6_ExpBar.value = (OUD.OhterDog3_Exp - UserData.instance.expTable[OUD.OhterDog3_Level - 1] / UserData.instance.expTable[OUD.OhterDog3_Level]);

            dog6_HungryBar.value = (OUD.OhterDog3_Hungry + OUD.OhterDog3_Tirsty) / 200f;   // changeing
            dog6_HealthBar.value = OUD.OhterDog3_Health / 100f;  // changeing
            dog6_HappyBar.value = OUD.OhterDog3_Happy / 100f;  // changeing


            DateTime dog3_BirthDate = DateTime.Parse(OUD.OhterDog3_Age);
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

            dog6_Age.text = dog3_AgeYear + "살 " + dog3_AgeMonth + "개월";
            PlayerDog6_age.text = dog1_AgeYear + "살 " + dog1_AgeMonth + "개월";

            if (OUD.OhterDog3_Sex == "M")
            {
                PlayerDog6_Gender.text = "남아";
                if (OUD.OhterDog3_Neuter == "T")
                {
                    dog6_Sex.text = "♂ / O";
                    PlayerDog6_Neuter.text = "중성화 O";
                }
                else
                {
                    dog6_Sex.text = "♂ / X";
                    PlayerDog6_Neuter.text = "중성화 X";
                }

            }
            else
            {
                PlayerDog6_Gender.text = "여아";
                if (OUD.OhterDog3_Neuter == "T")
                {
                    dog6_Sex.text = "♀ / O";
                    PlayerDog6_Neuter.text = "중성화 O";
                }
                else
                {
                    dog6_Sex.text = "♀ / X";
                    PlayerDog6_Neuter.text = "중성화 X";
                }
            }

            dog3_Weight.text = OUD.OhterDog3_Weight + "kg";

            PlayerDog6_Name.text = OUD.OhterDog3_Name;
            PlayerDog6_Level.text = "Level " + OUD.OhterDog3_Level;
            PlayerDog6_Style.text = OUD.OhterDog3_Style;
        }

  
        //SetDogData0();



    }

    /*
    public void SetDogData0()
    {
        var reqeust = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                {"Dog1_Health",  OUD.OhterDog1_Health.ToString() }, {"Dog1_Hungry",  OUD.OhterDog1_Hungry.ToString()}, {"Dog1_Tirsty",  OUD.OhterDog1_Tirsty.ToString()}, {"Dog1_Clean",   OUD.OhterDog1_Clean.ToString()}, {"Dog1_Happy",   OUD.OhterDog1_Happy.ToString()},
                {"Dog2_Health",  OUD.OhterDog2_Health.ToString() }, {"Dog2_Hungry",  OUD.OhterDog2_Hungry.ToString()}, {"Dog2_Tirsty",  OUD.OhterDog2_Tirsty.ToString()}, {"Dog2_Clean",   OUD.OhterDog2_Clean.ToString()}, {"Dog2_Happy",   OUD.OhterDog2_Happy.ToString()}

            },
            Permission = UserDataPermission.Public

        };
        PlayFabClientAPI.UpdateUserData(reqeust, (result) => { print("User 데이터 저장 성공"); if (UserData.instance.User_DogCount > 2) { SetDogData1(); } }, (error) => { });

    }
    public void SetDogData1()
    {
        var reqeust = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                {"Dog3_Health",  OUD.OhterDog3_Health.ToString() }, {"Dog3_Hungry",  OUD.OhterDog3_Hungry.ToString()}, {"Dog3_Tirsty",  OUD.OhterDog3_Tirsty.ToString()}, {"Dog3_Clean",   OUD.OhterDog3_Clean.ToString()}, {"Dog3_Happy",   OUD.OhterDog3_Happy.ToString()}
            },
            Permission = UserDataPermission.Public

        };
        PlayFabClientAPI.UpdateUserData(reqeust, (result) => { print("User 데이터 저장 성공"); }, (error) => { });

    }

    */

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
