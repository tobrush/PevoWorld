using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;


public class PlayfabLogin1 : MonoBehaviour
{

    [SerializeField]
    private string username;

    #region Unity Methods
    private void Start()
    {   
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

        if(username.Length >= 3 && username.Length <= 24)
        {
            isValid = true;
        }
        return isValid;
    }

    public void LoginWithCustomId()
    {
        Debug.Log($"Login to Playfab as {username}");
        var request = new LoginWithCustomIDRequest {CustomId = username, CreateAccount = true};
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginCustomIdSuccess, OnFailure);
    }

    public void OnLoginCustomIdSuccess(LoginResult result)
    {
        Debug.Log($"You have logged into Playfab using custom id{username}");
        UpdateDisplayNAme(username);
    }

    #endregion
    #region Public Methods
    public void SetUsername(string name)
    {
        username = name;
        PlayerPrefs.SetString("USERNAME", username);

    }
    public void UpdateDisplayNAme(string displayname)
    {
        Debug.Log($"Updating Playfab account's Display name to : {username}");
        var request = new UpdateUserTitleDisplayNameRequest { DisplayName = displayname};
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplaynameSuccess, OnFailure);
    }

    public void OnDisplaynameSuccess(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log($"You have updated the displayname of  the playfab account!");
        SceneController.LoadScene("MainMenu");
    }

    #endregion

        #region Playfab Callbacks

        private void OnFailure(PlayFabError error)
    {
        Debug.Log($"There was an issue with your request : {error.GenerateErrorReport()}");
    }
    #endregion
}
