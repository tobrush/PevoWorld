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
        , result => Debug.Log("Login Success��")
        , error => Debug.Log("Login Fail..."));
        */
        InfoRequestParams.GetTitleData = true; // Ÿ��Ʋ ������ ���
        InfoRequestParams.GetUserInventory = true; // �÷��̾� �κ��丮 ���
        InfoRequestParams.GetUserVirtualCurrency = true; // �÷��̾��� ������ȭ ���
        InfoRequestParams.GetPlayerStatistics = true; // �÷����̾��� ��� ���
        InfoRequestParams.GetCharacterList = true; // ĳ���� ��� ���

        InfoRequestParams.GetUserData = true; // �÷��̾� ������ ���

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


        // �÷��̾��� ���� ������ ��������
        //PlayerName = result.InfoResultPayload.UserData["Name"].Value;

        // ĳ������ �ϳ��� ��´�.
        //Characters = result.InfoResultPayload.CharacterList;

        // Ÿ��Ʋ ������ ���
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
