using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Json;


public class PlayFabController : MonoBehaviour
{
    [SerializeField]
    GetPlayerCombinedInfoRequestParams InfoRequestParams;

    [HideInInspector] public string PlayerName { get; private set; }
    [HideInInspector] public List<CharacterResult> Characters { get; private set; }
    [HideInInspector] public List<ItemData> ItemDatas { get; private set; }
   

    void Start()
    {

        // Test Login
        /*
        PlayFabClientAPI.LoginWithCustomID(
            new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = true }
        , result => Debug.Log("Login Success！")
        , error => Debug.Log("Login Fail..."));
        */
        InfoRequestParams.GetTitleData = true; // 타이틀 데이터 얻기
        InfoRequestParams.GetUserInventory = true; // 플레이어 인벤토리 취득
        InfoRequestParams.GetUserVirtualCurrency = true; // 플레이어의 가상통화 취득
        InfoRequestParams.GetPlayerStatistics = true; // 플레이이어의 통계 취득
        InfoRequestParams.GetCharacterList = true; // 캐릭터 목록 얻기

        InfoRequestParams.GetUserData = true; // 플레이어 데이터 얻기

        PlayFabAuthService.Instance.InfoRequestParams = InfoRequestParams;
        PlayFabAuthService.OnLoginSuccess += PlayFabLogin_OnLoginSuccess;

        PlayFabAuthService.Instance.Authenticate(Authtypes.Silent);

    }
    void OnEnable()
    {
        PlayFabAuthService.OnLoginSuccess += PlayFabLogin_OnLoginSuccess;
    }
    private void PlayFabLogin_OnLoginSuccess(LoginResult result)
    {


        // 플레이어의 맞춤 데이터 가져오기
        //PlayerName = result.InfoResultPayload.UserData["Name"].Value;

        // 캐릭터의 하나를 얻는다.
        //Characters = result.InfoResultPayload.CharacterList;

        // 타이틀 데이터 취득
        ItemDatas = PlayFabSimpleJson.DeserializeObject<List<ItemData>>(result.InfoResultPayload.TitleData["ItemData"]);


    }
    private void OnDisable()
    {
        PlayFabAuthService.OnLoginSuccess -= PlayFabLogin_OnLoginSuccess;
    }


   void GetUserData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
           // PlayFabId = PlayFabId
        }, result => {
            Debug.Log(result.Data["Name"].Value);
        }, error => {
            Debug.Log(error.GenerateErrorReport());
        });
    }
}
