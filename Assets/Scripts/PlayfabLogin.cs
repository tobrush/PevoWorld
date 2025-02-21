using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;


public class PlayfabLogin : MonoBehaviour
{
    public PlayerLeaderboardEntry MyPlayFabInfo;
    public List<PlayerLeaderboardEntry> PlayFabUserList = new List<PlayerLeaderboardEntry>();

    public GameObject LoginPanel, SignUpPanel, SmallMenuPanel, LoadingPanel, ErrorPanel;
    public Text ErrorMessage;
    public InputField Login_EmailInput, Login_PasswordInput, SignUp_EmailInput, SignUp_PasswordInput, SignUp_UsernameInput, SignUp_PevoIDInput;

    #region Unity Methods
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        
        if(string.IsNullOrEmpty(PlayFabSettings.TitleId))
        {
            PlayFabSettings.TitleId = "D3CE6";
        }
    }

    #endregion

    #region Private Methods
    private bool IsValidUsername()
    {
        bool isValid = false;

        if(SignUp_UsernameInput.text.Length >= 3 && SignUp_UsernameInput.text.Length <= 24)
        {
            isValid = true;
        }
        return isValid;
    }

    public void LoginWithEmail()
    {
        Debug.Log($"Login to Playfab as {Login_EmailInput.text}");
        var request = new LoginWithEmailAddressRequest { Email = Login_EmailInput.text, Password = Login_PasswordInput.text };
        PlayFabClientAPI.LoginWithEmailAddress(request, (result) => { print("로그인 성공");  SceneChange(); }, (error) => { print(error.GenerateErrorReport()); retryLogin(error.GenerateErrorReport()); });
    }

    public void Register()
    {
        var request = new RegisterPlayFabUserRequest { Email = SignUp_EmailInput.text, Password = SignUp_PasswordInput.text, Username = SignUp_UsernameInput.text, DisplayName = SignUp_UsernameInput.text };
        PlayFabClientAPI.RegisterPlayFabUser(request, (result) => { print("회원가입 성공"); }, (error) => { print(error.GenerateErrorReport()); retrySignUp(error.GenerateErrorReport()); } );
    }

    #endregion
    #region Public Methods
    public void SetUsername(string name)
    {
        
        PlayerPrefs.SetString("Email", Login_EmailInput.text);

    }
    public void RegisterChecker() // 이걸로 회원가입 // 글자체크인데  수정해야함
    {
        if (!IsValidUsername()) return;

        Register(); 
    }

    public void PanelChange()
    {
        if(Login_EmailInput.text == "" && Login_PasswordInput.text == "")
        {
            LoginPanel.SetActive(false);
            SignUpPanel.SetActive(false);
            SmallMenuPanel.SetActive(false);
            LoadingPanel.SetActive(true);
        }
       
    }

    public void retryLogin(string error)
    {
        Debug.Log("RetryLogin");
        LoginPanel.SetActive(true);
        SignUpPanel.SetActive(false);
        SmallMenuPanel.SetActive(true);
        LoadingPanel.SetActive(false);
        ErrorPanel.SetActive(true);
        ErrorMessage.text = error;


    }
    public void retrySignUp(string error)
    {
        Debug.Log("RetrySignUp");
        LoginPanel.SetActive(false);
        SignUpPanel.SetActive(true);
        SmallMenuPanel.SetActive(true);
        LoadingPanel.SetActive(false);
        ErrorPanel.SetActive(true);
        ErrorMessage.text = error;

    }

    public void SceneChange()
    {

        SceneController.LoadScene("Main");
    }

        #endregion




        #region Playfab Callbacks

        private void OnFailure(PlayFabError error)
    {
        Debug.Log($"There was an issue with your request : {error.GenerateErrorReport()}");
    }
    #endregion
}
