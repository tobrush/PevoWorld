using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using DanielLochner.Assets.SimpleScrollSnap;

public class PlayfabGuestLoginManager : MonoBehaviour
{
    public SimpleScrollSnap SSS;

    public RawImage CheckDog;
    public RenderTexture[] DogsRenderTextrue;
    public TMP_Text CheckYourDogName;

    public Transitioner transitioner;
    public Button LoginBtn, NewUserBtn, LinkSocialBtn, StartBtn;
    //private VivoxVoiceManager vivox;
    public Image WhiteBackGround;

    public GameObject Loading;
    public TMP_InputField InputID;
    public RectTransform SocialCheck, NameCheck, LocalCheck, PevoCheck, PevoApp, PetCard, CharacterChoice;

    public GoogleSheetManager GSM;

    [SerializeField]
    GetPlayerCombinedInfoRequestParams InfoRequestParams;

    public void Awake()
    {
        if (Application.isPlaying)
        {
            //EditorSceneManager.OpenScene(PreviousScene);
        }
        Application.targetFrameRate = 30;
        //vivox = VivoxVoiceManager.Instance;
    }

    public void Start()
    {

        if (UserData.instance.NotAutoLogin)
        {
            PlayerPrefs.DeleteKey("Sign");
        }
    
        InfoRequestParams.GetPlayerProfile = true;
        InfoRequestParams.GetUserData = true;
        InfoRequestParams.GetTitleData = true;
        InfoRequestParams.GetUserInventory = true;
        InfoRequestParams.GetUserVirtualCurrency = true;

        PlayFabAuthService.Instance.InfoRequestParams = InfoRequestParams;
        Loading.SetActive(false);
    }

    public void Login()
    {
        PlayFabAuthService.OnLoginSuccess += PlayFabAuthService_OnLoginSuccess;
        PlayFabAuthService.Instance.Authenticate(Authtypes.Silent);
        LoginBtn.interactable = false;
        LoginBtn.gameObject.SetActive(false);
        Loading.SetActive(true);
        transitioner.enabled = true;
    }

    private void PlayFabAuthService_OnLoginSuccess(LoginResult success)
    {
        Debug.Log("Login Success!");
        UserData.instance.User_MyID = success.PlayFabId;
        PhotoManager.instance.OnLogin(success);

        if (success.NewlyCreated)
        {
            //
        }
        else
        {
            //
        }

        if (PlayerPrefs.HasKey("Sign"))
        {
            UserData.instance.User_Name = success.InfoResultPayload.UserData["User_Name"].Value;
            UserData.instance.User_Local = success.InfoResultPayload.UserData["User_Local"].Value;
            UserData.instance.User_Hello = success.InfoResultPayload.UserData["User_Hello"].Value;
            UserData.instance.User_LikeCount = int.Parse(success.InfoResultPayload.UserData["User_LikeCount"].Value);
            UserData.instance.User_LikeIDs = success.InfoResultPayload.UserData["User_LikeIDs"].Value;
            UserData.instance.User_FriendsCount = int.Parse(success.InfoResultPayload.UserData["User_FriendsCount"].Value);
            UserData.instance.User_FriendsIDs = success.InfoResultPayload.UserData["User_FriendsIDs"].Value;
            UserData.instance.User_Inbox = success.InfoResultPayload.UserData["Inbox"].Value;
            UserData.instance.User_SaveTime = success.InfoResultPayload.UserData["SaveTime"].Value;
            UserData.instance.User_DogCount = int.Parse(success.InfoResultPayload.UserData["User_DogCount"].Value);
            UserData.instance.User_PevoOne = success.InfoResultPayload.UserData["User_PevoOne"].Value;

            // Dog1 Infor
            UserData.instance.Dog1_PevoNumber = success.InfoResultPayload.UserData["Dog1_PevoNumber"].Value;
            UserData.instance.Dog1_PetRgtNo = success.InfoResultPayload.UserData["Dog1_PetRgtNo"].Value;
            UserData.instance.Dog1_Character = success.InfoResultPayload.UserData["Dog1_Character"].Value;
            UserData.instance.Dog1_Name = success.InfoResultPayload.UserData["Dog1_Name"].Value;
            UserData.instance.Dog1_Style = success.InfoResultPayload.UserData["Dog1_Style"].Value;
            UserData.instance.Dog1_Age = success.InfoResultPayload.UserData["Dog1_Age"].Value;
            UserData.instance.Dog1_Adopted = success.InfoResultPayload.UserData["Dog1_Adopted"].Value;
            UserData.instance.Dog1_Weight = success.InfoResultPayload.UserData["Dog1_Weight"].Value;
            UserData.instance.Dog1_Sex = success.InfoResultPayload.UserData["Dog1_Sex"].Value;
            UserData.instance.Dog1_Neuter = success.InfoResultPayload.UserData["Dog1_Neuter"].Value;
            UserData.instance.Dog1_Food = success.InfoResultPayload.UserData["Dog1_Food"].Value;
            UserData.instance.Dog1_Place = success.InfoResultPayload.UserData["Dog1_Place"].Value;

            // Dog1 Condition
            UserData.instance.Dog1_Health = float.Parse(success.InfoResultPayload.UserData["Dog1_Health"].Value);
            UserData.instance.Dog1_Hungry = float.Parse(success.InfoResultPayload.UserData["Dog1_Hungry"].Value);
            UserData.instance.Dog1_Tirsty = float.Parse(success.InfoResultPayload.UserData["Dog1_Tirsty"].Value);
            UserData.instance.Dog1_Clean = float.Parse(success.InfoResultPayload.UserData["Dog1_Clean"].Value);
            UserData.instance.Dog1_Happy = float.Parse(success.InfoResultPayload.UserData["Dog1_Happy"].Value);

            // Dog1 Stats
            UserData.instance.Dog1_Level = int.Parse(success.InfoResultPayload.UserData["Dog1_Level"].Value);
            UserData.instance.Dog1_Exp = int.Parse(success.InfoResultPayload.UserData["Dog1_Exp"].Value);
            UserData.instance.Dog1_StatPoint = int.Parse(success.InfoResultPayload.UserData["Dog1_StatPoint"].Value);
            UserData.instance.Dog1_Speed = int.Parse(success.InfoResultPayload.UserData["Dog1_Speed"].Value);
            UserData.instance.Dog1_Power = int.Parse(success.InfoResultPayload.UserData["Dog1_Power"].Value);
            UserData.instance.Dog1_Stamina = int.Parse(success.InfoResultPayload.UserData["Dog1_Stamina"].Value);
            UserData.instance.Dog1_Sense = int.Parse(success.InfoResultPayload.UserData["Dog1_Sense"].Value);
            UserData.instance.Dog1_Guts = int.Parse(success.InfoResultPayload.UserData["Dog1_Guts"].Value);
            UserData.instance.Dog1_Lux = int.Parse(success.InfoResultPayload.UserData["Dog1_Lux"].Value);

            // Dog1 Equip
            UserData.instance.Dog1_item_Head = success.InfoResultPayload.UserData["Dog1_item_Head"].Value;
            UserData.instance.Dog1_item_Eye = success.InfoResultPayload.UserData["Dog1_item_Eye"].Value;
            UserData.instance.Dog1_item_Neck = success.InfoResultPayload.UserData["Dog1_item_Neck"].Value;
            UserData.instance.Dog1_item_Body = success.InfoResultPayload.UserData["Dog1_item_Body"].Value;


            if (UserData.instance.User_DogCount > 1)
            {
                // Dog2 Infor
                UserData.instance.Dog2_PevoNumber = success.InfoResultPayload.UserData["Dog2_PevoNumber"].Value;
                UserData.instance.Dog2_PetRgtNo = success.InfoResultPayload.UserData["Dog2_PetRgtNo"].Value;
                UserData.instance.Dog2_Character = success.InfoResultPayload.UserData["Dog2_Character"].Value;
                UserData.instance.Dog2_Name = success.InfoResultPayload.UserData["Dog2_Name"].Value;
                UserData.instance.Dog2_Style = success.InfoResultPayload.UserData["Dog2_Style"].Value;
                UserData.instance.Dog2_Age = success.InfoResultPayload.UserData["Dog2_Age"].Value;
                UserData.instance.Dog2_Adopted = success.InfoResultPayload.UserData["Dog2_Adopted"].Value;
                UserData.instance.Dog2_Weight = success.InfoResultPayload.UserData["Dog2_Weight"].Value;
                UserData.instance.Dog2_Sex = success.InfoResultPayload.UserData["Dog2_Sex"].Value;
                UserData.instance.Dog2_Neuter = success.InfoResultPayload.UserData["Dog2_Neuter"].Value;
                UserData.instance.Dog2_Food = success.InfoResultPayload.UserData["Dog2_Food"].Value;
                UserData.instance.Dog2_Place = success.InfoResultPayload.UserData["Dog2_Place"].Value;

                // Dog2 Condition
                UserData.instance.Dog2_Health = float.Parse(success.InfoResultPayload.UserData["Dog2_Health"].Value);
                UserData.instance.Dog2_Hungry = float.Parse(success.InfoResultPayload.UserData["Dog2_Hungry"].Value);
                UserData.instance.Dog2_Tirsty = float.Parse(success.InfoResultPayload.UserData["Dog2_Tirsty"].Value);
                UserData.instance.Dog2_Clean = float.Parse(success.InfoResultPayload.UserData["Dog2_Clean"].Value);
                UserData.instance.Dog2_Happy = float.Parse(success.InfoResultPayload.UserData["Dog2_Happy"].Value);

                // Dog2 Stats
                UserData.instance.Dog2_Level = int.Parse(success.InfoResultPayload.UserData["Dog2_Level"].Value);
                UserData.instance.Dog2_Exp = int.Parse(success.InfoResultPayload.UserData["Dog2_Exp"].Value);
                UserData.instance.Dog2_StatPoint = int.Parse(success.InfoResultPayload.UserData["Dog2_StatPoint"].Value);
                UserData.instance.Dog2_Speed = int.Parse(success.InfoResultPayload.UserData["Dog2_Speed"].Value);
                UserData.instance.Dog2_Power = int.Parse(success.InfoResultPayload.UserData["Dog2_Power"].Value);
                UserData.instance.Dog2_Stamina = int.Parse(success.InfoResultPayload.UserData["Dog2_Stamina"].Value);
                UserData.instance.Dog2_Sense = int.Parse(success.InfoResultPayload.UserData["Dog2_Sense"].Value);
                UserData.instance.Dog2_Guts = int.Parse(success.InfoResultPayload.UserData["Dog2_Guts"].Value);
                UserData.instance.Dog2_Lux = int.Parse(success.InfoResultPayload.UserData["Dog2_Lux"].Value);

                // Dog2 Equip
                UserData.instance.Dog2_item_Head = success.InfoResultPayload.UserData["Dog2_item_Head"].Value;
                UserData.instance.Dog2_item_Eye = success.InfoResultPayload.UserData["Dog2_item_Eye"].Value;
                UserData.instance.Dog2_item_Neck = success.InfoResultPayload.UserData["Dog2_item_Neck"].Value;
                UserData.instance.Dog2_item_Body = success.InfoResultPayload.UserData["Dog2_item_Body"].Value;
            }


            if (UserData.instance.User_DogCount > 2)
            {
                // Dog3 Infor
                UserData.instance.Dog3_PevoNumber = success.InfoResultPayload.UserData["Dog3_PevoNumber"].Value;
                UserData.instance.Dog3_PetRgtNo = success.InfoResultPayload.UserData["Dog3_PetRgtNo"].Value;
                UserData.instance.Dog3_Character = success.InfoResultPayload.UserData["Dog3_Character"].Value;
                UserData.instance.Dog3_Name = success.InfoResultPayload.UserData["Dog3_Name"].Value;
                UserData.instance.Dog3_Style = success.InfoResultPayload.UserData["Dog3_Style"].Value;
                UserData.instance.Dog3_Age = success.InfoResultPayload.UserData["Dog3_Age"].Value;
                UserData.instance.Dog3_Adopted = success.InfoResultPayload.UserData["Dog3_Adopted"].Value;
                UserData.instance.Dog3_Weight = success.InfoResultPayload.UserData["Dog3_Weight"].Value;
                UserData.instance.Dog3_Sex = success.InfoResultPayload.UserData["Dog3_Sex"].Value;
                UserData.instance.Dog3_Neuter = success.InfoResultPayload.UserData["Dog3_Neuter"].Value;
                UserData.instance.Dog3_Food = success.InfoResultPayload.UserData["Dog3_Food"].Value;
                UserData.instance.Dog3_Place = success.InfoResultPayload.UserData["Dog3_Place"].Value;

                // Dog3 Condition
                UserData.instance.Dog3_Health = float.Parse(success.InfoResultPayload.UserData["Dog3_Health"].Value);
                UserData.instance.Dog3_Hungry = float.Parse(success.InfoResultPayload.UserData["Dog3_Hungry"].Value);
                UserData.instance.Dog3_Tirsty = float.Parse(success.InfoResultPayload.UserData["Dog3_Tirsty"].Value);
                UserData.instance.Dog3_Clean = float.Parse(success.InfoResultPayload.UserData["Dog3_Clean"].Value);
                UserData.instance.Dog3_Happy = float.Parse(success.InfoResultPayload.UserData["Dog3_Happy"].Value);

                // Dog3 Stats
                UserData.instance.Dog3_Level = int.Parse(success.InfoResultPayload.UserData["Dog3_Level"].Value);
                UserData.instance.Dog3_Exp = int.Parse(success.InfoResultPayload.UserData["Dog3_Exp"].Value);
                UserData.instance.Dog3_StatPoint = int.Parse(success.InfoResultPayload.UserData["Dog3_StatPoint"].Value);
                UserData.instance.Dog3_Speed = int.Parse(success.InfoResultPayload.UserData["Dog3_Speed"].Value);
                UserData.instance.Dog3_Power = int.Parse(success.InfoResultPayload.UserData["Dog3_Power"].Value);
                UserData.instance.Dog3_Stamina = int.Parse(success.InfoResultPayload.UserData["Dog3_Stamina"].Value);
                UserData.instance.Dog3_Sense = int.Parse(success.InfoResultPayload.UserData["Dog3_Sense"].Value);
                UserData.instance.Dog3_Guts = int.Parse(success.InfoResultPayload.UserData["Dog3_Guts"].Value);
                UserData.instance.Dog3_Lux = int.Parse(success.InfoResultPayload.UserData["Dog3_Lux"].Value);

                // Dog3 Equip
                UserData.instance.Dog3_item_Head = success.InfoResultPayload.UserData["Dog3_item_Head"].Value;
                UserData.instance.Dog3_item_Eye = success.InfoResultPayload.UserData["Dog3_item_Eye"].Value;
                UserData.instance.Dog3_item_Neck = success.InfoResultPayload.UserData["Dog3_item_Neck"].Value;
                UserData.instance.Dog3_item_Body = success.InfoResultPayload.UserData["Dog3_item_Body"].Value;
            }

            CheckUser();
        }
        else
        {
            Loading.SetActive(false);
            NewUserBtn.gameObject.SetActive(true);
            LinkSocialBtn.gameObject.SetActive(true);
        }

    }

    public void SocialCheckClose()
    {
        SocialCheck.DOAnchorPos(new Vector2(2000, 0), 0.3f);
    }
    public void SocialCheckOpen()
    {
        SocialCheck.DOAnchorPos(Vector2.zero, 0.3f);
        //socical Link Btn
    }


    public void NameCheckClose()
    {
        NameCheck.DOAnchorPos(new Vector2(2000,0), 0.3f);
        InputID.text = "";

        //TODO : User Name empty
    }
    public void NameCheckOpen()
    {
        NameCheck.DOAnchorPos(Vector2.zero, 0.3f);
        //TODO : UserData.instance.User_Name
       
    }

    public void LocalCheckClose()
    {
        LocalCheck.DOAnchorPos(new Vector2(2000, 0), 0.3f);
    }
    public void LocalCheckOpen()
    {
        LocalCheck.DOAnchorPos(Vector2.zero, 0.3f);
        //TODO : UserData.instance.User_Local
    }

    public void PevoCheckClose()
    {
        PevoCheck.DOAnchorPos(new Vector2(2000, 0), 0.3f);
    }
    public void PevoCheckOpen()
    {
        PevoCheck.DOAnchorPos(Vector2.zero, 0.3f);
    }

    public void PevoAppClose()
    {
        PevoApp.DOAnchorPos(new Vector2(2000, 0), 0.3f);
    }
    public void PevoAppOpen()
    {
        PevoApp.DOAnchorPos(Vector2.zero, 0.3f);
    }
    public void PetCardClose()
    {
        PetCard.DOAnchorPos(new Vector2(2000, 0), 0.3f);
    }
    public void PetCardOpen()
    {
        PetCard.DOAnchorPos(Vector2.zero, 0.3f);
    }

    public void CharacterChoiceClose()
    {
        CharacterChoice.DOAnchorPos(new Vector2(2000, 0), 0.3f); print(2+ SSS.SelectedPanel);
    }
    public void CharacterChoiceOpen()
    {
        CharacterChoice.DOAnchorPos(Vector2.zero, 0.3f);
    }

    public void CharacterCehck()
    {
        
        CheckYourDogName.text = UserData.instance.User_Name + " 님의 " + "\n" + "<size=80>" + UserData.instance.Dog1_Name + "</size>";

        switch (2 + SSS.SelectedPanel)
        {
            case 2:
                CheckDog.texture = DogsRenderTextrue[0];
                break;
            case 3:
                CheckDog.texture = DogsRenderTextrue[1];
                break;
            case 4:
                CheckDog.texture = DogsRenderTextrue[2];
                break;
            case 5:
                CheckDog.texture = DogsRenderTextrue[3];
                break;
            case 6:
                CheckDog.texture = DogsRenderTextrue[4];
                break;
            case 7:
                CheckDog.texture = DogsRenderTextrue[5];
                break;
            case 8:
                CheckDog.texture = DogsRenderTextrue[6];
                break;
            case 9:
                CheckDog.texture = DogsRenderTextrue[7];
                break;
            case 10:
                CheckDog.texture = DogsRenderTextrue[8];
                break;
            case 11:
                CheckDog.texture = DogsRenderTextrue[9];
                break;
            case 12:
                CheckDog.texture = DogsRenderTextrue[10];
                break;
            case 13:
                CheckDog.texture = DogsRenderTextrue[11];
                break;
            case 14:
                CheckDog.texture = DogsRenderTextrue[12];
                break;
            default:
                CheckDog.texture = DogsRenderTextrue[0];
                break;
        }
     
    }

    public void PevoWorldStart()
    {
        UserData.instance.Dog1_Character = (2 + SSS.SelectedPanel).ToString();

        GSM.Register();
        
        StartBtn.interactable = false;
        WhiteBackGround.raycastTarget = true;
        WhiteBackGround.maskable = true;
        WhiteBackGround.DOColor(new Color(1, 1, 1, 1), 3.0f);
        // loading start
    }



    public void MakeNewSlot00()
    {
        UserData.instance.User_LikeCount = 0;
        UserData.instance.User_FriendsCount = 0;
        UserData.instance.User_DogCount = 1;

        var reqeust = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
                {
                    {"User_Name", UserData.instance.User_Name},
                    {"User_Local", UserData.instance.User_Local},
                    {"User_Hello", "안녕하세요. 저는 " + UserData.instance.User_Name + "입니다. 반가워요!"},
                    {"User_LikeCount", "0"},
                    {"User_LikeIDs", ""},
                    {"User_FriendsCount", "0"},
                    {"User_FriendsIDs", ""},
                    {"User_DogCount", "1"},
                    {"Dog1_PevoNumber", UserData.instance.Dog1_PevoNumber},
                    {"Dog1_Name", UserData.instance.Dog1_Name}
                },
            Permission = UserDataPermission.Public
        };
        PlayFabClientAPI.UpdateUserData(reqeust, (result) => { MakeNewSlot01(); }, (error) => print("User ?????? ???? ????"));

    }

    public void MakeNewSlot01()
    {
        UserData.instance.Dog1_Health = 50;
        UserData.instance.Dog1_Hungry = 50;
        UserData.instance.Dog1_Tirsty = 50;

        var reqeust1 = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
                {
                    {"Dog1_Style", UserData.instance.Dog1_Style},
                    {"Dog1_Age", UserData.instance.Dog1_Age},
                    {"Dog1_Weight", UserData.instance.Dog1_Weight},
                    {"Dog1_Sex", UserData.instance.Dog1_Sex},
                    {"Dog1_Neuter", UserData.instance.Dog1_Neuter},
                    {"Dog1_Food", ""},
                    {"Dog1_Place", ""},
                    {"Dog1_Health", "50"},
                    {"Dog1_Hungry", "50"},
                    {"Dog1_Tirsty", "50"}
                },
            Permission = UserDataPermission.Public

        };
        PlayFabClientAPI.UpdateUserData(reqeust1, (result) => { MakeNewSlot02(); }, (error) => print("User ?????? ???? ????"));

    }
    public void MakeNewSlot02()
    {
        UserData.instance.Dog1_Clean = 50;
        UserData.instance.Dog1_Happy = 50;
        UserData.instance.Dog1_Level = 1;
        UserData.instance.Dog1_Exp = 0;
        UserData.instance.Dog1_StatPoint = 0;
        UserData.instance.Dog1_Speed = 1;
        UserData.instance.Dog1_Power = 1;
        UserData.instance.Dog1_Stamina = 1;
        UserData.instance.Dog1_Guts = 1;

        var reqeust2 = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
                {
                    {"Dog1_Clean", "50"},
                    {"Dog1_Happy", "50"},
                    {"Dog1_Level", "1"},
                    {"Dog1_Exp", "0"},
                    {"Dog1_StatPoint", "0"},
                    {"Dog1_Speed", "1"},
                    {"Dog1_Power", "1"},
                    {"Dog1_Stamina", "1"},
                    {"Dog1_Sense", "1"},
                    {"Dog1_Guts", "1"}
                },
            Permission = UserDataPermission.Public

        };
        PlayFabClientAPI.UpdateUserData(reqeust2, (result) => { MakeNewSlot03(); }, (error) => print("User ?????? ???? ????"));

    }
    public void MakeNewSlot03()
    {
        UserData.instance.Dog1_Lux = 1;

        var reqeust3 = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
                {
                    {"Dog1_Lux", "1"},
                    {"Dog1_item_Head", ""},
                    {"Dog1_item_Eye", ""},
                    {"Dog1_item_Neck", ""},
                    {"Dog1_item_Body", ""},
                    {"Dog2_PevoNumber", ""},
                    {"Dog2_Name", ""},
                    {"Dog2_Style", ""},
                    {"Dog2_Age", ""},
                    {"Dog2_Weight", ""}
                },
            Permission = UserDataPermission.Public

        };
        PlayFabClientAPI.UpdateUserData(reqeust3, (result) => { MakeNewSlot04(); }, (error) => print("User ?????? ???? ????"));

    }

    public void MakeNewSlot04()
    {
        UserData.instance.Dog2_Health = 50;
        UserData.instance.Dog2_Hungry = 50;
        UserData.instance.Dog2_Tirsty = 50;
        UserData.instance.Dog2_Clean = 50;
        UserData.instance.Dog2_Happy = 50;
        UserData.instance.Dog2_Level = 1;

        var reqeust4 = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
                {
                    {"Dog2_Sex", ""},
                    {"Dog2_Neuter", ""},
                    {"Dog2_Food", ""},
                    {"Dog2_Place", ""},
                    {"Dog2_Health", "50"},
                    {"Dog2_Hungry", "50"},
                    {"Dog2_Tirsty", "50"},
                    {"Dog2_Clean", "50"},
                    {"Dog2_Happy", "50"},
                    {"Dog2_Level", "1"}
                },
            Permission = UserDataPermission.Public

        };
        PlayFabClientAPI.UpdateUserData(reqeust4, (result) => { MakeNewSlot05(); }, (error) => print("User ?????? ???? ????"));

    }

    public void MakeNewSlot05()
    {
        UserData.instance.Dog2_Exp = 0;
        UserData.instance.Dog2_StatPoint = 0;
        UserData.instance.Dog2_Speed = 1;
        UserData.instance.Dog2_Power = 1;
        UserData.instance.Dog2_Stamina = 1;
        UserData.instance.Dog2_Sense = 1;
        UserData.instance.Dog2_Guts = 1;
        UserData.instance.Dog2_Lux = 1;


        var reqeust5 = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
                {
                    {"Dog2_Exp", "0"},
                    {"Dog2_StatPoint", "0"},
                    {"Dog2_Speed", "1"},
                    {"Dog2_Power", "1"},
                    {"Dog2_Stamina", "1"},
                    {"Dog2_Sense", "1"},
                    {"Dog2_Guts", "1"},
                    {"Dog2_Lux", "1"},
                    {"Dog2_item_Head", ""},
                    {"Dog2_item_Eye", ""}
                },
            Permission = UserDataPermission.Public

        };
        PlayFabClientAPI.UpdateUserData(reqeust5, (result) => { MakeNewSlot06(); }, (error) => print("User ?????? ???? ????"));

    }
    public void MakeNewSlot06()
    {

        var reqeust6 = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
                {
                    {"Dog2_item_Neck", ""},
                    {"Dog2_item_Body", ""},
                    {"Dog3_PevoNumber", ""},
                    {"Dog3_Name", ""},
                    {"Dog3_Style", ""},
                    {"Dog3_Age", ""},
                    {"Dog3_Weight", ""},
                    {"Dog3_Sex", ""},
                    {"Dog3_Neuter", ""},
                    {"Dog3_Food", ""}

                },
            Permission = UserDataPermission.Public

        };
        PlayFabClientAPI.UpdateUserData(reqeust6, (result) => { MakeNewSlot07(); }, (error) => print("User ?????? ???? ????"));

    }
    public void MakeNewSlot07()
    {
        UserData.instance.Dog3_Health = 50;
        UserData.instance.Dog3_Hungry = 50;
        UserData.instance.Dog3_Tirsty = 50;
        UserData.instance.Dog3_Clean = 50;
        UserData.instance.Dog3_Happy = 50;
        UserData.instance.Dog3_Level = 1;
        UserData.instance.Dog3_Exp = 0;
        UserData.instance.Dog3_StatPoint = 0;
        UserData.instance.Dog3_Speed = 1;

        var reqeust7 = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
                {
                    {"Dog3_Place", ""},
                    {"Dog3_Health", "50"},
                    {"Dog3_Hungry", "50"},
                    {"Dog3_Tirsty", "50"},
                    {"Dog3_Clean", "50"},
                    {"Dog3_Happy", "50"},
                    {"Dog3_Level", "1"},
                    {"Dog3_Exp", "0"},
                    {"Dog3_StatPoint", "0"},
                    {"Dog3_Speed", "1"}
                },
            Permission = UserDataPermission.Public

        };
        PlayFabClientAPI.UpdateUserData(reqeust7, (result) => { MakeNewSlot08(); }, (error) => print("User ?????? ???? ????"));

    }
    public void MakeNewSlot08()
    {
        UserData.instance.Dog3_Power = 1;
        UserData.instance.Dog3_Stamina = 1;
        UserData.instance.Dog3_Sense = 1;
        UserData.instance.Dog3_Guts = 1;
        UserData.instance.Dog3_Lux = 1;
        UserData.instance.User_PevoOne = "0";



        var reqeust8 = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
                {
                    {"Dog3_Power", "1"},
                    {"Dog3_Stamina", "1"},
                    {"Dog3_Sense", "1"},
                    {"Dog3_Guts", "1"},
                    {"Dog3_Lux", "1"},
                    {"Dog3_item_Head", ""},
                    {"Dog3_item_Eye", ""},
                    {"Dog3_item_Neck", ""},
                    {"Dog3_item_Body", "" },
                    {"User_PevoOne", "0"}
                },
            Permission = UserDataPermission.Public

        };
        PlayFabClientAPI.UpdateUserData(reqeust8, (result) => { MakeNewSlot09(); }, (error) => print("User ?????? ???? ????"));

    }

    public void MakeNewSlot09()
    {
        string PetRgtNo;
        if (UserData.instance.Dog1_PetRgtNo == null)
        {
            PetRgtNo = "";
        }
        else
        {
            PetRgtNo = UserData.instance.Dog1_PetRgtNo.ToString();
        }


        var reqeust9 = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            { 
                //add
                {"Dog1_Character", UserData.instance.Dog1_Character},
                {"Dog1_PetRgtNo", PetRgtNo},
                {"Dog1_Adopted", UserData.instance.Dog1_Adopted},
                {"Dog2_Character", ""},
                {"Dog2_PetRgtNo", ""},
                {"Dog2_Adopted", ""},
                {"Dog3_Character", ""},
                {"Dog3_PetRgtNo", ""},
                {"Dog3_Adopted", "" },
                {"User_EntityID", PhotoManager.instance.entityId }

            },
            Permission = UserDataPermission.Public

        };
        PlayFabClientAPI.UpdateUserData(reqeust9, (result) => { SignUpComplite(); }, (error) => print("User ?????? ???? ????"));

    }
    public void SignUpComplite()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "MakeNewUser",
            GeneratePlayStreamEvent = true,
        }, (result) => { PlayerPrefs.SetString("Sign", "Sign"); SetDisplayName(); }, (error) => { print("New User Failed !!"); });

        //Debug.Log("Have New User");

    }

    public void SetDisplayName()
    {
        var request = new UpdateUserTitleDisplayNameRequest { DisplayName = UserData.instance.User_Name };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, (result) => { CheckUser(); }, (error) => print("DisplayName ?????? ???? ????"));
    }




    public void CheckUser()
    {
        if (UserData.instance.User_Inbox == "")
        {
            //check User_SaveTime, User_Inbox;
            PlayFabClientAPI.GetUserData(new GetUserDataRequest()
            {
                PlayFabId = UserData.instance.User_MyID
            }, result => {
                UserData.instance.User_Inbox = result.Data["Inbox"].Value;
                UserData.instance.User_SaveTime = result.Data["SaveTime"].Value;

                Loading.SetActive(false);

                Transitioner.Instance.TransitionToScene("Main");


            }, error => {
                //Debug.Log(error.GenerateErrorReport());
            });
        }
        else
        {
            Loading.SetActive(false);
            Transitioner.Instance.TransitionToScene("Main");
        }

    }



    public void ReLogin()
    {
        UserData.instance.NotAutoLogin = false;

        PlayerPrefs.SetString("Sign", "Sign");

        Login();
    }
}
