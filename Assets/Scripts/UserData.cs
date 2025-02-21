using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;

public class UserData : MonoBehaviour
{
    [HideInInspector]
    public int[] expTable = { 0, 15, 49, 106, 198, 333, 705, 1265, 2105, 3347, 4589, 5831, 7073, 8315, 9557, 11047, 12835, 14980, 17554, 20642, 24347, 28793, 34128, 40530, 48212, 57430, 68491, 81764, 97691, 116803, 135915, 155027, 174139, 193251, 212363, 235297, 262817, 295841, 335469, 383022, 434379, 489844, 549746, 614440, 684309, 759767, 841261, 929274, 1024328, 1126986, 1237856, 1357595, 1486913, 1626576, 1777412, 1940314, 2116248, 2306256, 2511464, 2733088, 2954712, 3176336, 3397960, 3619584, 3841208, 4079453, 4335566, 4610887, 4906857, 5225024, 5567053, 5934734, 6329991, 6754892, 7211660, 7700401, 8223353, 8782911, 9381638, 10022275, 10707756, 11441220, 12226026, 13065768, 13964291, 14925710, 15954428, 17055156, 18232934, 19493156, 20835292, 22264666, 23786949, 25408180, 27134791, 28973631, 30931995, 33017652, 35238876 };

    [SerializeField]
    public static UserData instance = null;

    bool bPaused = false;

    [Header("User Infor")]
    public string User_MyID; // ?? ??????
    public string User_Name; // ????????
    public string User_Local; // ????
    public string User_Hello; // ??????
    public int User_LikeCount; // ?????? ??
    public string User_LikeIDs; // ??????????
    public int User_FriendsCount; // ???? ??
    public string User_FriendsIDs; // ????????
    public string User_Inbox; // ?????? Json
    public string User_SaveTime; // ???????????? ?? ????????
    public int User_DogCount; // ?????? ??
    public string User_PevoOne;
    public string User_Local_lat;
    public string User_Local_lon;

    public string editDummyData;

    // Dog1

    [Header("Dog1 Infor")]
    public string Dog1_PevoNumber; // ????????
    public string Dog1_PetRgtNo;
    public string Dog1_Character; // ??????
    public string Dog1_Name; // ??????
    public string Dog1_Style; // ????
    public string Dog1_Age; // ??????
    public string Dog1_Adopted;
    public string Dog1_Weight; // ????????
    public string Dog1_Sex; // ????
    public string Dog1_Neuter; // ??????
    public string Dog1_Food; // ???? Json
    public string Dog1_Place; // ???? Json

    public string Dog1_basicFood_Name;
    public string Dog1_basicFood_Brand;
    public string Dog1_basicFood_cal;



    [Header("Dog1 Condition")]
    public float Dog1_Health; // ????
    public float Dog1_Hungry; // ??????
    public float Dog1_Tirsty; // ??????
    public float Dog1_Clean; // ????
    public float Dog1_Happy; // ????

    [Header("Dog1 Stats")]
    public int Dog1_Level;
    public int Dog1_Exp;
    public int Dog1_StatPoint;
    public int Dog1_Speed;
    public int Dog1_Power;
    public int Dog1_Stamina;
    public int Dog1_Sense;
    public int Dog1_Guts;
    public int Dog1_Lux;

    [Header("Dog1 Item Equip")]
    public string Dog1_item_Head;
    public string Dog1_item_Eye;
    public string Dog1_item_Neck;
    public string Dog1_item_Body;

    // Dog2

    [Header("Dog2 Infor")]
    public string Dog2_PevoNumber; // ????????
    public string Dog2_PetRgtNo;
    public string Dog2_Character; // ??????
    public string Dog2_Name; // ??????
    public string Dog2_Style; // ????
    public string Dog2_Age; // ??????
    public string Dog2_Adopted;
    public string Dog2_Weight; // ????????
    public string Dog2_Sex; // ????
    public string Dog2_Neuter; // ??????



    public string Dog2_Food; // ???? Json
    public string Dog2_Place; // ???? Json


    public string Dog2_basicFood_Name;
    public string Dog2_basicFood_Brand;
    public string Dog2_basicFood_cal;


    [Header("Dog2 Condition")]
    public float Dog2_Health; // ????
    public float Dog2_Hungry; // ??????
    public float Dog2_Tirsty; // ??????
    public float Dog2_Clean; // ????
    public float Dog2_Happy; // ????

    [Header("Dog2 Stats")]
    public int Dog2_Level;
    public int Dog2_Exp;
    public int Dog2_StatPoint;
    public int Dog2_Speed;
    public int Dog2_Power;
    public int Dog2_Stamina;
    public int Dog2_Sense;
    public int Dog2_Guts;
    public int Dog2_Lux;

    [Header("Dog2 Item Equip")]
    public string Dog2_item_Head;
    public string Dog2_item_Eye;
    public string Dog2_item_Neck;
    public string Dog2_item_Body;


    // Dog3

    [Header("Dog3 Infor")]
    public string Dog3_PevoNumber; // ????????3
    public string Dog3_PetRgtNo;
    public string Dog3_Character; // ??????
    public string Dog3_Name; // ??????
    public string Dog3_Style; // ????
    public string Dog3_Age; // ??????
    public string Dog3_Adopted;
    public string Dog3_Weight; // ????????
    public string Dog3_Sex; // ????
    public string Dog3_Neuter; // ??????
    public string Dog3_Food; // ???? Json
    public string Dog3_Place; // ???? Json

    public string Dog3_basicFood_Name;
    public string Dog3_basicFood_Brand;
    public string Dog3_basicFood_cal;


    [Header("Dog3 Condition")]
    public float Dog3_Health; // ????
    public float Dog3_Hungry; // ??????
    public float Dog3_Tirsty; // ??????
    public float Dog3_Clean; // ????
    public float Dog3_Happy; // ????

    [Header("Dog1 Stats")]
    public int Dog3_Level;
    public int Dog3_Exp;
    public int Dog3_StatPoint;
    public int Dog3_Speed;
    public int Dog3_Power;
    public int Dog3_Stamina;
    public int Dog3_Sense;
    public int Dog3_Guts;
    public int Dog3_Lux;

    [Header("Dog3 Item Equip")]
    public string Dog3_item_Head;
    public string Dog3_item_Eye;
    public string Dog3_item_Neck;
    public string Dog3_item_Body;

    [Header("UserSetting")]
    public bool NotAutoLogin = false;




    private void Awake()
    {
        if (instance == null) //instance?? null. ??, ?????????? ???????? ???? ??????
        {
            instance = this; //???????? instance?? ??????????.
            DontDestroyOnLoad(this.gameObject); //OnLoad(???? ???? ????????) ?????? ???????? ???? ????
        }
        else
        {
            if (instance != this) //instance?? ???? ???????? ???? instance?? ???? ???????? ?????? ????
                Destroy(this.gameObject); //?? ???? ???????? ?????? ???????? ???? AWake?? ?????? ????
        }
    }


    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            bPaused = true;
            //todo : ???????????? ?????????? ????
            SaveDogData();
            OnlineCheck("Off");

        }
        else
        {
            if(bPaused)
            {
                bPaused = false;
                //todo : ???????? ?????? ???? ?????????? ???? ?? ??????
                OnlineCheck("On");

            }
        }
    }
    private void OnApplicationQuit()
    {
        SaveDogData();
        OnlineCheck("Off");
    }

    public void OnlineCheck(string OnOff)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "OnlineRequest",
            FunctionParameter = new { onlineState = OnOff },
            GeneratePlayStreamEvent = true,
        }, (result) => { print("?????? ???? ????"); }, (error) => print("?????? ???? ????"));
    }


    public void SaveDogData()
    {
        var reqeust = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                {"Dog1_Health", UserData.instance.Dog1_Health.ToString() }, {"Dog1_Hungry", UserData.instance.Dog1_Hungry.ToString()}, {"Dog1_Tirsty", UserData.instance.Dog1_Tirsty.ToString()}, {"Dog1_Clean",  UserData.instance.Dog1_Clean.ToString()}, {"Dog1_Happy",  UserData.instance.Dog1_Happy.ToString()},
                {"Dog2_Health", UserData.instance.Dog2_Health.ToString() }, {"Dog2_Hungry", UserData.instance.Dog2_Hungry.ToString()}, {"Dog2_Tirsty", UserData.instance.Dog2_Tirsty.ToString()}, {"Dog2_Clean",  UserData.instance.Dog2_Clean.ToString()}, {"Dog2_Happy",  UserData.instance.Dog2_Happy.ToString()},
                {"Dog3_Health", UserData.instance.Dog3_Health.ToString() }, {"Dog3_Hungry", UserData.instance.Dog3_Hungry.ToString()}, {"Dog3_Tirsty", UserData.instance.Dog3_Tirsty.ToString()}, {"Dog3_Clean",  UserData.instance.Dog3_Clean.ToString()}, {"Dog3_Happy",  UserData.instance.Dog3_Happy.ToString()}
            },
            Permission = UserDataPermission.Public

        };
        PlayFabClientAPI.UpdateUserData(reqeust, (result) => { print("User ?????? ???? ????"); }, (error) => { });
    }
}
