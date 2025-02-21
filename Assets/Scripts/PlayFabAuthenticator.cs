using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayFabAuthenticator : MonoBehaviour
{

    private string _playFabPlayerIdCache;

    public void Awake()
    {
        AuthenticateWithPlayFab();
    }


    // 1. 현재 Playfab 사용자 인증 / 단순화를 위해 LoginWithCustomID API 호출 / 이단계에서 원하는 다른 로그인방법 사용가능 / PlayFabSettings.DeviceUniqueIdentifier를 사용자 지정 ID로 사용 / 다음단계가 될 콜백으로 RequestPhotonToken 전달 / 인증성공
    private void AuthenticateWithPlayFab()
    {
        LogMessage("PlayFab authenticating using Custom ID...");

        PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        {
            CreateAccount = true,
            CustomId = PlayFabSettings.DeviceUniqueIdentifier
        }, RequestPhotonToken, OnPlayFabError);
    }


    // 2. PlayFab에서 Photon 인증 토큰을 요청 / Photon은 다른 인증 토큰을 사용하기 때문에 이것은 중요 / PlayFab에서는 PlayFab SessionTicket을 직접 사용불가 / 명시적 토큰 요청해야함 / 이 API호출은 다음에 수행해야함 / Photon 앱 ID 전달 / PhotonNetwork클래스에 편리한 정적필드 사용하여 엑세스 / 콜백으로 AuthenticateWithPhoton을 전달 / 토큰 성공적 획득  
    private void RequestPhotonToken(LoginResult obj)
    {
        LogMessage("PlayFab authenticated. Requesting photon token...");

        _playFabPlayerIdCache = obj.PlayFabId;

        PlayFabClientAPI.GetPhotonAuthenticationToken(new GetPhotonAuthenticationTokenRequest()
        {
            PhotonApplicationId = "024b5b66-2950-4c8c-9090-b2d9d4bd6cd2"
        }, AuthenticateWithPhoton, OnPlayFabError);
    }

    // 3. 새로운 AuthenticationValue 인스턴스를 생성 / 이 클래스는 Photon 환경 내에서 플레이어를 인증하는 방법을 설명
    private void AuthenticateWithPhoton(GetPhotonAuthenticationTokenResult obj)
    {
        LogMessage("Photon token acquired: " + obj.PhotonCustomAuthenticationToken + "  Authentication complete.");

        // AuthType을 사용자 지정으로 설정합니다. 즉, 자체 PlayFab 인증 절차를 가져옵니다.
        var customAuth = new AuthenticationValues { AuthType = CustomAuthenticationType.Custom };
        // "username" 매개변수를 추가합니다. 혼동하지 마십시오. PlayFab은 이 매개 변수에 사용자 이름이 아닌 플레이어 PlayFab ID(!)가 포함될 것으로 예상합니다.
        customAuth.AddAuthParameter("username", _playFabPlayerIdCache);

        // "토큰" 매개변수를 추가합니다. PlayFab은 이전 단계에서 Photon 인증 토큰 문제를 포함할 것으로 예상합니다.
        customAuth.AddAuthParameter("token", obj.PhotonCustomAuthenticationToken);

        // 마지막으로 전체 애플리케이션에서 이 인증 매개변수를 사용하도록 Photon에 지시합니다.
        PhotonNetwork.AuthValues = customAuth;
    }

    private void OnPlayFabError(PlayFabError obj)
    {
        LogMessage(obj.GenerateErrorReport());
    }

    public void LogMessage(string message)
    {
        Debug.Log("PlayFab + Photon Example: " + message);
    }
}