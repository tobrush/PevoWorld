using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using TMPro;

public class InforManager : MonoBehaviour
{

    public GameObject WarningWindowUI;
    public Text WarningText;
    private Censor censor;
    public Button OkayBtn;

    public TMP_InputField PevoNumbuer_input;
    public TMP_InputField UserName_input;
    public TMP_InputField Local_input;
    public TMP_InputField Hello_input;
    public TMP_InputField DogName_input;
    public TMP_InputField DogStyle_input;
    public TMP_InputField DogAge_input;
    public TMP_Dropdown DogAge_Year;
    public TMP_Dropdown DogAge_Month;
    public TMP_Dropdown DogAge_Day;
    public TMP_InputField DogWeight_input;

    public Toggle DogSexMale;
    public Toggle DogNeuterOff;

    public string AppUrl;

    public TMP_InputField NewInput;
    public GameObject LoadingAnim;

    public void Start()
    {
        censor = GetComponent<Censor>();
    }

    public void inforGetString()
    {
        OkayBtn.interactable = false;
        if (UserName_input.text != "" && Local_input.text != "" && DogName_input.text != "" && DogStyle_input.text != "" && DogWeight_input.text != "")
        {
            if (UserName_input.text.Length >= 3)
            {
                UserData.instance.User_Name = UserName_input.text;
                UserData.instance.User_Local = Local_input.text;
                UserData.instance.User_Hello = Hello_input.text;

                UserData.instance.Dog1_PevoNumber = PevoNumbuer_input.text;
                UserData.instance.Dog1_Name = DogName_input.text;
                UserData.instance.Dog1_Style = DogStyle_input.text;
                //UserData.instance.Dog1_Age = DogAge_input.text;
                UserData.instance.Dog1_Age = DogAge_Year.itemText.name + DogAge_Month.itemText.name + DogAge_Day.itemText.name;
                UserData.instance.Dog1_Weight = DogWeight_input.text;

                UserData.instance.Dog1_Sex = DogSexMale ? "1" : "2";
                UserData.instance.Dog1_Neuter = DogNeuterOff ? "0" : "1";

                SetData();
            }
            else
            {
                WarningText.text = "※ 사용자 이름은 3글자 이상이어야 합니다. (공백포함)";
                WarningWindowUI.SetActive(true);
                OkayBtn.interactable = true;
            }

        }
        else
        {
            WarningText.text = "※ 올바른 데이터를 입력해주세요.";
            WarningWindowUI.SetActive(true);
            OkayBtn.interactable = true;
        }

    }


    public void SetData()
    {
        var reqeust = new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                {"User_Name", UserData.instance.User_Name},
                {"User_Local", UserData.instance.User_Local},
                {"User_Hello", UserData.instance.User_Hello},
                {"Dog1_PevoNumber", UserData.instance.Dog1_PevoNumber},
                {"Dog1_Name", UserData.instance.Dog1_Name},
                {"Dog1_Style", UserData.instance.Dog1_Style},
                {"Dog1_Age", UserData.instance.Dog1_Age},
                {"Dog1_Weight", UserData.instance.Dog1_Weight},
                {"Dog1_Sex", UserData.instance.Dog1_Sex},
                {"Dog1_Neuter", UserData.instance.Dog1_Neuter}
            },
            Permission = UserDataPermission.Public

        };
        PlayFabClientAPI.UpdateUserData(reqeust, (result) => { print("User 데이터 저장 성공"); privateSetData(); }, (error) => print("User 데이터 저장 실패"));



    }

    public void privateSetData()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "MakeNewUser",
            GeneratePlayStreamEvent = true,
        }, (result) => { print("We Have New User !!"); }, (error) => { print("New User Failed !!"); });

        SetName();

    }

    public void SetName()
    {
        var request = new UpdateUserTitleDisplayNameRequest { DisplayName = UserData.instance.User_Name };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, (result) => { print("DisplayName 데이터 저장 성공"); StartPevoWorld(); }, (error) => print("DisplayName 데이터 저장 실패"));
    }


    public void StartPevoWorld()
    {
        Transitioner.Instance.transform.GetChild(0).gameObject.SetActive(true);
        Transitioner.Instance.TransitionToScene("Main");
    }


    public void KeyFilter(TMP_InputField thisObj) // string 매개변수는 기본으로 들어가는 매개변수이다
    {
        thisObj.text = censor.CensorText(thisObj.text);
       
        StartCoroutine(WarningWindow(thisObj));
    }

    IEnumerator WarningWindow(TMP_InputField thisObj)
    {
      
        yield return new WaitForSeconds(0.1f);

        string star = "*";

        if (thisObj.text.Contains(star))
        {
            LoadingAnim.SetActive(false);
            thisObj.text = "";
            WarningText.text = "※ 비속어 사용은 금지합니다.";
            WarningWindowUI.SetActive(true);


        }
        else
        {
            if (NewInput.text != "")
            {
                LoadingAnim.SetActive(true);
            }
        }

    }

    private void OnApplicationQuit()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest()
        {
            FunctionName = "DeleteUser",
            GeneratePlayStreamEvent = true,
        }, (result) => { }, (error) => { });
    }

    public void DownloadPevoApp()
    {
#if UNITY_EDITOR
        AppUrl = "https://play.google.com/store/apps/details?id=com.ddcares.pevo";
#elif UNITY_IPHONE
        AppUrl = "https://apps.apple.com/kr/app/%ED%8E%98%EB%B3%B4/id1502216647";
#elif UNITY_ANDROID
        AppUrl = "https://play.google.com/store/apps/details?id=com.ddcares.pevo";
#endif
        Application.OpenURL(AppUrl);
    }
    




    public void inputStart()
    {
        LoadingAnim.SetActive(false);
    }


}


